<template>
  <div class="add-blab-container">
    <h2>Add a Blab</h2>

    <div
      @keypress="isOverMax(256)"
      class="blab-textarea"
      contenteditable="true"
      @click="
        successMessage = '';
        errorMessage = '';
      "
      @keydown.enter.exact.prevent="save"
      ref="postContentElement"
      data-test="blab-input-text-area"
    ></div>
    <section>
      <button data-test="create-blab-button" @click="save()">Save</button>
      <div class="response-container" v-show="successMessage || errorMessage">
        <p class="success" data-test="save-blab-success-message">
          {{ successMessage }}
        </p>
        <p class="error" data-test="save-blab-error-message">
          {{ errorMessage }}
        </p>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { Ref } from "vue";
import { useProfileStore } from "@/stores/user-profile.store";
import {
  maximumBlabLength,
  minimumBlabLength,
} from "@/helpers/validate-lengths.helper";
import { AddABlab } from "@/models/user-profile/add-a-blab-user-profile.model";
import type { ApiResponse } from "@/models/api/api-response.model";
import { UserProfileService } from "@/services/user-profile.service";
import { StatusCodes } from "http-status-codes";
import type { IPost } from "@/models/posts/post.interface";
const model: Ref<AddABlab> = ref(new AddABlab());
const postContentElement: Ref<HTMLElement | null> = ref(null);
const successMessage: Ref<string> = ref("");
const errorMessage: Ref<string> = ref("");
const userStore = useProfileStore();

const emits = defineEmits<{ (e: "postNewBlab", post: IPost): void }>();

function save(): void {
  if (
    postContentElement.value &&
    postContentElement.value.textContent !== null &&
    userStore.userDetails.userId
  ) {
    model.value.userId = parseInt(userStore.userDetails.userId.toString());
    model.value.content = postContentElement.value.textContent.trim();
    errorMessage.value = "";
    successMessage.value = "";
    const isOverMaximum: boolean = maximumBlabLength(model.value.content, 256);
    const isUnderMinimum: boolean = minimumBlabLength(model.value.content, 1);
    if (isOverMaximum) {
      errorMessage.value = "Your blab exceeded 256 characters, please shorten";
    } else if (isUnderMinimum) {
      errorMessage.value = "You must enter at least 1 character"; // error msg
    } else if (
      !isUnderMinimum &&
      !isOverMaximum &&
      userStore.userDetails.userId
    ) {
      const postBlab: Promise<ApiResponse> =
        new UserProfileService().addABlabToProfile(model.value);
      postBlab.then((res) => {
        if (postContentElement.value) {
          if (res.statusCode === StatusCodes.CREATED) {
            successMessage.value = "Blab was posted successfully!";
            emits("postNewBlab", res.data);
            postContentElement.value.textContent = "";
            userStore.getUserDetails();
          } else if (res.statusCode === StatusCodes.BAD_REQUEST) {
            errorMessage.value = "Your input is invalid please try again...";
          } else if (res.statusCode === StatusCodes.NOT_FOUND) {
            errorMessage.value =
              "This feature is down currently. Please try again...";
          } else if (res.statusCode === StatusCodes.UNAUTHORIZED) {
            errorMessage.value =
              "You are not currently logged in please login to add a blab.";
          }
        }
      });
    }
  }
}
function isOverMax(max: number): void {
  if (postContentElement.value) {
    if (postContentElement.value?.textContent?.length === max) {
      const currentContentBoxHeight = postContentElement.value.clientHeight;

      postContentElement.value.style.maxHeight = `${currentContentBoxHeight}px`;
    } else {
      postContentElement.value.style.maxHeight = "auto";
    }
  }
}
</script>

<style lang="scss" scoped>
.add-blab-container {
  margin: auto;
  display: flex;
  flex-direction: column;
  width: 45%;
  align-items: flex-start;

  gap: 20px;

  h2 {
    font-size: 1.5rem;
    font-weight: 800;
  }
}

button {
  outline: none;
  border: none;
  font-size: 1rem;
  font-weight: 800;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;

  height: 50px;
  align-self: flex-end;
  transition: all 0.2s ease;
  cursor: pointer;

  &:hover {
    background-color: var(--sidebar-color);
    transform: scale(1.2);
  }

  img {
    width: 1.3rem;
  }
}
.blab-textarea,
button {
  padding: 1rem;
  border-radius: 1rem;
  border: solid 2px var(--secondary-color);
  background-color: var(--background-color);
  color: var(--secondary-color);

  &:focus-visible {
    border: solid 2px var(--secondary-color);
    outline: var(--secondary-color);
  }
}

.blab-textarea {
  width: 100%;
  resize: none;
  overflow-y: auto;
  font-size: 1rem;
  padding: 0.5rem;
  flex: 1;
  max-height: auto;
}
.response-container {
  flex: 1;
}
.error,
.success {
  text-align: left;

  font-size: 1.2rem;
  font-weight: 800;
}
.error {
  color: rgb(249, 58, 58);
}

section {
  display: flex;
  width: 100%;
  gap: 20px;
  justify-content: space-between;
  align-items: center;
}

[contentEditable="true"]:empty:before {
  content: "What's blabbening my dudes?";
  opacity: 0.6;
}
</style>
