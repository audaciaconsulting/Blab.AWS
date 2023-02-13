<script setup lang="ts">
//This chats view is what is displayed when the "chats" button is clicked in the navigation menu.
// What is displayed is different from the other chat view, in this one, the side view of all the user's chats are seen.
import ChatNavigationMenu from "@/components/chat-and-messages/messaging-from-chat-menu/ChatNavigationMenu.vue";
import DisplayMessages from "@/components/chat-and-messages/common/DisplayMessages.vue";
import MessageInput from "@/components/chat-and-messages/common/MessageInput.vue";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import type { Message } from "@/models/messages/message.model";
import type { GetChatInfoResponse } from "@/models/user-chats/getChatInfoApiResponse.model";
import { ChatService } from "@/services/chats.service";
import { MessagesService } from "@/services/messages.service";
import { StatusCodes } from "http-status-codes";
import { type Ref, ref, nextTick } from "vue";
import { useRouter } from "vue-router";

const chatService: ChatService = new ChatService();
const messageService: MessagesService = new MessagesService();
const chats: Ref<GetChatInfoResponse[]> = ref([]);
const selectedChatMessages: Ref<Message[]> = ref([]);
const showErrorMessage: Ref<boolean> = ref(false);
const router = useRouter();

function goBack(): void {
  // This is the function for the back button, that will take the user to the last page the previous page that they were on.
  router.back();
}

//This gets the list of chats that a user is apart of.
chatService
  .getChatInfo()
  .then((response: ApiResponseWithType<GetChatInfoResponse[]>) => {
    if (response.statusCode === StatusCodes.OK) {
      response.data.output.forEach((chat) => {
        if (chat.timeOfLatestMessage !== null) {
          chat.timeOfLatestMessage = new Date(chat.timeOfLatestMessage);
        }
      });
      chats.value = response.data.output;
    } else {
      showErrorMessage.value = true;
    }
  });

const chatIdOfOpenedChat: Ref<number> = ref(0);
const showMessageInputIfDisplayingAChat: Ref<boolean> = ref(false);

async function openChat(chatId: number): Promise<void> {
  const response = await messageService.getMessages(chatId);
  chatIdOfOpenedChat.value = chatId;
  showMessageInputIfDisplayingAChat.value = true;
  if (response.statusCode === StatusCodes.OK) {
    selectedChatMessages.value = response.data;
  } else {
    showErrorMessage.value = true;
  }
}
const otherUserDisplayNameDisplayed: Ref<string> = ref("");

function displayOtherUserDisplayName(name: string): void {
  otherUserDisplayNameDisplayed.value = name;
}

async function displayNewMessage(newMessage: Message): Promise<void> {
  // The reason why we push ...newMessage is because before it was still bounded to the v-model even after pressing the button, so you could edit the message unless the button was pressed again.
  //This creates a "mock" version of the newMessage to solve the issue.
  selectedChatMessages.value.push({ ...newMessage });
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
  <div>
    <div class="parent">
      <div class="chat-details">
        <button
          type="button"
          class="back-button-in-details"
          data-test="go-back-from-chat-btn"
          @click="goBack"
        >
          Back
        </button>
        <h1>
          {{ otherUserDisplayNameDisplayed }}
        </h1>
      </div>
      <ChatNavigationMenu
        :chats="chats"
        @open:chat="openChat"
        @otherUserDisplayName="displayOtherUserDisplayName"
      />
      <div class="chat-view-container chat-menu-container">
        <DisplayMessages :chatMessages="selectedChatMessages" />
        <MessageInput
          v-if="showMessageInputIfDisplayingAChat"
          @sendAMessage="displayNewMessage"
          :chatId="chatIdOfOpenedChat"
        />
        <p v-if="showErrorMessage">Sorry, something unexpected has happened.</p>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
/* Name of the user the logged in user is chatting with.*/
.chat-details {
  display: flex;
  grid-column: 2/-1;
  grid-row: 1;
  background-color: var(--primary-color);
  align-items: center;
  align-content: center;
  gap: 15px;
  font-weight: bold;
  border-radius: var(--border-radius-amount);
  color: var(--tertiary-color);
  padding: 20px;
  h1 {
    font-size: 1.8rem;
    font-weight: 800;
  }
}
.back-button-in-details {
  cursor: pointer;
  font-weight: 600;
  padding: 0.8rem;
  background-color: transparent;
  color: var(--secondary-color);
  text-decoration: none;
  border-radius: 0.5rem;
  border: solid 2px var(--secondary-color);
  transition: background-color, color 200ms ease;

  &:hover {
    background-color: var(--secondary-color);
    color: var(--primary-color);
  }
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
.chat-menu-container {
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: larger;
}
</style>
