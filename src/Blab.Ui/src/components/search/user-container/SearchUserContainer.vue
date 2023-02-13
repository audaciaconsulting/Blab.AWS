<template>
  <!-- This component loads the container where the users searched will be displayed -->
  <transition v-if="props.loadingUsers">
    <!-- while loading the data a loader will appear -->
    <SpinnerLoader :lightTheme="true" />
  </transition>
  <template v-else>
    <div v-if="props.users.length" class="searched-users-container">
      <SearchedUser
        @click="closeContainer"
        v-for="user in props.users"
        :key="user.userId"
        :userDetails="user"
      />
      <!-- if there are more users to collect a button can be clicked to load more users with the same search term -->
      <button
        @click.prevent="loadMoreUsers"
        v-if="props.areMoreUsers"
        data-test="load-more-users-button"
      >
        <!-- If more users are being loaded the button will have a spinner inside to let the user know data is being collected -->
        <span> Load More Users... </span>
        <!-- <SpinnerLoader :light-theme="true" v-else /> -->
      </button>
    </div>

    <template v-else>
      <span v-if="message"> {{ props.message }} </span>
      <!-- If no users re returned a message will appear to let the user know -->
      <span v-else>There are no matching users with this search word...</span>
    </template>
  </template>
</template>

<script setup lang="ts">
import SpinnerLoader from "@/components/spinners/SpinnerLoader.vue";
import type { ISearchUser } from "@/models/search/search-user.interface";
import type { PropType } from "vue";
import SearchedUser from "./SearchedUser.vue";
const props = defineProps({
  users: {
    required: true,
    type: Array as PropType<ISearchUser[]>,
  },
  areMoreUsers: {
    required: true,
    type: Boolean,
  },
  message: {
    required: true,
    type: String,
  },
  loadingUsers: {
    required: true,
    type: Boolean,
  },
});
const emit = defineEmits<{
  (e: "closeSearchContainer"): void;
  (e: "loadMoreUsers"): void;
}>();
function closeContainer(): void {
  // closes the search container and resets the search
  emit("closeSearchContainer");
}

function loadMoreUsers(): void {
  // tells the parent to load more users
  emit("loadMoreUsers");
}
</script>

<style scoped lang="scss">
.searched-users-container {
  top: 80%;
  position: absolute;
  width: 50%;
  max-height: calc(110px * 5);
  left: calc(50% - 25%);
  background-color: var(--primary-color);
  box-shadow: var(--box-shadow);
  border-radius: var(--border-radius-amount);
  z-index: 1;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  flex-direction: column;
  padding: 1rem;
  overflow-y: auto;

  button {
    background-color: var(--background-color);
    border-radius: 1rem;
    width: 40%;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 0.66rem;
    margin-top: 1rem;
    color: var(--icon-color);
    font-weight: 800;
    font-size: 1.2rem;
  }
}
</style>
