import { createPinia } from "pinia";
import { setActivePinia } from "pinia";
import UserProfileView from "@/views/UserProfileView.vue";
import { describe, it, expect, beforeEach, vi } from "vitest";
import { mount } from "@vue/test-utils";
import { createRouter, createWebHistory } from "vue-router";
import routes from "@/router/routes/routes";

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

beforeEach(() => {
  setActivePinia(createPinia());
});

describe("UserProfileView", () => {
  it("User Profile view renders properly", () => {
    const userProfileView = mount(UserProfileView, {
      global: {
        plugins: [router],
      },
    });

    expect(userProfileView);
  });
});
