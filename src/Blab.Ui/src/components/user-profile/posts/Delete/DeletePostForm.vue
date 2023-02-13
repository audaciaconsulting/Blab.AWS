<template>
  <!-- This is the "Yes button to be styled with red text" -->
  <!-- the button will cause the blab to be deleted.  -->
  <div class="delete-post-button-container">
    <input
      class="error"
      @click="deleteBlab"
      type="button"
      value="Yes"
      data-test="delete-post-yes"
    />
    <!-- This is the 'no' button which will close the modal -->
    <input
      @click="closeModal"
      type="button"
      value="No"
      data-test="delete-post-no"
    />
  </div>
</template>

<script setup lang="ts">
const emit = defineEmits<{
  (e: "closeModal"): void;
  (e: "deleteBlab"): void;
}>();

function closeModal(): void {
  emit("closeModal");
}

function deleteBlab(): void {
  emit("deleteBlab");
}
</script>

<style scoped lang="scss">
.delete-post-button-container {
  display: flex;
  justify-content: center;
  align-content: center;
  width: 60%;
  gap: 30px;
  margin: auto;

  .input-error-container {
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: flex-start;
  }
  input {
    padding: 0.5rem 2rem;
    background-color: var(--secondary-color);
    border-radius: var(--border-radius-amount);
    outline: none;
    cursor: pointer;
    border: none;

    &:first-child {
      font-weight: 700;
    }
  }
  img {
    width: 22px;
  }
  .response-container {
    flex: 1;
  }
  .error,
  .success {
    text-align: left;

    font-size: 1.2rem;
    font-weight: 800;
  }
  .error {
    color: rgb(249, 58, 58);
  }
  section {
    display: flex;
    width: 100%;
    gap: 20px;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
