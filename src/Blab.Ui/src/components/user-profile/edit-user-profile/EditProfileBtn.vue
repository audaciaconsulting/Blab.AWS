<template>
  <!-- This button makes the modal visable to the user by using the emit to the parent Component EditProfile -->
  <button
    @click="changeEditState"
    data-test="test-edit-profile"
    src="@/assets/icons/settings.svg"
  >
    <img src="@/assets/icons/settings.svg" alt="settings icon" />
  </button>
</template>

<script setup lang="ts">
const emit = defineEmits<{ (e: "toggleEditState"): void }>();

function changeEditState(): void {
  emit("toggleEditState");
}
</script>

<style scoped lang="scss">
button {
  background-color: transparent;
  width: 60px;
  transition: transform 0.15s ease-in-out;

  &:hover {
    transform: scale(1.35);
  }
  img {
    max-width: 100%;
  }
}
</style>
