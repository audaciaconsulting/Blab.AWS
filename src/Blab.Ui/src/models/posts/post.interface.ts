import type { Reaction } from "@/models/reactions/reaction.enum";

export interface IPost {
  id: number;
  content: string;
  dateCreated: Date | string;
  userId: number;
  displayName: string;
  handle: string;
  reaction: Reaction | null;
  profilePhotoBlob: string | null;
}
