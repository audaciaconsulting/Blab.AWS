<template>
  <div class="header-container">
    <CloseComponent @close-toggle="closeModel" />
    <h2>Edit Profile</h2>
  </div>
</template>

<script setup lang="ts">
import CloseComponent from "@/components/buttons/CloseComponent.vue";

const emit = defineEmits<{ (e: "toggleEditState"): void }>();
function closeModel(): void {
  emit("toggleEditState");
}
</script>

<style scoped lang="scss">
.header-container {
  max-height: 25%;
  width: 100%;
  display: flex;
  gap: 30px;
  align-items: center;
  h2 {
    font-weight: 800;
    font-size: 2rem;
  }
}
</style>
