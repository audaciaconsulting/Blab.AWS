import type { Reaction } from "@/models/reactions/reaction.enum";
export interface ReactionResponse {
  type: Reaction;
  count: number;
}
