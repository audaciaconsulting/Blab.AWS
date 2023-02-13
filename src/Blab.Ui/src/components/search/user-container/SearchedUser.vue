<template>
  <!-- This component displays each user and their account details in the search container which links to the users account -->
  <router-link
    :to="{ name: 'UserProfile', params: { handle: props.userDetails.handle } }"
    class="searched-user-container"
    data-test="searched-user-profile-page"
  >
    <img
      src="@/assets/default-profile-picture.jpg"
      alt="users profile picture"
    />
    <div class="user-details">
      <h3>{{ props.userDetails.displayName }}</h3>
      <span>@{{ props.userDetails.handle }}</span>
    </div>
  </router-link>
</template>

<script setup lang="ts">
import type { ISearchUser } from "@/models/search/search-user.interface";
import type { PropType } from "vue";
const props = defineProps({
  userDetails: {
    required: true,
    type: Object as PropType<ISearchUser>,
  },
});
</script>

<style scoped lang="scss">
.searched-user-container {
  width: 100%;
  display: flex;
  gap: 20px;
  text-decoration: none;
  color: #fff;
  justify-content: flex-start;
  align-items: center;
  background-color: transparent;
  height: 100px;
  flex-shrink: 0;
  transition: background-color 200ms ease-in-out;
  padding: 1rem;
  border-bottom: var(--border-bottom) 2px solid;

  &:hover {
    background-color: var(--background-color);
  }
  img {
    height: 65px;
    width: 65px;
    border-radius: 50%;
  }
  .user-details {
    display: flex;
    flex-direction: column;

    h3 {
      font-weight: 900;
      font-size: 1.2rem;
    }
    span {
      color: var(--secondary-color);
      font-size: 1rem;
    }
  }
}
</style>
