import { createPinia } from "pinia";
import { setActivePinia } from "pinia";

import { describe, it, expect, beforeEach, vi } from "vitest";
import { mount } from "@vue/test-utils";
import EditProfileVue from "../user-profile/edit-user-profile/EditProfile.vue";

beforeEach(() => {
  setActivePinia(createPinia());
});

describe("Edit Profile Component", () => {
  it("Edit Profile Component renders properly", () => {
    const mockRoute = { params: { handle: "tester" } };

    const editProfileComponent = mount(EditProfileVue, {
      global: {
        mocks: {
          $route: mockRoute,
        },
      },
    });

    expect(editProfileComponent);
  });
});
