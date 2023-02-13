import type { ApiResponse } from "@/models/api/api-response.model";
import type { AddComment } from "@/models/comments/add-comment.model";
import type { GetComments } from "@/models/comments/get-comments.model";
import baseService from "@/services/base.service";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import type { IPagingResponse } from "@/models/paging/paging-request-response.interface";
import type { IComment } from "@/models/comments/comment.interface";

export class CommentService {
  // Calls the search comment endpoint with a specified page and page size
  public getCommentsFeed(
    postId: number,
    model: GetComments
  ): Promise<ApiResponseWithType<IPagingResponse<IComment>>> {
    return baseService.get(
      `/api/blab/${postId}/comment/search/?pageNumber=${model.pageNumber}&pageSize=${model.pageSize}`
    );
  }
  // Calls the add comment endpoint
  public addCommentToBlab(
    postId: number,
    model: AddComment
  ): Promise<ApiResponse> {
    return baseService.post(`/api/blab/${postId}/comment`, model);
  }
}

const commentService = new CommentService();
export default commentService;
