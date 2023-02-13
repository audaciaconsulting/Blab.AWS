<template>
  <div class="post-content-container">
    <div
      class="profile-header"
      @click="router.push({ name: 'UserProfile', params: { handle } })"
    >
      <img
        data-test="user-profile-button"
        :src="imageSrc"
        alt="profile picture"
      />
      <div class="handle-display-container">
        <h2>{{ props.post.displayName }}</h2>
        <span>@{{ props.post.handle }}</span>
      </div>
    </div>
    <RouterLink
      v-if="props.post.id"
      :to="{ name: 'SinglePost', params: { id: props.post.id } }"
    >
      <p>
        {{ props.post.content }}
      </p>
      <span>{{ dayjs(date).format("HH:mm MMM DD, YYYY") }}</span>
    </RouterLink>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, type PropType, type Ref } from "vue";
import dayjs from "dayjs";
import type { IPost } from "@/models/posts/post.interface";
import { RouterLink, useRouter } from "vue-router";
import { profilePhotoSelector } from "@/helpers/profile-photo-selector";
const props = defineProps({
  post: { required: true, type: Object as PropType<IPost> },
});

const imageSrc = computed(() => {
  return profilePhotoSelector(props.post.profilePhotoBlob);
});

const handle: Ref<string> = ref(props.post.handle);
const date = props.post.dateCreated as Date;
const router = useRouter();
</script>
<style scoped lang="scss">
.post-content-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
  width: 100%;
  cursor: pointer;

  span {
    margin-bottom: calc(10px);
  }
}
.handle-display-container {
  width: 85%;
  display: flex;
  flex-direction: column;
  h2,
  span {
    width: 100%;
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
  }
  span {
    display: flex;
    color: rgba($color: #ffffff, $alpha: 0.75);
    flex-direction: column;
    border-radius: 20%;
    font-size: 1.3rem;
    font-weight: 600;
  }
}
.profile-header {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  gap: 20px;
  img {
    width: 60px;
    height: 60px;
    border-radius: 50%;
  }
}
</style>
