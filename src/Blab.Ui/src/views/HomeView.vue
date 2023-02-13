<template>
  <div class="home-container">
    <CreateBlab @postNewBlab="displayNewBlab" />
    <PostsComponent
      :posts="blabFeedReactionModel.postFeed"
      :no-blabs-message="message?.toString()"
      :is-loading="isLoadingData"
      :is-on-my-user-page="false"
      @addedReaction="updateBlabFeedReaction"
      @updatedReaction="updateBlabFeedReaction"
      @deletedReaction="deleteBlabFeedReaction"
      @change-loading-state-to-false="isLoadingData = false"
    />
    <SpinnerLoader v-if="isLoadingData" :lightTheme="true" />
  </div>
</template>

<script setup lang="ts">
import CreateBlab from "@/components/blabs/CreateBlab.vue";
import SpinnerLoader from "@/components/spinners/SpinnerLoader.vue";
import PostsComponent from "@/components/post/PostsComponent.vue";
import { useProfileStore } from "@/stores/user-profile.store";
import { sortByDateArrayProp } from "@/helpers/date.helper";
import { UserProfileService } from "@/services/user-profile.service";
import { type Ref, ref, onMounted, onUnmounted, computed } from "vue";
import type { IPost } from "@/models/posts/post.interface";
import { GetBlabs } from "@/models/user-blab-feed/get-blabs.model";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import { StatusCodes } from "http-status-codes";
import type { IPagingResponse } from "@/models/paging/paging-request-response.interface";
import { getScrollPercentage } from "@/helpers/get-scroll-percentage.helper";
import { PostReactionFeed } from "@/composables/post-reaction-feed.composable";
import type { Reaction } from "@/models/reactions/reaction.enum";
import { convertStringToDateArray } from "@/helpers/convert-string-to-date-array.helper";

const userStore = useProfileStore();
const isLoadingData: Ref<boolean> = ref(false);
const isErrorLoading: Ref<boolean> = ref(false);
const model: Ref<GetBlabs> = ref(new GetBlabs(0, 10));
const message: Ref<String | undefined> = ref("");
const scrollContainer: Ref<HTMLElement | null> = ref(
  document.querySelector("body")
);

const blabFeedReactionModel = ref(new PostReactionFeed());

let page: Ref<IPagingResponse<IPost> | null> = ref(null);

function updateBlabFeedReaction(postId: number, newReaction: Reaction): void {
  blabFeedReactionModel.value.useUpdatePostReaction(postId, newReaction);
}
function deleteBlabFeedReaction(postId: number) {
  blabFeedReactionModel.value.useDeletePostReaction(postId);
}
const isFeedFullyLoaded = computed(() => {
  return (
    blabFeedReactionModel.value.postFeed.length === page.value?.totalRecords
  );
});

onMounted(() => {
  blabFeedReactionModel.value.postFeed = [];
  loadBlabFeed();
  scrollContainer.value?.addEventListener("scroll", checkScrollPercentage);
});

onUnmounted(() => {
  scrollContainer.value?.removeEventListener("scroll", checkScrollPercentage);
});

function displayNewBlab(newBlab: IPost): void {
  //This add the emitted new blab to the top of the feed.
  blabFeedReactionModel.value.postFeed.unshift(newBlab);
}
function loadBlabFeed(): void {
  // if the user is logged in
  if (userStore.currentUser) {
    if (!isErrorLoading.value) {
      const getBlabFeed = new UserProfileService();

      if (!isLoadingData.value) {
        isLoadingData.value = true;
        getBlabFeed
          .getUsersBlabFeed(model.value)
          .then((resData: ApiResponseWithType<IPagingResponse<IPost>>) => {
            if (resData.statusCode === StatusCodes.OK) {
              isErrorLoading.value = false;
              // if the response is OK blabs will be added to the feed
              const feed: IPost[] = resData.data.output.data;
              page.value = resData.data.output;

              //increments the pageNumber by one
              if (feed.length > 0) {
                model.value.pageNumber++;
              }

              blabFeedReactionModel.value.postFeed.push(...feed);
              // This variable will makes the array unique
              const getUniqArrBy: any = (
                props: string[] = [],
                arrInp: IPost[] = [],
                objTmp: any = {},
                arrTmp: any[] = []
              ) => {
                if (arrInp.length > 0) {
                  const lastItem: any = arrInp[arrInp.length - 1];
                  const propStr: string = props.reduce(
                    (res, item) => `${res}${lastItem[item]}`,
                    ""
                  );
                  if (!objTmp[propStr]) {
                    objTmp[propStr] = true;
                    arrTmp = [...arrTmp, lastItem];
                  }
                  arrInp.pop();
                  return getUniqArrBy(props, arrInp, objTmp, arrTmp);
                }
                return arrTmp;
              };
              // Removes blabs with the same ID
              const blabFeedUnique = getUniqArrBy(
                ["id"],
                blabFeedReactionModel.value.postFeed
              );
              // sets the blab feed to the unique version
              blabFeedReactionModel.value.postFeed = sortByDateArrayProp(
                convertStringToDateArray(blabFeedUnique, "dateCreated"),

                "dateCreated"
              );

              //if the user feed is empty
              if (!blabFeedReactionModel.value.postFeed.length) {
                message.value = "Your feed is empty!";
              }
              // If all of the blabs have been loaded the user will be given a message
              else if (
                blabFeedReactionModel.value.postFeed.length ===
                resData.data.output.totalRecords
              ) {
                message.value = "You have loaded all of your feed!";
              }
            } else if (resData.statusCode === StatusCodes.NOT_FOUND) {
              message.value =
                "Data has not been found please reload the page and try again...";
              isErrorLoading.value = true;
            } else if (resData.statusCode === StatusCodes.BAD_REQUEST) {
              message.value =
                "Bad request has occured reload the page and try again...";
              isErrorLoading.value = false;
            }
            isLoadingData.value = false;
          });
      }
    }
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
</script>
<style scoped lang="scss">
.profile-btn {
  margin-top: 3rem;
}

.home-container {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  gap: 20px;
}
</style>
