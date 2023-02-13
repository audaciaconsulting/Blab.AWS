<template>
  <img class="background-picture" :src="backgroundImageSrc" />
  <div class="banner-container">
    <img
      class="profile-picture"
      :src="profileImageSrc"
      rounded="circle"
      alt="profile-picture"
    />
    <UserProfileDetailsVue
      v-if="props.userDetails.handle"
      :handle="props.userDetails.handle"
      :displayName="props.userDetails.displayName"
      :bio="props.userDetails.bio ? props.userDetails.bio : ''"
    />
    <div>
      <FollowUser
        v-if="!isOnMyUserProfile"
        :userDetails="userDetails"
        @followed="emit('followed')"
        @unfollowed="emit('unfollowed')"
      />
      <br />
      <MessageButton v-if="!isOnMyUserProfile" :userDetails="userDetails" />
      <EditProfileBtn
        @toggleEditState="changeEditState"
        v-if="isOnMyUserProfile"
      />
      <EditProfile
        @change-user-data="changeUserData"
        @toggleEditState="changeEditState"
        :profileEditState="editState"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import UserProfileDetailsVue from "./UserProfileDetails.vue";
import MessageButton from "@/components/user-profile/message-button/MessageButton.vue";
import type { IUserProfileDetails } from "@/models/user-profile/user-profile-details.interface";
import EditProfileBtn from "../edit-user-profile/EditProfileBtn.vue";
import EditProfile from "../edit-user-profile/EditProfile.vue";
import { computed, ref, type PropType, type Ref } from "vue";
import { useRoute } from "vue-router";
import { useProfileStore } from "@/stores/user-profile.store";
import FollowUser from "./FollowUser.vue";
import { profilePhotoSelector } from "@/helpers/profile-photo-selector";
import { backgroundPhotoSelector } from "@/helpers/background-photo-selector.helper";

const props = defineProps({
  userDetails: {
    required: true,
    type: Object as PropType<IUserProfileDetails>,
  },
  isOnOwnProfile: {
    required: true,
    type: Boolean,
  },
});
const emit = defineEmits<{
  (e: "changeUserData"): void;
  (e: "followed"): void;
  (e: "unfollowed"): void;
}>();

const route = useRoute();
const userStore = useProfileStore();

const editState: Ref<boolean> = ref(false);
function changeEditState(): void {
  editState.value = !editState.value;
}

const isOnMyUserProfile = computed(() => {
  return userStore.userDetails.handle === route.params.handle;
});
function changeUserData(): void {
  emit("changeUserData");
}

const profileImageSrc = computed(() => {
  return profilePhotoSelector(props.userDetails.profilePhoto?.blobName);
});

const backgroundImageSrc = computed(() => {
  return backgroundPhotoSelector(props.userDetails.backgroundPhoto?.blobName);
});
</script>

<style scoped lang="scss">
.banner-container {
  display: flex;
  height: auto;
  padding: 18px 32px;
  width: 100%;
  border-bottom: var(--border-bottom) solid 2px;
  .profile-picture {
    height: 100px;
    width: 100px;
    border-radius: 50%;
    align-self: flex-start;
    margin-right: 20px;
  }
}
.background-picture {
  height: 30vh;
  width: 100%;

  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
}
</style>
