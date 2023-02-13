<script setup lang="ts">
//This chat view is for when displaying messages in a chat with a user when you click the message button on their user profile.
// The styling is different as certain elements are displayed, like the chat navigation menu.
import { useRoute, useRouter } from "vue-router";
import { MessagesService } from "@/services/messages.service";
import { useProfileStore } from "@/stores/user-profile.store";
import type { ApiResponse } from "@/models/api/api-response.model";
import { nextTick, onMounted, ref, type Ref } from "vue";
import MessageInput from "@/components/chat-and-messages/common/MessageInput.vue";
import DisplayMessages from "@/components/chat-and-messages/common/DisplayMessages.vue";
import type { Message } from "@/models/messages/message.model";
import { StatusCodes } from "http-status-codes";

const messagesService = new MessagesService();
const userStore = useProfileStore();

const chatMessages: Ref<Message[]> = ref([]);
const route = useRoute();
let theChatId: number = route.params.chatId as unknown as number;
const router = useRouter();

const loggedInUserId: Ref<number | null> = ref(null);

const errorMessage: Ref<string> = ref("");

function goBack(): void {
  // This is the function for the back button, that will take the user to the last page the previous page that they were on.
  router.back();
}

// get messages - populate 'chatMessages'
onMounted(() => {
  loggedInUserId.value = userStore.userDetails.userId;
  messagesService.getMessages(theChatId).then((response: ApiResponse) => {
    if (response.statusCode === StatusCodes.OK) {
      chatMessages.value = response.data;
    } else {
      errorMessage.value = "Sorry, something went wrong.";
    }
  });
});

const listElement = ref<any | null>(null);
// the emit from component MessageInput - add message to 'chatMessage'
async function displayNewMessage(newMessage: Message): Promise<void> {
  // The reason why we push ...newMessage is because before it was still bounded to the v-model even after pressing the button, so you could edit the message unless the button was pressed again.
  //This creates a "mock" version of the newMessage to solve the issue.
  chatMessages.value.push({ ...newMessage });
  //This awaits for Vue to update the DOM after our new message after which we scroll to the bottom of our messages so that after every message sent the user does not have scroll every time.
  await nextTick();
  const el = document.querySelector(".messages-container") as HTMLElement;
  if (el) {
    window.scrollTo(0, el.scrollHeight);
    el.scrollTop = el.scrollHeight;
  }
}
</script>

<template>
  <div class="parent">
    <div class="chat-view-container">
      <button
        type="button"
        class="back-button"
        data-test="go-back-from-chat-btn"
        @click="goBack"
      >
        Back
      </button>
      <p class="error" data-test="save-blab-error-message">
        {{ errorMessage }}
      </p>
      <!-- Pass the messages as a prop -->
      <DisplayMessages :chatMessages="chatMessages" ref="listElement" />
      <!-- Get the emitted data of the new message from the Message Input -->
      <MessageInput @sendAMessage="displayNewMessage" :chatId="theChatId" />
    </div>
  </div>
</template>

<style lang="scss">
.parent {
  padding: 1rem;
  height: 100vh;
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  grid-template-rows: repeat(10, 1fr);
  gap: 10px;
}

.chat-view-container {
  width: 100%;
  display: flex;
  flex-direction: column;
  background-color: var(--primary-color);
  border-radius: var(--border-radius-amount);
  justify-content: flex-end;
  grid-column: 2/-1;
  grid-row: 2/-1;
  height: 100%;
}
.header {
  color: 003C4D;
}

.back-button {
  cursor: pointer;
  margin-left: 2rem;
  padding: 1vmin;
  background-color: var(--secondary-color);
  color: white;
  text-decoration: none;
  font-size: 1.2rem;
  border-radius: 0.5rem;
  width: 10%;
  justify-self: flex-start;
}

.error {
  text-align: left;
  color: rgb(249, 58, 58);
  font-size: 1.2rem;
  font-weight: 800;
}
</style>
