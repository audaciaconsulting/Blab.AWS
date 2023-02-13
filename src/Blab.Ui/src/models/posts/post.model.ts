import type { Reaction } from "../reactions/reaction.enum";
import type { IPost } from "./post.interface";

export class Post implements IPost {
  id!: number;
  content!: string;
  dateCreated!: Date;
  userId!: number;
  displayName!: string;
  handle!: string;
  reaction: Reaction | null = null;
  profilePhotoBlob: string | null = null;
}
