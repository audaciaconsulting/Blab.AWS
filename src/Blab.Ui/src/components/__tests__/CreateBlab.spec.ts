import { createPinia } from "pinia";
import { setActivePinia } from "pinia";
import { describe, it, expect, beforeEach, vi } from "vitest";
import { mount } from "@vue/test-utils";
import CreateBlabVue from "../blabs/CreateBlab.vue";

beforeEach(() => {
  setActivePinia(createPinia());
});

describe("mounting Create Blab Component", () => {
  it("Create blab loads properly", () => {
    const mockRoute = { params: { handle: "tester" } };
    const createBlab = mount(CreateBlabVue, {
      global: {
        mocks: {
          $route: mockRoute,
        },
      },
    });

    expect(createBlab);
  });
});
