<script setup lang="ts">
import SinglePost from "@/components/posts/SinglePost.vue";
import DeletePostModal from "@/components/user-profile/posts/Delete/DeletePostModal.vue";
import postService from "@/services/posts/post.service";
import { ref, type Ref } from "vue";
import { useRoute } from "vue-router";
import type { ApiResponse } from "@/models/api/api-response.model";
import type { IPost } from "@/models/posts/post.interface";
import { StatusCodes } from "http-status-codes";
import CommentsComponent from "@/components/comment/CommentsComponent.vue";
import type { IPagingResponse } from "@/models/paging/paging-request-response.interface";
import type { IComment } from "@/models/comments/comment.interface";
import commentService from "@/services/comment.service";
import { GetComments } from "@/models/comments/get-comments.model";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
const route = useRoute();
const isLoadingData: Ref<boolean> = ref(false);
const message: Ref<String | undefined> = ref("");
const loadedComments: Ref<IComment[]> = ref([]);

// loads single blab post.
const singlePost: Ref<IPost | null> = ref(null);
postService
  .getPost(+route.params.id)
  .then((response: ApiResponse) => {
    if (response.statusCode == StatusCodes.OK) {
      singlePost.value = response.data;
    }
  })
  .catch((error) => console.log(error));

// loads feed of comments
const getCommentModel: Ref<GetComments> = ref(new GetComments());
const commentFeed: Ref<IPagingResponse<IComment> | null> = ref(null);
function loadComments(reload?: boolean): void {
  isLoadingData.value = true;
  if (reload) {
    getCommentModel.value.pageNumber = 0;
    loadedComments.value = [];
  }
  commentService
    .getCommentsFeed(+route.params.id, getCommentModel.value)
    .then((response: ApiResponseWithType<IPagingResponse<IComment>>) => {
      if (response.statusCode == StatusCodes.OK) {
        commentFeed.value = response.data.output;
        loadedComments.value = loadedComments.value.concat(
          commentFeed.value.data
        );
        getCommentModel.value.pageNumber++;
      }
      if (!loadedComments.value.length) {
        message.value = "No comments yet.";
      } else if (
        loadedComments.value.length === commentFeed.value?.totalRecords
      ) {
        message.value = "All comments have been loaded for this blab";
      }
      isLoadingData.value = false;
    });
}

loadComments();

const showModal: Ref<boolean> = ref(false);
const postIdToDelete: Ref<number | null> = ref(null);
function openDeletePostModal(postId: number): void {
  postIdToDelete.value = postId;
  showModal.value = true;
}
function closeDeletePostModal(): void {
  postIdToDelete.value = null;
  showModal.value = false;
}
</script>

<template>
  <div class="post-comment-container">
    <SinglePost
      @openDeleteModal="openDeletePostModal"
      v-if="singlePost"
      :post="singlePost"
    />
    <DeletePostModal
      v-if="postIdToDelete"
      :profileDeleteState="showModal"
      :post-id="postIdToDelete"
      @closeModal="closeDeletePostModal"
    />
    <CommentsComponent
      v-if="commentFeed"
      :commentsMessage="message?.toString()"
      :comments="loadedComments"
      :isLoadingData="isLoadingData"
      @loadComments="loadComments"
      @commentCreated="loadComments(true)"
    />
  </div>
</template>

<style>
.post-comment-container {
  height: calc(100vh - 20vh);
  overflow-y: hidden;
  display: flex;
  width: 100%;
  gap: 20px;
}
</style>
