<script setup lang="ts">
import type { Message } from "@/models/messages/message.model";
import { computed, type PropType, type ComputedRef } from "vue";
import ChatMessage from "@/components/chat-and-messages/common/ChatMessage.vue";
import { sortByDateArray } from "@/helpers/messages-date.helper";

const props = defineProps({
  chatMessages: {
    required: true,
    type: Object as PropType<Message[]>,
  },
});
//This ordered the messages by date time so that the newest messages appear at the bottom.
let orderedChatMessages: ComputedRef<Message[]> = computed(() => {
  return sortByDateArray(props.chatMessages, "sent") as Message[];
});
</script>

<template>
  <div class="messages-container">
    <ChatMessage
      v-for="message in orderedChatMessages"
      :key="message.id"
      :message="message"
    ></ChatMessage>
  </div>
</template>

<style lang="scss">
.messages-container {
  display: flex;
  // reverse so that you can scroll from the bottom
  flex-direction: column-reverse;
  padding: 1rem;
  flex: 1;
  gap: 20px;
  overflow-y: auto;
  height: 100%;
  width: 100%;
}
</style>
