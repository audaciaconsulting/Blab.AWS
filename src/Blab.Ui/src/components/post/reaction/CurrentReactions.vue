<template>
  <div class="current-reaction-container">
    <div
      class="reaction-amount"
      data-test="reaction-amount-counter"
      v-for="reaction in props.reactions"
      :key="reaction.reactionName"
    >
      <div v-if="reaction.reactionAmount > 0" class="pill-container">
        {{ reaction.icon }} {{ reaction.reactionAmount.toString() }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { IReaction } from "@/models/reactions/reaction.interface";
import type { PropType } from "vue";

const props = defineProps({
  reactions: {
    required: true,
    type: Array as PropType<IReaction[]>,
  },
});
</script>

<style scoped lang="scss">
.current-reaction-container {
  display: flex;
  align-items: center;
  justify-content: flex-start;
  color: white;
  flex-wrap: wrap;

  .pill-container {
    background-color: var(--sidebar-color);
    margin: 5px;
    padding: 0.3rem 0.7rem;
    border-radius: 1rem;
    font-size: 1rem;
  }
}
</style>
