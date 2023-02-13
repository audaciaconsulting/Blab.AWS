<template>
  <transition>
    <!-- This is the parent/container for the delete blab (post) section -->
    <div class="delete-post-container" v-if="props.profileDeleteState">
      <div class="blur-background"></div>

      <div @click="deletePostState"></div>
      <div class="delete-blab">
        <!-- This contains the question "Are you sure you want to delete this blab?"" -->
        <DeletePostHeader @openDeleteModal="deletePostState" />
        <!-- This contains the yes and no buttons -->
        <DeletePostForm @close-modal="closeModal" @delete-blab="deleteBlab" />
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import DeletePostHeader from "@/components/user-profile/posts/Delete/DeletePostHeader.vue";
import DeletePostForm from "@/components/user-profile/posts/Delete/DeletePostForm.vue";
import { PostService } from "@/services/posts/post.service";
import { useNotification } from "@kyvg/vue3-notification";
import { useRouter } from "vue-router";
import { useProfileStore } from "@/stores/user-profile.store";

const router = useRouter();
const { notify } = useNotification();
const userStore = useProfileStore();
const postService = new PostService();

const props = defineProps({
  profileDeleteState: { required: true, type: Boolean },
  postId: { required: true, type: Number as () => number },
});
const emit = defineEmits<{
  (e: "openDeleteModal"): void;
  (e: "closeModal"): void;
  (e: "deleteBlab", type: number): void;
  (e: "refreshBlabs", postId: number): void;
}>();
function deletePostState() {
  emit("openDeleteModal");
}
function closeModal(): void {
  emit("closeModal");
}

function deleteBlab(): void {
  if (props.postId !== null) {
    postService.deletePost(props.postId).then((res) => {
      if (res.statusCode === 200) {
        notify("Blab was deleted!");
        emit("closeModal");
        router.push({
          name: "UserProfile",
          params: { handle: userStore.userDetails.handle },
        });
        refreshProfileAfterDelete();
      } else {
        notify("Something went wrong please try again...");
      }
    });
  }
}
// function looks to the store to return the array of blabs that does not include the deleted blab.
// the $patch looks at computed method in userProfielView to call from the store.
function refreshProfileAfterDelete(): void {
  userStore.$patch((state) => {
    state.userDetails.blabs = state.userDetails.blabs.filter((post) => {
      return post.id !== props.postId;
    });
  });

  emit("refreshBlabs", props.postId);
}
</script>

<style scoped lang="scss">
.delete-post-container {
  top: calc(50% - 40vmin);
  left: calc(50% - 50vmin);
  width: 100vmin;
  max-height: 80vmin;
  display: flex;
  justify-content: flex-start;
  flex-direction: column;
  align-items: center;
  background-color: var(--background-color);
  padding: 1rem;
  position: fixed;
  gap: 20px;
  z-index: 100;
  border-radius: var(--border-radius-amount);
}
.delete-blab {
  top: calc(50% - 40vmin);
  left: calc(50% - 50vmin);
  width: 100vmin;
  max-height: 80vmin;
  display: flex;
  justify-content: flex-start;
  flex-direction: column;
  align-items: center;
  background-color: var(--background-color);
  padding: 1rem;
  position: fixed;
  gap: 20px;
  z-index: 100;
  border-radius: var(--border-radius-amount);
}
</style>
