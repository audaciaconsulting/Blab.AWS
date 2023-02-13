<template>
  <div class="header-container">
    <h3>Are you sure you want to delete this blab?</h3>
  </div>
</template>

<script setup lang="ts">
const emit = defineEmits<{ (e: "openDeleteModal"): void }>();
{
  emit("openDeleteModal");
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
    text-align: center;
    font-weight: 800;
    font-size: 2rem;
  }
}
</style>
