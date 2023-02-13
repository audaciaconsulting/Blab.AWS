<template>
  <div
    data-test="reaction-box"
    class="reaction-box-container"
    @mouseleave="onHoverLeave()"
    @mouseenter="onHoverEnter"
  >
    <div
      :class="['react-icon', isReactionResLoading]"
      v-for="reaction in props.reactions"
      :key="reaction.reactionName"
      :data-test="`react-icon-${reaction.reactionName}`"
      @click="changeIsReactActive(reaction.reaction)"
    >
      <div class="currently-active" v-if="reaction.isCurrentReaction"></div>
      <span>{{ reaction.icon }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Reaction } from "@/models/reactions/reaction.enum";
import type { IReaction } from "@/models/reactions/reaction.interface";
import { computed, type ComputedRef, type PropType } from "vue";
const props = defineProps({
  reactions: { required: true, type: Array as PropType<IReaction[]> },
  isReactionBeingUpdated: { required: true, type: Boolean },
});
const isReactionResLoading: ComputedRef = computed(() =>
  props.isReactionBeingUpdated ? "disable" : ""
);
const emit = defineEmits<{
  (e: "changeCurrentReaction", type: Reaction): void;
  (e: "hoverOnReactionBox"): void;
  (e: "hoverOffReactionBox"): void;
}>();
function onHoverEnter(): void {
  emit("hoverOnReactionBox");
}
function onHoverLeave(): void {
  emit("hoverOffReactionBox");
}

function changeIsReactActive(reactionEnum: Reaction): void {
  emit("changeCurrentReaction", reactionEnum);
}
</script>

<style scoped lang="scss">
.reaction-box-container {
  position: absolute;
  top: -70px;
  left: 0;
  z-index: 10;
  background-color: var(--secondary-color);
  padding: 0.5rem;
  font-size: 2rem;
  border-radius: 2rem;
  display: flex;
  gap: 5px;

  .currently-active {
    position: absolute;
    bottom: 0;
    width: 6px;
    height: 6px;
    background-color: var(--background-color);
    border-radius: 50%;
  }
  span {
    transform: scale(1);
    &:hover {
      animation: shake 500ms infinite linear;
    }
  }
  .react-icon {
    display: flex;
    flex-direction: column;
    align-items: center;
    cursor: pointer;
  }
}

@keyframes shake {
  0% {
    transform: rotate(0deg) scale(1.5);
  }
  33% {
    transform: rotate(-10deg) scale(1.5);
  }
  66% {
    transform: rotate(10deg) scale(1.5);
  }
  100% {
    transform: rotate(0deg) scale(1.5);
  }
}
</style>
