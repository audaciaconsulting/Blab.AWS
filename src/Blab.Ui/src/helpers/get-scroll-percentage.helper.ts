import type { Ref } from "vue";

export function getScrollPercentage(
  scrollContainer: Ref<HTMLElement | null>,
  isDisplayingMessage: boolean
): number {
  // checks if the scroll Container is not null and  if a error message is displayed
  if (scrollContainer.value !== null && !isDisplayingMessage) {
    return (
      (scrollContainer.value.scrollTop /
        (scrollContainer.value.scrollHeight -
          scrollContainer.value.clientHeight)) *
      100
    );
  }
  return 0;
}
