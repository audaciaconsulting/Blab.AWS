import { createPinia } from "pinia";
import { setActivePinia } from "pinia";

import { describe, it, expect, beforeEach } from "vitest";
import { mount } from "@vue/test-utils";
import SearchComponentVue from "../search/SearchComponent.vue";
beforeEach(() => {
  setActivePinia(createPinia());
});
describe("Search Component", () => {
  it("Search Component renders properly", () => {
    const SearchComponentVueTest = mount(SearchComponentVue);

    expect(SearchComponentVueTest);
  });
});
