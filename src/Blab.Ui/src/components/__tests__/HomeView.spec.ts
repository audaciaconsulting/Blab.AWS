import HomeView from "@/views/HomeView.vue";
import { createPinia } from "pinia";
import { setActivePinia } from "pinia";

import { describe, it, expect, beforeEach, vi } from "vitest";
import { mount } from "@vue/test-utils";

beforeEach(() => {
  setActivePinia(createPinia());
});

describe("HomeView", () => {
  it("Homeview View renders properly", () => {
    const homeViewTest = mount(HomeView);

    expect(homeViewTest);
  });
});
