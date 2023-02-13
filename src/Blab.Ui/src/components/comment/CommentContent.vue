<script setup lang="ts">
import type { IComment } from "@/models/comments/comment.interface";
import { computed, type PropType } from "vue";
import dayjs from "dayjs";
import { profilePhotoSelector } from "@/helpers/profile-photo-selector";

const props = defineProps({
  comment: { required: true, type: Object as PropType<IComment> },
});
const date = props.comment.created as Date;

const imageSrc = computed(() => {
  return profilePhotoSelector(props.comment.profilePhotoBlob);
});
</script>

<template>
  <article>
    <div class="comment-content-container">
      <div class="profile-header">
        <img :src="imageSrc" alt="profile picture" />
        <div class="handle-display-container">
          <h2>{{ props.comment.displayName }}</h2>
          <span>@{{ props.comment.handle }}</span>
        </div>
      </div>
      <p class="comment-content">
        {{ props.comment.content }}
      </p>
      <span>{{ dayjs(date).format("HH:mm MMM DD, YYYY") }}</span>
    </div>
  </article>
</template>

<style scoped lang="scss">
.comment-content-container {
  display: flex;
  flex-direction: column;
  border-bottom: 1px var(--background-color) solid;
  width: 100%;

  span {
    margin-bottom: calc(10px);
  }
}
.comment-content {
  word-wrap: break-word;
  font-size: 1.1rem;
  padding-bottom: 10px;
}
.handle-display-container {
  width: 85%;
  display: flex;
  flex-direction: column;
  h2,
  span {
    width: 100%;
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
  }
  span {
    display: flex;
    color: rgba($color: #ffffff, $alpha: 0.75);
    flex-direction: column;
    border-radius: 20%;
    font-size: 1rem;
    font-weight: 600;
  }
}
.profile-header {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  gap: 20px;
  img {
    width: 40px;
    height: 40px;
    border-radius: 50%;
  }
}
</style>
