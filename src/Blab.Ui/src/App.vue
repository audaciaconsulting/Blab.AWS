<template>
  <SpinnerLoader v-if="loadingUserDetails" :light-theme="true" />
  <div v-else class="master-container">
    <NavigationComponent />

    <main>
      <SearchComponent v-if="!isChatView" />
      <RouterView />
      <notifications classes="delete-notification" />
    </main>
    <div class="trending-container"></div>
  </div>
</template>

<script setup lang="ts">
import { RouterView, useRouter } from "vue-router";
import { useProfileStore } from "@/stores/user-profile.store";
import { computed, type Ref } from "vue";
import { ref } from "vue";
import SearchComponent from "@/components/search/SearchComponent.vue";
import SpinnerLoader from "@/components/spinners/SpinnerLoader.vue";
import NavigationComponent from "@/components/navigation/NavigationComponent.vue";

const userStore = useProfileStore();
const router = useRouter();
let loadingUserDetails: Ref<boolean> = ref(true);

userStore.getUserDetails().then(() => (loadingUserDetails.value = false));

// If on the ChatView, don't show the search component.
const isChatView = computed(
  () =>
    router.currentRoute.value.name === "Chat" ||
    router.currentRoute.value.name === "Chats"
);
</script>

<style lang="scss">
.master-container {
  display: flex;
  justify-content: center;
}
header,
.trending-container {
  z-index: 1;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  height: 100vh;
  position: sticky;
  min-width: var(--navBarWidth);
  background-color: var(--background-color);
  left: 0;
  top: 0;
}

button {
  cursor: pointer;
  font-size: 1rem;
  outline: none;
  border: none;
}

input[type="text"],
textarea {
  width: 100%;
  background-color: var(--background-color);
  border: none;
  padding: 0.5rem;
  color: var(--secondary-color);
  font-size: 1.2rem;
  border-bottom: var(--icon-color) solid 2px;
  &:focus-visible {
    box-shadow: none;
    outline: none;
  }
  &::placeholder {
    color: var(--secondary-color);
  }
}
// style of the notification itself
.delete-notification {
  margin: 50px 50px 5px 5px;
  padding: 20px;
  font-size: 15px;
  color: #ffffff;
  background: var(--secondary-color) !important;
}
.trending-container {
  right: 0;
}
main {
  width: calc(100% - (var(--navBarWidth) * 2));
  display: flex;
  flex-direction: column;
  flex-shrink: 0;
}
</style>
