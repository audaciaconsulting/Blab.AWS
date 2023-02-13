<template>
  <div class="posts-container" v-if="props.posts.length">
    <PostComponent
      @openDeleteModal="openDeletePostModal"
      v-for="(post, index) in props.posts"
      :key="index"
      :post="post"
      :is-on-my-user-page="isOnMyUserPage"
    />
    <DeletePostModal
      :profileDeleteState="showModal"
      @closeModal="closeDeletePostModal"
      :post-id="(postIdToDelete as number)"
      @refreshProfileAfterDelete="refreshProfileAfterDelete"
    />
  </div>
  <div v-else>No blabs yet, create a new blab to see yours here.</div>
</template>

<script setup lang="ts">
import DeletePostModal from "@/components/user-profile/posts/Delete/DeletePostModal.vue";
import { ref, type PropType, type Ref } from "vue";
import PostComponent from "@/components/post/PostComponent.vue";
import type { IUserProfileDetails } from "@/models/user-profile/user-profile-details.interface";
import { useProfileStore } from "@/stores/user-profile.store";
import type { IPost } from "@/models/posts/post.interface";

const userStore = useProfileStore();

const profileUserDetails: Ref<IUserProfileDetails> = ref({
  userId: 0,
  handle: "loading handle...",
  bio: "loading bio.....",
  displayName: "loading display name",
  blabs: [],
  isFollowing: null,
  profilePhoto: null,
  backgroundPhoto: null,
});
const showModal: Ref<boolean> = ref(false);
const postIdToDelete: Ref<number | null> = ref(null);
const props = defineProps({
  posts: { required: true, type: Array as PropType<IPost[]> },
  isOnMyUserPage: { required: true, type: Boolean },
});

function openDeletePostModal(postId: number): void {
  postIdToDelete.value = postId;
  showModal.value = true;
}
function closeDeletePostModal(): void {
  postIdToDelete.value = null;
  showModal.value = false;
}
function refreshProfileAfterDelete(): void {
  profileUserDetails.value = userStore.userDetails;
}
</script>

<style scoped lang="scss">
.posts-container {
  height: auto;
  width: 75%;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  gap: 50px;
  margin: 50px 0;
}
</style>
