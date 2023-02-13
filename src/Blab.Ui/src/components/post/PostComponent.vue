<template>
  <article class="post-container">
    <div class="right-align-button" v-if="isOnMyUserPage">
      <DeletePostBtn @openDeleteModal="openDeletePostModal" />
    </div>
    <RouterLink
      v-if="props.post.id"
      :to="{ name: 'SinglePost', params: { id: props.post.id } }"
    >
      <PostContent :post="props.post" />
    </RouterLink>

    <ReactionComponent
      :blab-id="props.post.id"
      :current-reaction="props.post.reaction"
      @added-reaction="
        (postId, newReaction) => emit('addedReaction', postId, newReaction)
      "
      @updated-reaction="
        (postId, newReaction) => emit('updatedReaction', postId, newReaction)
      "
      @deleted-reaction="(postId) => emit('deletedReaction', postId)"
    />
  </article>
</template>

<script setup lang="ts">
import type { IPost } from "@/models/posts/post.interface";
import type { Reaction } from "@/models/reactions/reaction.enum";
import type { PropType } from "vue";
import { RouterLink } from "vue-router";
import PostContent from "@/components/post/PostContent.vue";
import ReactionComponent from "@/components/post/reaction/ReactionComponent.vue";
import DeletePostBtn from "@/components/user-profile/posts/Delete/DeletePostBtn.vue";

const props = defineProps({
  post: { required: true, type: Object as PropType<IPost> },
  isOnMyUserPage: { required: true, type: Boolean },
});

function openDeletePostModal() {
  emit("openDeleteModal", props.post.id);
}

const emit = defineEmits<{
  (e: "addedReaction", blabId: number, reaction: Reaction): void;
  (e: "updatedReaction", blabId: number, reaction: Reaction): void;
  (e: "deletedReaction", blabId: number): void;
  (e: "openDeleteModal", postId: number): void;
}>();
</script>

<style lang="scss">
.post-container {
  text-decoration: none;
  color: #fff;
  width: 45%;
  background-color: var(--primary-color);
  padding: 1.5rem;
  border-radius: var(--border-radius-amount);
  display: flex;
  flex-direction: column;
  a {
    text-decoration: none;
    color: #fff;
  }
  h2 {
    width: 100%;
    line-height: 1.66rem;
    font-size: 1.66rem;
    font-weight: 700;
  }

  p {
    word-wrap: break-word;
    font-size: 1.15rem;
  }

  .right-align-button {
    align-self: flex-end;
  }
}
</style>
