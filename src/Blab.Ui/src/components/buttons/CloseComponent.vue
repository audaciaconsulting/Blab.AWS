<template>
  <button
    @click="changeToggleState"
    data-test="close-edit-button"
    alt="close icon"
  >
    <img src="@/assets/icons/close.svg" alt="Close icon" />
  </button>
</template>

<script setup lang="ts">
const emit = defineEmits<{ (e: "closeToggle"): void }>();

function changeToggleState(): void {
  emit("closeToggle");
}
</script>

<style scoped lang="scss">
button {
  background-color: transparent;
  width: 45px;
  transition: transform 0.1s linear;

  &:hover {
    transform: scale(1.25);
  }
  img {
    max-width: 100%;
  }
}
</style>
