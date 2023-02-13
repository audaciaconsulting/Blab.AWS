<script setup lang="ts">
import type { ApiResponse } from "@/models/api/api-response.model";
import { AddComment } from "@/models/comments/add-comment.model";
import commentService from "@/services/comment.service";
import { useProfileStore } from "@/stores/user-profile.store";
import { StatusCodes } from "http-status-codes";
import { type Ref, ref, computed } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const newComment: Ref<AddComment> = ref(new AddComment());
const errorMessage: Ref<boolean> = ref(false);
const userStore = useProfileStore();

let loggedInUserId: Ref<number> = computed(() => {
  return userStore.userDetails.userId;
});

const emit = defineEmits<{ (e: "commentCreated"): void }>();
function addAComment(): void {
  newComment.value.userId = loggedInUserId.value;
  commentService
    .addCommentToBlab(+route.params.id, newComment.value)
    .then((response: ApiResponse) => {
      if (response.statusCode === StatusCodes.CREATED) {
        emit("commentCreated");
        newComment.value.content = "";
      } else {
        errorMessage.value = true;
      }
    });
}
</script>

<template>
  <p class="input-error-message" v-if="errorMessage">
    Comment couldn't be blabbed.
  </p>
  <div class="add-comment-container">
    <textarea
      maxlength="256"
      v-model="newComment.content"
      class="comment-text-area"
      data-test="comment-input-text-area"
      placeholder="Blab a comment!"
      @keydown.enter.exact.prevent="addAComment"
    />

    <button
      type="button"
      class="add-comment-button"
      data-test="add-comment-button"
      @click="addAComment"
    >
      <img src="@/assets/icons/send.svg" alt="send icon" />
    </button>
  </div>
</template>

<style lang="scss" scoped>
.add-comment-container {
  background-color: var(--primary-color);
  padding: 0.3rem;
  border-radius: 0 0 1rem 1rem;
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  bottom: 0%;
  position: sticky;
  box-shadow: 0 -4px 4px 0 rgba(0, 0, 0, 0.25);

  button {
    background-color: transparent;
  }
}

textarea {
  font-size: 1rem;
  flex: 1;
  margin: 0.5rem;
  background-color: var(--background-color);
  outline: none;
  border: none;
  padding: 0.5rem;
  border-radius: 1rem;
  color: #fff;
  resize: vertical;
  min-height: 3rem;
  max-height: 10rem;

  &::placeholder {
    color: #fff;
  }
}
.input-error-message {
  color: var(--error-color);
  font-weight: 800;
  width: 100%;
  text-align: center;
  font-size: 1.2rem;
}
</style>
