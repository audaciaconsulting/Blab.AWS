import { createPinia } from "pinia";
import { setActivePinia } from "pinia";
import { describe, it, expect, beforeEach, vi } from "vitest";
import { mount } from "@vue/test-utils";
import ReactionComponentVue from "../post/reaction/ReactionComponent.vue";

beforeEach(() => {
  setActivePinia(createPinia());
});

describe("ReactionComponent", () => {
  it("Reaction Component renders properly", () => {
    const reactionComponent = mount(ReactionComponentVue);

    expect(reactionComponent);
  });
});
