import type { AddPhoto } from "../photos/add-photo.model";

export class UpdateUserProfile {
  userId!: number;
  handle!: string;
  displayName!: string;
  bio?: string;
  profilePhoto?: AddPhoto | null;
  backgroundPhoto?: AddPhoto | null;
}
