<template>
  <div>
    <div class="spinner-container" :data-light-theme="props.lightTheme">
      <div></div>
      <div></div>
      <div></div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { defineProps } from "vue";
const props = defineProps({
  lightTheme: { required: false, type: Boolean },
});
</script>
<style scoped lang="scss">
.spinner-container {
  width: 50px;
  height: 50px;
  position: relative;
  margin: 0.1rem;

  &[data-light-theme="true"] {
    div {
      border-color: var(--secondary-color) transparent transparent transparent;
    }
  }
  div {
    border-radius: 50%;
    position: absolute;
    width: 100%;
    height: 100%;
    border: solid 8px;
    border-color: var(--background-color) transparent transparent transparent;
    animation: rotateShrink 1s cubic-bezier(0.5, 0.2, 0.55, 1) infinite;

    &:nth-child(1) {
      animation-delay: -200ms;
    }
    &:nth-child(2) {
      animation-delay: -100ms;
    }
  }
}

@keyframes rotateShrink {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
