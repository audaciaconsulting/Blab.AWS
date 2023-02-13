<script setup lang="ts">
import { type Ref, ref, computed, type ComputedRef, type PropType } from "vue";
import { FollowService } from "@/services//follows/follow.service";
import { useProfileStore } from "@/stores/user-profile.store";
import { Follow } from "@/models/follows/follow.model";
import type { IUserProfileDetails } from "@/models/user-profile/user-profile-details.interface";
import type { ApiResponse } from "@/models/api/api-response.model";
import { StatusCodes } from "http-status-codes";

const props = defineProps({
  userDetails: {
    required: true,
    type: Object as PropType<IUserProfileDetails>,
  },
});

const emit = defineEmits<{
  (e: "followed"): void;
  (e: "unfollowed"): void;
}>();

const followService = new FollowService();
const userStore = useProfileStore();

const isMouseHovering: Ref<boolean> = ref(false);
const isFollowingClassStyle: ComputedRef<"" | "following"> = computed(() =>
  props.userDetails.isFollowing ? "following" : ""
);

function followStateCheck(): void {
  if (!props.userDetails.isFollowing) {
    followUser();
  } else {
    unfollowUser();
  }
}

function followUser(): void {
  const request = new Follow();
  request.followerId = userStore.userDetails.userId;
  request.followeeId = props.userDetails.userId;
  followService.followUser(request).then((response: ApiResponse) => {
    if (response.statusCode === StatusCodes.CREATED) {
      emit("followed");
    }
  });
}

function unfollowUser(): void {
  const followeeId = props.userDetails.userId;
  followService.unfollowUser(followeeId).then((response: ApiResponse) => {
    if (response.statusCode === StatusCodes.NO_CONTENT) {
      emit("unfollowed");
    }
  });
}
</script>

<template>
  <div
    @click="followStateCheck"
    @mouseover.prevent="isMouseHovering = true"
    @mouseleave.prevent="isMouseHovering = false"
    :class="`follow-container ${isFollowingClassStyle}`"
  >
    <span v-if="!props.userDetails.isFollowing"> Follow </span>
    <span v-else-if="isMouseHovering" class="unfollow"> </span>
    <span v-else>Following</span>
  </div>
</template>

<style scoped lang="scss">
.follow-container {
  font-size: 1.5rem;

  border-radius: var(--border-radius-amount);
  border: var(--secondary-color) solid 3px;
  color: var(--background-color);
  background-color: var(--secondary-color);
  padding: 0.8rem 1rem;
  line-height: 1.5rem;
  cursor: pointer;
  min-width: 160px;
  text-align: center;

  .unfollow {
    &::before {
      font-weight: 800;
      content: "Unfollow";
    }
  }
}
span {
  font-weight: 800;
}

.following {
  background: transparent;
  color: var(--secondary-color);
}
</style>
