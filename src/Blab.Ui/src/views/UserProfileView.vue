<template>
  <SpinnerLoader v-if="loading" :lightTheme="true" />
  <div v-else class="user-profile-container">
    <BannerComponent
      v-if="userStore.userDetails.handle"
      @changeUserData="updateUserData"
      :userDetails="profileUserDetails"
      :isOnOwnProfile="isOnMyUserPage"
      @followed="retrieveUserProfile"
      @unfollowed="retrieveUserProfile"
    />
    <PostsComponent
      v-if="usersFeedReactionModel.postFeed && userStore.userDetails.handle"
      :posts="usersFeedReactionModel.postFeed"
      :no-blabs-message="message?.toString()"
      :is-on-my-user-page="isOnMyUserPage"
      @addedReaction="updateUserPostReaction"
      @updatedReaction="updateUserPostReaction"
      @deletedReaction="deleteUserPostReaction"
      @change-loading-state-to-false="isLoadingData = false"
      @refresh-blabs="refreshBlabs"
    />
    <SpinnerLoader v-if="isLoadingData" :lightTheme="true" />
  </div>
</template>

<script setup lang="ts">
import { UserProfileService } from "@/services/user-profile.service";
import BannerComponent from "@/components/user-profile/banner/UserProfileBanner.vue";
import PostsComponent from "@/components/post/PostsComponent.vue";
import SpinnerLoader from "@/components/spinners/SpinnerLoader.vue";
import { sortByDateArrayProp } from "@/helpers/date.helper";
import { useProfileStore } from "@/stores/user-profile.store";
import {
  ref,
  type Ref,
  computed,
  type ComputedRef,
  onUpdated,
  onMounted,
  onUnmounted,
} from "vue";
import { useRoute, type RouteLocationNormalizedLoaded } from "vue-router";
import type { IUserProfileDetails } from "@/models/user-profile/user-profile-details.interface";
import type { IPost } from "@/models/posts/post.interface";
import type { Reaction } from "@/models/reactions/reaction.enum";
import { PostReactionFeed } from "@/composables/post-reaction-feed.composable";
import router from "@/router";
import { GetBlabs } from "@/models/user-blab-feed/get-blabs.model";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import type { IPagingResponse } from "@/models/paging/paging-request-response.interface";
import { PostService } from "@/services/posts/post.service";
import { StatusCodes } from "http-status-codes";
import { getScrollPercentage } from "@/helpers/get-scroll-percentage.helper";
import { Photo } from "@/models/photos/photo.model";

const isLoadingData: Ref<boolean> = ref(false);
const model: Ref<GetBlabs> = ref(new GetBlabs(0, 10));
const message: Ref<String | null> = ref(null);
const loading: Ref<boolean> = ref(true);
const userProfileService = new UserProfileService();
const userStore = useProfileStore();
const postService = new PostService();
const route: RouteLocationNormalizedLoaded = useRoute();
const { handle } = route.params;
const profileHandle: string = handle as string;
const isOnMyUserPage: ComputedRef<boolean> = computed(() => {
  return userStore.userDetails.handle === handle;
});
const usersFeedReactionModel = ref(new PostReactionFeed());
const profileUserDetails: Ref<IUserProfileDetails> = ref({
  userId: 0,
  handle: "loading handle...",
  displayName: "loading display name",
  blabs: [],
  isFollowing: false,
  profilePhoto: new Photo(),
});
const isFeedFullyLoaded = computed(() => {
  return (
    usersFeedReactionModel.value.postFeed.length === page.value?.totalRecords
  );
});
const scrollContainer: Ref<HTMLElement | null> = ref(
  document.querySelector("body")
);
let page: Ref<IPagingResponse<IPost> | null> = ref(null);

if (handle) {
  document.title = `Blab - @${handle}`;
}

onMounted(async () => {
  scrollContainer.value?.addEventListener("scroll", checkScrollPercentage);
  await retrieveUserProfile();
  await loadBlabFeed();
  loading.value = false;
});

onUnmounted(() => {
  scrollContainer.value?.removeEventListener("scroll", checkScrollPercentage);
});

onUpdated(() => {
  if (useRoute().params.handle !== handle) {
    router.go(0);
  }
});

async function retrieveUserProfile(): Promise<void> {
  if (isOnMyUserPage.value === false) {
    const userProfileDetails = await userProfileService.getUserProfileByHandle(
      profileHandle
    );
    if (userProfileDetails) {
      profileUserDetails.value = userProfileDetails.data;
    }
  } else {
    profileUserDetails.value = userStore.userDetails;
  }
}

function loadBlabFeed(): void {
  if (userStore.userDetails) {
    isLoadingData.value = true;
    postService
      .getUsersOwnBlabs(profileUserDetails.value.userId, model.value)
      .then((resData: ApiResponseWithType<IPagingResponse<IPost>>) => {
        if (resData.statusCode === StatusCodes.OK) {
          // if the response is OK blabs will be added to the feed
          const feed: IPost[] = resData.data.output.data;
          page.value = resData.data.output;
          //increments the pageNumber by one
          if (feed.length > 0) {
            model.value.pageNumber++;
          }

          usersFeedReactionModel.value.postFeed.push(...feed);
          usersFeedReactionModel.value.postFeed =
            usersFeedReactionModel.value.postFeed = sortByDateArrayProp(
              usersFeedReactionModel.value.postFeed,
              "dateCreated"
            );

          //if the user feed is empty
          if (!usersFeedReactionModel.value.postFeed.length) {
            message.value = "The feed is empty!";
          }
          // If all of the blabs have been loaded the user will be given a message
          else if (
            usersFeedReactionModel.value.postFeed.length ===
            resData.data.output.totalRecords
          ) {
            message.value = "You have loaded all of the feed!";
          }

          isLoadingData.value = false;
        }
      });
  }
}

function checkScrollPercentage(): void {
  // checks if data is not currently loading and the scroll Container is not null
  if (!isFeedFullyLoaded.value) {
    //calculates how much a user has scrolled by a percentage
    const getScrollPercent: number = getScrollPercentage(
      scrollContainer,
      message.value ? true : false
    );

    // if the scroll is over 90% it will get more blabs only if it is not already getting new data
    if (getScrollPercent > 90 && !isLoadingData.value) {
      loadBlabFeed();
    }
  }
}

function updateUserPostReaction(postId: number, newReaction: Reaction): void {
  usersFeedReactionModel.value.useUpdatePostReaction(postId, newReaction);
}

function deleteUserPostReaction(postId: number): void {
  usersFeedReactionModel.value.useDeletePostReaction(postId);
}

function updateUserData(): void {
  profileUserDetails.value = userStore.userDetails;
  profileUserDetails.value.blabs.forEach((blab) => {
    if (profileUserDetails.value.profilePhoto?.blobName) {
      blab.profilePhotoBlob = profileUserDetails.value.profilePhoto?.blobName;
    }
  });
  usersFeedReactionModel.value.postFeed = profileUserDetails.value.blabs;
}

function refreshBlabs(postId: number): void {
  usersFeedReactionModel.value.postFeed =
    usersFeedReactionModel.value.postFeed.filter((post) => {
      return post.id !== postId;
    });
}
</script>

<style scoped lang="scss">
.user-profile-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
  padding: 0px 30px;
}
</style>
