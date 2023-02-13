<script setup lang="ts">
import { ChatService } from "@/services/chats.service";
import { useRouter } from "vue-router";
import { useProfileStore } from "@/stores/user-profile.store";
import type { ApiResponse } from "@/models/api/api-response.model";
import type { IUserProfileDetails } from "@/models/user-profile/user-profile-details.interface";
import { ref, type PropType, type Ref } from "vue";
import { Chat } from "@/models/user-chats/chat.model";

const props = defineProps({
  userDetails: {
    required: true,
    type: Object as PropType<IUserProfileDetails>,
  },
});

const chatService = new ChatService();
const userStore = useProfileStore();
const router = useRouter();
const errorMessage: Ref<string> = ref("");

function addChat(): void {
  const chat = createChat();
  chatService.createNewChat(chat).then((response: ApiResponse) => {
    const chatId = response.data;
    router.push({
      name: "Chat",
      params: { chatId },
    });
  });
}

function findChatId(): void {
  const chat = createChat();
  chatService.getChatID(chat).then((response: ApiResponse) => {
    if (response.statusCode === 200) {
      const chatId = response.data;
      router.push({
        name: "Chat",
        params: { chatId },
      });
    } else if (response.statusCode === 404) {
      addChat();
    } else {
      errorMessage.value = "Sorry, something went wrong.";
    }
  });
}
function createChat(): Chat {
  const chat = new Chat();
  chat.loggedInUserId = userStore.userDetails.userId;
  chat.otherUserId = props.userDetails.userId;
  return chat;
}
</script>

<template>
  <button
    class="message-button"
    @click="findChatId()"
    data-test="message-button"
  >
    Message
  </button>
  <p class="error" data-test="save-blab-error-message">
    {{ errorMessage }}
  </p>
</template>

<style>
.message-button {
  display: flex;
  cursor: pointer;
  padding: 1vmin;
  background-color: var(--secondary-color);
  color: white;
  text-decoration: none;
  font-size: 1.2rem;
  border-radius: 0.5rem;
  align-self: flex-end;
  justify-content: flex-end;
}
.error {
  text-align: left;
  color: rgb(249, 58, 58);
  font-size: 1.2rem;
  font-weight: 800;
}
</style>
