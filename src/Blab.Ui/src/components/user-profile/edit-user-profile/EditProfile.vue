<template>
  <transition>
    <!-- This is the parent/container for the edit user profile section -->
    <div v-if="props.profileEditState">
      <div @click="changeEditState" class="blur-background"></div>
      <div class="edit-profile-container">
        <!-- This contains the header and close icon -->
        <EditProfileHeader @toggleEditState="changeEditState" />
        <!-- This contains the form and submit button -->
        <EditProfileForm
          @change-user-data="changeUserData"
          @toggleEditState="changeEditState"
        />
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import EditProfileHeader from "./EditProfileHeader.vue";
import EditProfileForm from "./EditProfileForm.vue";

const props = defineProps({
  profileEditState: { required: true, type: Boolean },
});
const emit = defineEmits<{
  (e: "toggleEditState"): void;
  (e: "changeUserData"): void;
}>();
function changeEditState(): void {
  emit("toggleEditState");
}
function changeUserData(): void {
  emit("changeUserData");
}
</script>

<style scoped lang="scss">
.edit-profile-container {
  top: calc(50% - 50vmin);
  left: calc(50% - 50vmin);
  width: 100vmin;
  max-height: 100vmin;
  overflow-y: auto;
  display: flex;
  justify-content: flex-start;
  flex-direction: column;
  align-items: center;
  background-color: var(--background-color);
  padding: 1rem;
  position: fixed;
  gap: 20px;
  z-index: 100;
  border-radius: var(--border-radius-amount);
}
</style>
