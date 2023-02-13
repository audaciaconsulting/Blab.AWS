<template>
  <div class="details-container">
    <h2>{{ props.displayName }}</h2>
    <h3>@{{ props.handle }}</h3>
    <p v-html="stringToUrls(props.bio)"></p>
  </div>
</template>

<script setup lang="ts">
import { stringToUrls } from "@/helpers/string-to-urls.helper";

const props = defineProps({
  handle: { required: true, type: String },
  displayName: { required: true, type: String },
  bio: { required: true, type: String },
});
</script>

<style scoped lang="scss">
.details-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
  width: 50%;
  overflow-wrap: break-word;
  margin-right: 40%;

  h2 {
    font-size: 2rem;
    font-weight: 800;
  }

  h3 {
    font-size: 1.33rem;
    font-weight: 600;
  }
  p {
    color: white;
    font-weight: 400;
    width: 70%;
    :deep(a) {
      color: var(--icon-color);
      font-style: italic;
      font-size: 700;
      text-decoration: none;
    }
  }
}
</style>
