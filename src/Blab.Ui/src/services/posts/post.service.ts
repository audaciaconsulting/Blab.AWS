import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import type { ApiResponse } from "@/models/api/api-response.model";
import type { IPagingResponse } from "@/models/paging/paging-request-response.interface";
import type { IPost } from "@/models/posts/post.interface";
import type { ReactToPost } from "@/models/reactions/react-to-post.model";
import type { GetBlabs } from "@/models/user-blab-feed/get-blabs.model";
import baseService from "@/services/base.service";

export class PostService {
  public getPost(postId: number): Promise<ApiResponse> {
    return baseService.get("/api/blab/" + postId);
  }

  public deletePost(postId: number): Promise<ApiResponse> {
    // calls the post by Id then deletes
    return baseService.delete("/api/blab/" + postId);
  }

  public getUsersOwnBlabs(
    userId: number,
    model: GetBlabs
  ): Promise<ApiResponseWithType<IPagingResponse<IPost>>> {
    return baseService.get(
      `/api/users/${userId}/posts?pageNumber=${model.pageNumber}&pageSize=${model.pageSize}`
    );
  }

  public addReactionToPost(model: ReactToPost): Promise<ApiResponse> {
    return baseService.post("/api/blab/react", model);
  }

  public deletingReactionFromPost(
    userId: number,
    postId: number
  ): Promise<ApiResponse> {
    return baseService.delete(
      `/api/blab/react/?postId=${postId}&userId=${userId}`
    );
  }
  public updatingReactionFromPost(model: ReactToPost): Promise<ApiResponse> {
    return baseService.put("/api/blab/react", model);
  }
}

const postService = new PostService();
export default postService;
