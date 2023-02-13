import type { Reaction } from "./reaction.enum";
export interface IReaction {
  icon: string;
  reactionAmount: number;
  reactionName: string;
  isCurrentReaction: boolean;
  reaction: Reaction;
}
