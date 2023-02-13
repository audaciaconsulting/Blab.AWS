import type { IComment } from "./comment.interface";

export class CommentFeed {
  comments!: IComment[];
  pageSize!: number;
  pageNumber!: number;
}
