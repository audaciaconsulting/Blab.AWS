<template>
  <span :class="isSentByLoggedInUser">{{
    dayjs(message.sent).format("HH:mm:ss DD/MM/YY")
  }}</span>
  <span :class="isLoggedInUser">{{ message.content }} </span>
</template>

<script setup lang="ts">
import type { Message } from "@/models/messages/message.model";
import { useProfileStore } from "@/stores/user-profile.store";
import { computed, type PropType } from "vue";
import dayjs from "dayjs";

const userStore = useProfileStore();
const props = defineProps({
  message: { required: true, type: Object as PropType<Message> },
});

const loggedInUserId = userStore.userDetails.userId;
const isLoggedInUser = computed(() =>
  loggedInUserId !== undefined && +loggedInUserId !== props.message.userId
    ? "loggedInUser-messages message"
    : "otherUser-messages message"
);

const isSentByLoggedInUser = computed(() =>
  loggedInUserId !== undefined && +loggedInUserId !== props.message.userId
    ? "sentByLoggedInUser"
    : "sentByOtherUser"
);
</script>

<style scoped lang="scss">
.message {
  padding: 1rem;
  max-width: 50%;
  overflow-wrap: break-word;
}
.loggedInUser-messages {
  display: flex;
  border-radius: 2rem 2rem 2rem 0.25rem;
  background-color: var(--secondary-color);
  color: var(--background-color);
  margin-right: auto;
  align-self: flex-end;
  justify-self: flex-end;
  text-overflow: "";
}

.otherUser-messages {
  display: flex;
  border-radius: 2rem 2rem 0.25rem 2rem;
  background-color: var(--background-color);
  color: var(--secondary-color);
  margin-left: auto;
  align-self: flex-start;
  justify-self: flex-start;
  text-overflow: "";
}

.sentByLoggedInUser {
  display: flex;
  font-size: 10px;
  color: lightgray;
  margin-right: auto;
  align-self: flex-end;
  justify-self: flex-end;
}

.sentByOtherUser {
  display: flex;
  font-size: 10px;
  color: lightgray;
  margin-left: auto;
  align-self: flex-start;
  justify-self: flex-start;
}
</style>
