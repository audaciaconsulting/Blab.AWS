<script setup lang="ts">
import { MessagesService } from "@/services/messages.service";
import { useProfileStore } from "@/stores/user-profile.store";
import type { ApiResponse } from "@/models/api/api-response.model";
import { computed, ref, type PropType, type Ref } from "vue";
import { AddMessage } from "@/models/messages/add-message.model";
import { Message } from "@/models/messages/message.model";

const props = defineProps({
  chatId: { required: false, type: Object as PropType<number> },
});
const emits = defineEmits<{ (e: "sendAMessage", message: Message): void }>();

const messagesService = new MessagesService();
const userStore = useProfileStore();
const theChatId: number = props.chatId as number;
const loggedInUserId: Ref<number> = computed(() => {
  return userStore.userDetails.userId;
});

const showErrorMessage: Ref<boolean> = ref(false);
let model: Ref<Message> = ref(new Message());
let newMessage: Ref<AddMessage> = ref(new AddMessage());
const disableSendButton = computed(
  () =>
    newMessage.value.content === undefined || newMessage.value.content === ""
);

function sendAMessage(): void {
  newMessage.value.userId = loggedInUserId.value;
  messagesService
    .addMessage(theChatId, newMessage.value)
    .then((messageResponse: ApiResponse) => {
      if (messageResponse.statusCode === 201) {
        // emit the 'messageResponse.data' to ChatView, where it will be added to the array of ChatMessages.
        model.value = new Message();
        model.value.id = messageResponse.data.id;
        model.value.userId = messageResponse.data.userId;
        model.value.content = messageResponse.data.content;
        model.value.sent = messageResponse.data.sent;
        emits("sendAMessage", model.value);
        //This resets the content back to empty after the message has been sent to the api and been successfully added.
        newMessage.value.content = "";
      } else {
        showErrorMessage.value = true;
      }
    });
}
</script>

<template>
  <form class="message-container">
    <input
      v-model="newMessage.content"
      type="text"
      data-test="add-message-input"
      placeholder="Blab something"
      @keydown.enter.exact.prevent="sendAMessage"
    />
    <p v-if="showErrorMessage">Message couldn't be sent.</p>
    <button
      type="button"
      :disabled="disableSendButton"
      data-test="send-message-button"
      @click="sendAMessage"
    >
      <img src="@/assets/icons/send.svg" alt="send icon" />
    </button>
  </form>
</template>

<style lang="scss">
.send-button {
  float: right;
  padding: 1vmin;
}

.message-container {
  bottom: 0%;
  background-color: transparent;
  display: flex;
  align-items: center;
  width: 100%;

  input {
    flex: 1;
    margin: 0.5rem;
    background-color: var(--background-color);
    outline: none;
    border: none;
    padding: 0.8rem;
    border-radius: 10rem;
    color: #fff;
  }

  button {
    background-color: transparent;
  }
}
.disabled-button {
  pointer-events: none;
  opacity: 0.3;
}

::placeholder {
  color: white;
}
</style>
