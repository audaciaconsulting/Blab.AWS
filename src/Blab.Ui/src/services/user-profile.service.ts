import type { ReactionResponse } from "./../models/reactions/reaction-response.model";
import type { UserSearch } from "@/models/search/search-user.model";
import type { ApiResponse } from "@/models/api/api-response.model";
import type { AddABlab } from "@/models/user-profile/add-a-blab-user-profile.model";
import type { UpdateUserProfile } from "@/models/user-profile/update-user-profile.model";
import baseService from "@/services/base.service";
import type { GetBlabs } from "@/models/user-blab-feed/get-blabs.model";
import type { ISearchUser } from "@/models/search/search-user.interface";
import type { IPagingResponse } from "@/models/paging/paging-request-response.interface";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import type { IPost } from "@/models/posts/post.interface";

export class UserProfileService {
  //Calls the endpoint that returns the user profile details using their handle.
  public getUserProfileByHandle(handle: string): Promise<ApiResponse> {
    return baseService.get("/api/users/handle/" + handle);
  }
  public getUserProfile(userId: number): Promise<ApiResponse> {
    //the endpoint that gets the user profile details by their ID.
    return baseService.get("/api/users/id/" + userId);
  }
  public getProfileUserID(handle: string): Promise<ApiResponse> {
    //calls the endpoint that uses the handle/username of a user to return their userId, if they exist in the database.
    return baseService.get("/get-id/" + handle);
  }
  public updateUserProfile(
    updatedProfileValues: UpdateUserProfile
  ): Promise<ApiResponseWithType<string>> {
    return baseService.put("/api/users/profile", updatedProfileValues);
  }
  public addABlabToProfile(model: AddABlab): Promise<ApiResponse> {
    return baseService.post("/api/blab", model);
  }
  public getUsersBlabFeed(
    model: GetBlabs
  ): Promise<ApiResponseWithType<IPagingResponse<IPost>>> {
    return baseService.get(
      `/api/feed/?pageNumber=${model.pageNumber}&pageSize=${model.pageSize}`
    );
  }
  public getReactionToBlab(blabId: number): Promise<ApiResponse> {
    return baseService.get(`/api/blab/${blabId}/reactions`);
  }
  public searchForUsers(
    model: UserSearch
  ): Promise<ApiResponseWithType<IPagingResponse<ISearchUser>>> {
    return baseService.get(
      `/api/users/search/?pageNumber=${model.pageNumber}&pageSize=${model.pageSize}&searchTerm=${model.searchTerm}`
    );
  }
}
