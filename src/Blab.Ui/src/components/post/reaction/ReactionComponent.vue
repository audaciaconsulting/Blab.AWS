<template>
  <section>
    <transition name="slide-left-fade">
      <ReactionBox
        :is-reaction-being-updated="isReactionBeingUpdated"
        :reactions="reactions"
        @change-current-reaction="changeReaction"
        v-if="isMouseHoveringOnBtn"
        @changeBoxState="boxToFalse"
        @hover-on-reaction-box="mouseEnterReactionBox"
        @hover-off-reaction-box="mouseLeaveReactionBox"
      />
    </transition>
    <input
      :class="changeHoveringClassState"
      @mouseleave="checkMousePos"
      data-test="react-button"
      type="button"
      value="React"
      @mouseover="boxToTrue"
    />
    <CurrentReactions :reactions="reactions" />
  </section>
</template>

<script setup lang="ts">
import { computed, ref, type ComputedRef, type PropType, type Ref } from "vue";

import ReactionBox from "@/components/post/reaction/reaction-box/ReactionBox.vue";
import CurrentReactions from "./CurrentReactions.vue";
import type { IReaction } from "@/models/reactions/reaction.interface";
import { Reaction } from "@/models/reactions/reaction.enum";
import { useProfileStore } from "@/stores/user-profile.store";
import { ReactToPost } from "@/models/reactions/react-to-post.model";
import { PostService } from "@/services/posts/post.service";
import type { ApiResponse } from "@/models/api/api-response.model";
import { StatusCodes } from "http-status-codes";
import { UserProfileService } from "@/services/user-profile.service";
import type { ReactionResponse } from "@/models/reactions/reaction-response.model";
const isMouseHoveringOnBtn: Ref<boolean> = ref(false);
const isReactionBeingUpdated: Ref<boolean> = ref(false);
const isHoveringOnReactionBox: Ref<boolean> = ref(false);
const props = defineProps({
  blabId: { required: true, type: Number },
  currentReaction: {
    default: null,
    type: Number as PropType<Reaction | null>,
  },
});

const emit = defineEmits<{
  (e: "addedReaction", blabId: number, reaction: Reaction): void;
  (e: "updatedReaction", blabId: number, reaction: Reaction): void;
  (e: "deletedReaction", blabId: number): void;
}>();

const userStore = useProfileStore();
const userProfileService: UserProfileService = new UserProfileService();
const model: Ref<ReactToPost> = ref(new ReactToPost());
const changeHoveringClassState: ComputedRef<"" | "isHovering"> = computed(() =>
  isMouseHoveringOnBtn.value ? "isHovering" : ""
);
const reactionId: typeof Reaction = Reaction;
const reactions: Ref<IReaction[]> = ref([
  {
    icon: "❤️",
    reactionName: "heart",
    reactionAmount: 0,
    isCurrentReaction: false,
    reaction: reactionId.heart,
  },
  {
    icon: "😊",
    reactionName: "smile",
    reactionAmount: 0,
    isCurrentReaction: false,
    reaction: reactionId.smile,
  },
  {
    icon: "🔥",
    reactionName: "fire",
    reactionAmount: 0,
    isCurrentReaction: false,
    reaction: reactionId.fire,
  },
  {
    icon: "🐧",
    reactionName: "penguin",
    reactionAmount: 0,
    isCurrentReaction: false,
    reaction: reactionId.penguin,
  },
  {
    icon: "🧙‍♂️",
    reactionName: "wizard",
    reactionAmount: 0,
    isCurrentReaction: false,
    reaction: reactionId.wizard,
  },
]);
addCurrentReactions();
const postService: PostService = new PostService();

async function addCurrentReactions(): Promise<void> {
  if (props.blabId) {
    const loggedInReaction: number | null = props.currentReaction;

    const res = await userProfileService.getReactionToBlab(props.blabId);
    const reactionsRes: ReactionResponse[] = res.data;

    reactions.value.map((reaction) => {
      return reactionsRes.forEach((reactionRes) => {
        if (reaction.reaction === loggedInReaction) {
          reaction.isCurrentReaction = true;
        }
        if (reactionRes.count > 0 && reactionRes.type === reaction.reaction) {
          reaction.reactionAmount = reactionRes.count;
        }
      });
    });
  }
}

function boxToFalse(): void {
  isMouseHoveringOnBtn.value = false;
}
function boxToTrue(): void {
  isMouseHoveringOnBtn.value = true;
}
function checkMousePos(): void {
  setTimeout(() => {
    if (
      !isHoveringOnReactionBox.value &&
      changeHoveringClassState.value === "isHovering"
    ) {
      boxToFalse();
    }
  }, 150);
}
function mouseLeaveReactionBox(): void {
  isHoveringOnReactionBox.value = false;
  checkMousePos();
}
function mouseEnterReactionBox(): void {
  isHoveringOnReactionBox.value = true;
}
function findingCurrentReaction(reactionEnum: Reaction): IReaction | undefined {
  // This function finds the currnet reaction from the passed in reaction name
  return reactions.value.find((reaction) => {
    return reaction.reaction === reactionEnum;
  });
}

function updatingReactionState(reaction: IReaction): IReaction {
  // resets all of the reactions to false apart from the currently active one and decrements the previous reaction value
  reactions.value.forEach((react) => {
    if (reaction.reactionName !== react.reactionName) {
      react.isCurrentReaction = false;

      if (props.currentReaction === react.reaction) {
        if (react.reactionAmount - 1 >= 0) {
          react.reactionAmount--;
        }
      }
    }
  });
  changeCurrentReactionValue(reaction);
  return reaction;
}
function changeCurrentReactionValue(reaction: IReaction): void {
  // This will increase or decrease the reaction value depending if it was the current value before hand
  reaction.isCurrentReaction = !reaction.isCurrentReaction;

  if (reaction.isCurrentReaction) {
    reaction.reactionAmount++;
  } else {
    if (reaction.reactionAmount - 1 >= 0) {
      reaction.reactionAmount--;
    }
  }
}
async function changeReaction(reactionEnum: Reaction): Promise<void> {
  isReactionBeingUpdated.value = true;
  // This finds the current reaction the user has reacted to and filters it accordingly
  const currentReactionIfReacted = findingCurrentReaction(reactionEnum);

  // if the reaction is not the same as before it will run this if
  if (userStore.userDetails.userId && currentReactionIfReacted) {
    const isReactionBeingUpdated: boolean =
      props.currentReaction !== currentReactionIfReacted.reaction;

    // sending blab ID , User ID, Reaction ID to the api
    model.value.reactionType = currentReactionIfReacted.reaction;
    model.value.postId = props.blabId;
    model.value.userId = userStore.userDetails.userId;

    let reactionRes: ApiResponse | null = null;
    if (props.currentReaction === null) {
      // If the user has no reaction on a post it will add anew reaction
      reactionRes = await postService.addReactionToPost(model.value);
    } else if (!isReactionBeingUpdated) {
      // This will run if the post is being deleted
      reactionRes = await postService.deletingReactionFromPost(
        userStore.userDetails.userId,
        props.blabId
      );
    } else if (isReactionBeingUpdated) {
      // updating reaction to another reaction
      reactionRes = await postService.updatingReactionFromPost(model.value);
    }

    if (reactionRes && reactionRes.statusCode === StatusCodes.CREATED) {
      updatingReactionState(currentReactionIfReacted);
      emit("addedReaction", props.blabId, currentReactionIfReacted.reaction);
    } else if (reactionRes && reactionRes.statusCode === StatusCodes.OK) {
      updatingReactionState(currentReactionIfReacted);
      emit("updatedReaction", props.blabId, currentReactionIfReacted.reaction);
    } else if (
      reactionRes &&
      reactionRes.statusCode === StatusCodes.NO_CONTENT
    ) {
      updatingReactionState(currentReactionIfReacted);
      emit("deletedReaction", props.blabId);
    }
  }
  isReactionBeingUpdated.value = false;
}
</script>

<style scoped lang="scss">
section {
  color: var(--background-color);
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
  gap: 20px;
  margin-top: 10px;

  input {
    color: var(--background-color);
    font-size: 1.2rem;
    background-color: var(--secondary-color);
    padding: 5px 10px;
    border-radius: 0.5rem;
    font-weight: 600;
    outline: none;
    border: none;
    cursor: pointer;
  }
}

.slide-left-fade-enter-active,
.slide-left-fade-leave-active {
  transition: all 0.4s ease;
}

.slide-left-fade-enter-from,
.slide-left-fade-leave-to {
  opacity: 0;
  transform: translateY(33%) scale(0);
  transform-origin: center left;
}
</style>
