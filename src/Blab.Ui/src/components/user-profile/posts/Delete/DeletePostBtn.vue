<template>
  <!-- This button makes the "Delete Post" modal visable to the user -->
  <button
    class="delete-btn"
    @click="deletePostState"
    data-test="test-delete-post"
  >
    <img src="@/assets/icons/bin.svg" alt="bin icon" />
    Delete
  </button>
</template>

<script setup lang="ts">
const emits = defineEmits<{
  (e: "openDeleteModal"): void;
}>();

function deletePostState() {
  emits("openDeleteModal");
}
</script>

<style scoped lang="scss">
input[type="button"] {
  width: 100px;
  height: 30px;
}

.delete-btn {
  width: 100px;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 4px;
  margin: 10px;
  border-radius: 4px;
  color: white;
  background-color: var(--sidebar-color);

  img {
    width: 1.5rem;
  }
}
</style>
