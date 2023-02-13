<template>
  <header>
    <nav class="wrapper">
      <img
        alt="Blab logo"
        class="logo"
        src="@/assets/main-logo.png"
        width="125"
        height="125"
      />

      <RouterLink to="/" data-test="home-btn">
        <IconSvg svg-key="/src/assets/icons/home.svg" />
        Home</RouterLink
      >
      <RouterLink
        v-if="userStore.userDetails.handle"
        data-test="user-profile-btn"
        :to="{
          name: 'UserProfile',
          params: { handle: userStore.userDetails.handle },
        }"
      >
        <IconSvg svg-key="/src/assets/icons/profile-nav.svg" />
        Profile</RouterLink
      >
      <RouterLink
        v-if="userStore.userDetails.handle"
        data-test="chats-btn"
        :to="{
          name: 'Chats',
        }"
      >
        <IconSvg svg-key="/src/assets/icons/chats.svg" />
        Chats</RouterLink
      >
      <button data-test="logout-button" @click="logOutUser">
        <IconSvg svg-key="/src/assets/icons/logout.svg" />Log out
      </button>
    </nav>
  </header>
</template>

<script setup lang="ts">
import { AuthService } from "@/services/auth.service";
import { RouterLink } from "vue-router";
import { useProfileStore } from "@/stores/user-profile.store";
import IconSvg from "./IconSvg.vue";
function logOutUser(): void {
  authService.logout();
}
const userStore = useProfileStore();
const authService = new AuthService();
</script>

<style lang="scss">
header {
  nav {
    background-color: var(--sidebar-color);
    margin: 0px calc(16px / 2);
    display: flex;
    flex-direction: column;
    align-items: center;
    height: calc(100vh - 16px);
    width: 100%;
    gap: 10px;
    padding: 16px;
    border-radius: var(--border-radius-amount);
  }
  a,
  button {
    display: flex;
    justify-content: space-around;
    align-items: center;
    padding: 14px;
    border-radius: 4px;
    color: var(--secondary-color);
    width: 100%;
    border: 2px var(--secondary-color) solid;
    text-decoration: none;
    background-color: var(--sidebar-color);
    transition: all 200ms ease-in-out;
  }
  .router-link-exact-active,
  a:hover,
  button:hover {
    background-color: var(--secondary-color);
    color: var(--sidebar-color);
    &:hover {
      svg {
        path {
          fill: var(--sidebar-color);
        }
        rect {
          fill: var(--secondary-color);
        }
      }
      [data-outline-chat] {
        fill: var(--secondary-color);
      }

      [data-logout-door] {
        fill: var(--sidebar-color);
      }
      [data-outline-arrow] {
        fill: var(--secondary-color);
      }
    }
  }
  [data-logout-svg] {
    path {
      fill: var(--secondary-color);
    }
    rect {
      fill: var(--sidebar-color);
    }
    [data-logout-door] {
      fill: var(--secondary-color);
    }
    [data-outline-arrow] {
      fill: var(--sidebar-color);
    }
  }
  .router-link-exact-active {
    font-weight: 800;
    svg {
      path {
        fill: var(--sidebar-color);
      }
      rect {
        fill: var(--secondary-color);
      }
    }
    [data-outline-chat] {
      fill: var(--secondary-color);
    }
  }
}
</style>
