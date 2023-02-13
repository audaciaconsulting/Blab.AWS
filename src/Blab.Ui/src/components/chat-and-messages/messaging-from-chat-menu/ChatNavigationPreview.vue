<script setup lang="ts">
import type { GetChatInfoResponse } from "@/models/user-chats/getChatInfoApiResponse.model";
import { timeLastMessageSent } from "@/helpers/last-message-time-helper";
import { computed, ref, type PropType, type Ref } from "vue";
import { MessagesService } from "@/services/messages.service";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import type { UpdateMessagesToReadResponse } from "@/models/messages/update-messages-to-read-response.model";
import { StatusCodes } from "http-status-codes";
import { useProfileStore } from "@/stores/user-profile.store";
import { profilePhotoSelector } from "@/helpers/profile-photo-selector";

const props = defineProps({
  chat: { required: true, type: Object as PropType<GetChatInfoResponse> },
});

//emit the name of the other user in the chat so that it can be displayed in the chat view
const emits = defineEmits<{
  (e: "otherUserDisplayName", otherUserDisplayName: string): void;
  (e: "open:chat", chatId: number): void;
}>();

const showErrorMessage: Ref<boolean> = ref(false);
const messagesService = new MessagesService();
const userStore = useProfileStore();

const loggedInUserId: Ref<number> = computed(() => {
  return userStore.userDetails.userId;
});

function takeToChat(): void {
  emits("open:chat", props.chat.chatId);
  emits("otherUserDisplayName", props.chat.otherUserDisplayName as string);
  if (
    !props.chat.hasLastMessageBeenRead &&
    props.chat.userIdOfLastMessageSender != loggedInUserId.value
  ) {
    messagesService
      .updateMessagesToRead(props.chat.chatId)
      .then((response: ApiResponseWithType<UpdateMessagesToReadResponse>) => {
        if (response.statusCode === StatusCodes.BAD_REQUEST) {
          showErrorMessage.value = true;
        }
      });
  }
}

//If the last message is unread, the css has font-weight bold so that the user can see which chat has been unread
const isContentUnread = computed(() =>
  props.chat.hasLastMessageBeenRead === true ||
  props.chat.userIdOfLastMessageSender === loggedInUserId.value
    ? "content"
    : "unread-content"
);

const imageSrc = computed(() => {
  return profilePhotoSelector(props.chat.profilePhotoBlob);
});
</script>

<template>
  <div
    class="chat-preview-container"
    @click="takeToChat()"
    data-test="individual-chat-panel"
  >
    <div class="user-details-container">
      <img
        class="profile-picture"
        :src="imageSrc"
        rounded="circle"
        alt="profile-picture"
      />
      <div class="user-info">
        <span class="displayName">{{ props.chat.otherUserDisplayName }}</span>
        <span :class="isContentUnread">{{
          props.chat.contentOfLastMessage
        }}</span>
        <span class="time">{{
          timeLastMessageSent(props.chat.timeOfLatestMessage)
        }}</span>
      </div>
    </div>
    <p v-if="showErrorMessage">Something went wrong</p>
  </div>
</template>

<style lang="scss">
.chat-preview-container {
  display: flex;
  cursor: pointer;
  padding: 1rem 0.2rem;
  border-bottom: 2px solid white;
  align-items: center;
  justify-content: space-between;
  max-width: 100%;

  .user-details-container {
    display: flex;
    align-items: center;
    justify-content: flex-start;
    gap: 5px;
    flex: 1;
    overflow-x: hidden;
  }
  .user-info {
    display: flex;
    flex-direction: column;
    flex: 1;
  }
  .content,
  .unread-content {
    font-size: 0.7rem;
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    display: inline-block;
    width: 120px;
  }
  .content {
    font-size: medium;
    color: white;
  }

  .unread-content {
    font-weight: bolder;
    color: var(--secondary-color);
  }

  .time {
    width: 100%;
    font-size: smaller;
    align-self: flex-end;
    font-size: 0.6rem;
  }
  .displayName {
    color: white;
    font-size: large;
    font-size: 0.8rem;
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    color: var(--tertiary-color);
    text-decoration: underline;
  }

  .unread {
    font-weight: bold;
  }
}

.profile-picture {
  height: 28px;
  width: 28px;
  border-radius: 50%;
}
</style>
