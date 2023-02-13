import type { IPost } from "@/models/posts/post.interface";
import type { Photo } from "@/models/photos/photo.model";

export interface IUserProfileDetails {
  displayName: string;
  handle: string;
  bio?: string;
  userId: number;
  blabs: IPost[];
  isFollowing?: boolean | null;
  profilePhoto?: Photo | null;
  backgroundPhoto?: Photo | null;
}
