<template>
  <div v-html="svg"></div>
</template>

<script setup lang="ts">
import { type Ref, ref } from "vue";

const props = defineProps({
  svgKey: { required: true, type: String },
});
const svg: Ref<null | string> = ref(null);
// This gets all of the svgs inside the svg icons folder
const getSvgs: Record<string, string> = import.meta.glob(
  "@/assets/icons/*.svg",
  {
    as: "raw",
    eager: true,
  }
);
// gets the correct svg by using the key the user has passed into this component
svg.value = getSvgs[props.svgKey];
</script>

<style scoped lang="scss">
:deep(svg) {
  width: 20px;
  height: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  path {
    fill: var(--secondary-color);
  }
  rect {
    fill: var(--sidebar-color);
  }
}

:deep([data-outline-chat]) {
  fill: var(--sidebar-color);
}

[data-outline-chat] {
  fill: var(--secondary-color);
}
</style>
