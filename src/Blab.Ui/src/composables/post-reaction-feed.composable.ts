import type { IPost } from "@/models/posts/post.interface";
import type { Reaction } from "@/models/reactions/reaction.enum";
import { useProfileStore } from "@/stores/user-profile.store";

export class PostReactionFeed {
  postFeed: IPost[] = [];
  userStore = useProfileStore();

  useUpdatePostReaction(postId: number, newReaction: Reaction): void {
    const reactedPost: IPost | undefined = this.postFeed.find(
      (post: IPost) => post.id == postId
    );
    if (reactedPost) {
      reactedPost.reaction = newReaction;
      this.useChangeStore(reactedPost);
    }
  }

  useDeletePostReaction(postId: number): void {
    const reactedPost = this.postFeed.find((post: IPost) => post.id == postId);
    if (reactedPost) {
      reactedPost.reaction = null;
      this.useChangeStore(reactedPost);
    }
  }

  useChangeStore(reactedPost: IPost) {
    if (reactedPost.handle === this.userStore.userDetails.handle) {
      this.userStore.$patch((state) => {
        state.userDetails.blabs.find((blab) => {
          if (reactedPost.id === blab.id) {
            blab.reaction = reactedPost.reaction;
          }
        });
      });
    }
  }
}
