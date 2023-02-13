<script setup lang="ts">
import type { GetChatInfoResponse } from "@/models/user-chats/getChatInfoApiResponse.model";
import ChatNavigationPreviewVue from "./ChatNavigationPreview.vue";
import { computed, ref, type ComputedRef, type PropType, type Ref } from "vue";
import { sortByDateArray } from "@/helpers/messages-date.helper";

const showErrorMessage: Ref<boolean> = ref(false);

const props = defineProps({
  chats: {
    required: true,
    type: Object as PropType<GetChatInfoResponse[]>,
  },
});
const emits = defineEmits<{
  (e: "open:chat", chatId: number): void;
  (e: "otherUserDisplayName", name: string): void;
}>();

//This orders the chats by date time so that the latest messages appear at the top.
let orderedChats: ComputedRef<GetChatInfoResponse[]> = computed(() => {
  return sortByDateArray(
    props.chats,
    "timeOfLatestMessage"
  ) as GetChatInfoResponse[];
});

function openChat(chatId: number): void {
  emits("open:chat", chatId);
}
function displayOtherUserName(name: string): void {
  emits("otherUserDisplayName", name);
}
</script>

<template>
  <div class="chat-preview-parent-container">
    <ChatNavigationPreviewVue
      v-for="chat in orderedChats"
      :key="chat.chatId"
      :chat="chat"
      @open:chat="openChat"
      @otherUserDisplayName="displayOtherUserName"
    />
  </div>
  <p v-if="showErrorMessage">Something went wrong</p>
</template>

<style lang="scss">
.chat-preview-parent-container {
  display: flex;
  flex-direction: column;
  overflow-y: auto;
  gap: 10px;
  grid-column: 1/2;
  background-color: var(--primary-color);
  border-radius: 0.5rem;
  grid-row: 1/-1;
  width: 100%;
}
</style>
