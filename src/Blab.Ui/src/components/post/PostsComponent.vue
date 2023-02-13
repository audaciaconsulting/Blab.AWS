<template>
  <div class="posts-parent-container">
    <div class="posts-container" v-if="props.posts.length">
      <PostComponent
        @openDeleteModal="openDeletePostModal"
        v-for="post in props.posts"
        :key="post.id"
        :post="post"
        :is-on-my-user-page="isOnMyUserPage"
        @added-reaction="
          (postId, newReaction) => emit('addedReaction', postId, newReaction)
        "
        @updated-reaction="
          (postId, newReaction) => emit('updatedReaction', postId, newReaction)
        "
        @deleted-reaction="(postId) => emit('deletedReaction', postId)"
      />
      <DeletePostModal
        v-if="postIdToDelete"
        :profileDeleteState="showModal"
        :post-id="postIdToDelete"
        @closeModal="closeDeletePostModal"
        @refresh-blabs="refreshBlabs"
      />
    </div>
    <div class="message" v-if="props.noBlabsMessage">
      {{ props.noBlabsMessage }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref, type PropType, type Ref } from "vue";
import DeletePostModal from "@/components/user-profile/posts/Delete/DeletePostModal.vue";
import PostComponent from "@/components/post/PostComponent.vue";
import type { IPost } from "@/models/posts/post.interface";
import type { Reaction } from "@/models/reactions/reaction.enum";

const props = defineProps({
  posts: { required: true, type: Array as PropType<IPost[]> },
  noBlabsMessage: { required: false, type: String },
  isLoading: { required: false, type: Boolean },
  deletePostState: { required: false },
  openDeletePostModal: { required: false, type: Number },
  isOnMyUserPage: { required: true, type: Boolean },
});
const emit = defineEmits<{
  (e: "addedReaction", blabId: number, reaction: Reaction): void;
  (e: "updatedReaction", blabId: number, reaction: Reaction): void;
  (e: "deletedReaction", blabId: number): void;
  (e: "isOnMyUserPage", required: true, type: Boolean): void;
  (e: "changeLoadingStateToFalse"): void;
  (e: "refreshBlabs", postId: number): void;
}>();

const showModal: Ref<boolean> = ref(false);
const postIdToDelete: Ref<number | null> = ref(null);
// The observer will check to see if the height of the post feed has changed if it has the loading state can be set to false so the scroll event can be activated
const observer = new ResizeObserver((entries) => {
  // This will stop loading if the size changes of the element
  if (props.posts.length && props.isLoading && entries[0]) {
    emit("changeLoadingStateToFalse");
  }
});
onMounted(() => {
  const postsParentContainer = document.querySelector(
    ".posts-parent-container"
  );
  if (postsParentContainer) {
    // adds the observer if the parent for the post container is truthy
    observer.observe(postsParentContainer);
  }
});

onUnmounted(() => {
  // removes the observer when unmounted
  observer.disconnect();
});

function openDeletePostModal(postId: number): void {
  postIdToDelete.value = postId;
  showModal.value = true;
}
function closeDeletePostModal(): void {
  postIdToDelete.value = null;
  showModal.value = false;
}

function refreshBlabs(postId: number): void {
  emit("refreshBlabs", postId);
}
</script>

<style scoped lang="scss">
.posts-parent-container {
  width: 100%;
  height: 100%;
}
.posts-container {
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  gap: 50px;
  padding-top: 20px;
}

.message {
  display: flex;
  justify-content: center;
}
</style>
