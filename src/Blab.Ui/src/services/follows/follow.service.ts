import type { ApiResponse } from "@/models/api/api-response.model";
import baseService from "@/services/base.service";
import type { Follow } from "@/models/follows/follow.model";

export class FollowService {
  public followUser(model: Follow): Promise<ApiResponse> {
    return baseService.post("/api/users/follow", model);
  }

  public unfollowUser(followeeId: number): Promise<ApiResponse> {
    return baseService.delete("/api/users/follow/" + followeeId);
  }
}
