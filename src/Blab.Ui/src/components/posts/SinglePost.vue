<script setup lang="ts">
import type { IPost } from "@/models/posts/post.interface";
import formatStringDate from "@/helpers/date-formatter";
import DeletePostBtn from "@/components/user-profile/posts/Delete/DeletePostBtn.vue";
import { ref, type PropType, type Ref } from "vue";
import { useRouter } from "vue-router";
import { useProfileStore } from "@/stores/user-profile.store";
import ReactionComponent from "../post/reaction/ReactionComponent.vue";
import { profilePhotoSelector } from "@/helpers/profile-photo-selector";

const router = useRouter();
const props = defineProps({
  post: { required: true, type: Object as PropType<IPost> },
});
const userStore = useProfileStore();
const loggedInUserId = userStore.userDetails.userId;
const postCreatedByUser = props.post.userId;
const formattedDate = formatStringDate(props.post.dateCreated);
const handle = props.post.handle;
const emit = defineEmits<{
  (e: "openDeleteModal", postId: number): void;
}>();
const postReaction: Ref<number | null> = ref(props.post.reaction);

const imageSrc: Ref<string> = ref(
  profilePhotoSelector(props.post.profilePhotoBlob)
);

function deletePostState() {
  emit("openDeleteModal", props.post.id);
}
// If the user wants to add or update the reaction it will emit to this function and change the postReaction value to match the databases
function changeReaction(blabId: number, reactionEnum: number): void {
  postReaction.value = reactionEnum;
  updateUserProfilesBlabReaction(blabId, reactionEnum);
}
// removes it from the single post if the user has requested it to be deleted
function removeReaction(blabId: number): void {
  postReaction.value = null;
  updateUserProfilesBlabReaction(blabId, null);
}
function updateUserProfilesBlabReaction(
  blabId: number,
  reaction: number | null
): void {
  // updates current logged in users reaction if the id matches one in the store
  userStore.$patch((state) => {
    state.userDetails.blabs.find((blab) => {
      if (blab.id === blabId) {
        blab.reaction = reaction;
      }
    });
  });
}
</script>

<template>
  <article v-if="props.post" class="post-container">
    <div class="right-align-button" v-if="loggedInUserId == postCreatedByUser">
      <p><DeletePostBtn @openDeleteModal="deletePostState" /></p>
    </div>
    <div class="header-container">
      <img
        class="profile-picture"
        :src="imageSrc"
        rounded="circle"
        alt="profile-picture"
        @click="router.push({ name: 'UserProfile', params: { handle } })"
      />
      <div
        class="profile-info-container"
        @click="router.push({ name: 'UserProfile', params: { handle } })"
      >
        <h1>{{ post.displayName }}</h1>
        <h2>@{{ post.handle }}</h2>
      </div>
    </div>
    <p class="post-content">{{ post.content }}</p>
    <span>{{ formattedDate }}</span>

    <ReactionComponent
      :blab-id="props.post.id"
      :current-reaction="postReaction"
      @deletedReaction="removeReaction"
      @addedReaction="changeReaction"
      @updatedReaction="changeReaction"
    />
  </article>
</template>

<style scoped lang="scss">
.post-container {
  background-color: var(--primary-color);
  padding: 1.5rem;
  border-radius: var(--border-radius-amount);
  display: flex;
  flex-direction: column;
  width: 50%;
  h2 {
    width: 100%;
    line-height: 1.66rem;
    font-size: 1.66rem;
    font-weight: 700;
  }
}
.post-content {
  word-wrap: break-word;
  font-size: 1.15rem;
  padding-bottom: 10px;
}

.profile-picture {
  height: 60px;
  width: 60px;
  border-radius: 50%;
}
.profile-info-container {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  h2 {
    font-size: medium;
  }
}
span {
  margin-bottom: 1rem;
}
.header-container {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  gap: 20px;
  padding: 0px 20px 20px 0px;
}
.right-align-button {
  right: 0px;
}
</style>
