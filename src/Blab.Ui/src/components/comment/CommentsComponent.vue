<script setup lang="ts">
import type { IComment } from "@/models/comments/comment.interface";
import { onMounted, onUnmounted, ref, type PropType, type Ref } from "vue";
import CommentContent from "@/components/comment/CommentContent.vue";
import CommentInput from "@/components/comment/CommentInput.vue";
import SpinnerLoader from "@/components/spinners/SpinnerLoader.vue";
import { getScrollPercentage } from "@/helpers/get-scroll-percentage.helper";
const props = defineProps({
  comments: {
    required: true,
    type: Object as PropType<IComment[]>,
  },
  commentsMessage: {
    required: false,
    type: String,
  },
  isLoadingData: {
    required: true,
    type: Boolean,
  },
});

const scrollContainer: Ref<HTMLElement | null> = ref(null);

onMounted(() => {
  scrollContainer.value = document.querySelector("#comment-feed");
  scrollContainer.value?.addEventListener("scroll", checkScrollPercentage);
});

onUnmounted(() => {
  scrollContainer.value?.removeEventListener("scroll", checkScrollPercentage);
});

function checkScrollPercentage(): void {
  //calculates how much a user has scrolled by a percentage
  const getScrollPercent: number = getScrollPercentage(
    scrollContainer,
    props.commentsMessage ? true : false
  );
  // if the scroll is over 99% it will get more comments only if it is not already getting new data
  if (getScrollPercent > 99 && !props.isLoadingData) {
    emit("loadComments");
  }
}

const emit = defineEmits<{
  (e: "commentCreated"): void;
  (e: "loadComments"): void;
}>();
</script>

<template>
  <article class="comments-panel">
    <div class="comment-feed" id="comment-feed">
      <CommentContent
        class="single-comment"
        v-for="comment in props.comments"
        :key="comment.content"
        :comment="comment"
      />
      <div v-if="isLoadingData">
        <SpinnerLoader :lightTheme="true" />
      </div>

      <div v-if="commentsMessage">{{ commentsMessage }}</div>
    </div>

    <CommentInput
      class="comment-input"
      @commentCreated="emit('commentCreated')"
    />
  </article>
</template>

<style scoped>
.comments-panel {
  height: 100%;
  justify-self: center;
  display: flex;
  flex-direction: column;
  width: 50%;
  background-color: var(--primary-color);
  border-radius: var(--border-radius-amount);
}

.comment-feed {
  flex: 1;
  width: 100%;
  padding: 5px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.single-comment {
  margin-bottom: 5px;
  padding: 5px;
  width: 100%;
}
</style>
