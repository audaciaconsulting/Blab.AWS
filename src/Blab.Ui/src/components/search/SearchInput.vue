<template>
  <!-- This component is the search bar for searching for a user -->
  <!-- If the user focuses on the search bar the search bar will expand -->
  <div
    class="search-input-container"
    :class="expandSearch ? 'expand' : ''"
    @focusout="closeSearchContainer"
    @focusin="expandSearch = true"
  >
    <button data-test="search-users-button" @click="updateTextAfterDelay">
      <img src="@/assets/icons/search.svg" alt="search icon" />
    </button>
    <!-- This is where the user will input the search term and the debounce function will run every time the user types -->
    <input
      data-test="search-input"
      type="text"
      placeholder="Search Here..."
      v-model.trim="searchText"
      @keydown.enter.exact.prevent="updateTextAfterDelay"
      @keyup="updateTextAfterDelay"
    />
  </div>
</template>

<script setup lang="ts">
import { debounce } from "@/helpers/debounce.helper";
import { type Ref, ref } from "vue";
const emit = defineEmits<{
  (e: "updateSearchText", type: string): void;
  (e: "closeSearchContainer"): void;
}>();
const props = defineProps({
  isLoadingMoreUsers: { required: true, type: Boolean },
});
const expandSearch: Ref<boolean> = ref(false);
const searchText: Ref<string> = ref("");
const updateTextAfterDelay = debounce((): void => {
  //this will emit to the parent only if the user stops typing for 1 second and the input length is >= 2. this function updates the search term on the parent component
  emit("updateSearchText", searchText.value);
});

function closeSearchContainer(): void {
  // Emits to the parent to reset the search state back to default
  if (!props.isLoadingMoreUsers) {
    expandSearch.value = false;
    // so if a user clicks on a user the router link will still work
    setTimeout(() => {
      emit("closeSearchContainer");
      searchText.value = "";
    }, 300);
  }
}
</script>

<style scoped lang="scss">
.search-input-container {
  background-color: var(--primary-color);
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 2rem 0;
  border-radius: 2rem;
  width: 40%;
  box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25);
  transition: width 200ms ease;
  img {
    padding: 0.5rem;
  }
  input {
    font-size: 1.1rem;
    flex: 1;
    background-color: transparent;
    border: none;
    color: var(--secondary-color);
  }
  button {
    background-color: transparent;
  }
}
.expand {
  width: 50%;
}
</style>
