<template>
  <div class="input-container">
    <div>
      <div @mouseover="hover = true" @mouseleave="hover = false">
        <img v-if="!hover" :src="imageSrc" class="profile-image" />
        <input
          data-test="profile-image-input"
          id="input-profile-image"
          type="file"
          accept="image/jpg, image/png, image/jpeg"
          @change="handleNewProfileImage()"
          hidden
        />
        <img
          v-if="hover"
          src="@/assets/icons/plus-icon.svg"
          class="profile-image"
          @click="chooseProfileImage"
        />
      </div>
    </div>
  </div>
  <div class="input-container">
    <div
      class="background-image"
      @mouseover="hoverBackground = true"
      @mouseleave="hoverBackground = false"
    >
      <img
        v-if="!hoverBackground"
        :src="backgroundSrc"
        class="background-image"
      />
      <input
        data-test="background-image-input"
        id="input-background-image"
        type="file"
        accept="image/jpg, image/png, image/jpeg"
        @change="handleNewBackgroundImage()"
        hidden
      />
      <img
        v-if="hoverBackground"
        src="@/assets/icons/plus-icon.svg"
        class="background-image"
        @click="chooseBackgroundImage"
      />
    </div>
    <span class="error">{{ imageError }}</span>
  </div>

  <!-- I used v model on all the text/ textarea tags so it would create 2 way binding with the model class I created down below -->
  <div class="input-container">
    <img src="@/assets/icons/profile.svg" alt="user profile icon" />
    <div class="input-error-container">
      <input
        data-test="profile-handle-input"
        type="text"
        placeholder="Edit your handle here..."
        v-model.trim="model.handle"
      />
      <!-- This is where the handle input error is displayed if validation requirements are not met -->
      <span class="error">{{ handleError }}</span>
    </div>
  </div>
  <div class="input-container">
    <img src="@/assets/icons/profile.svg" alt="user profile icon" />
    <div class="input-error-container">
      <input
        data-test="profile-display-name-input"
        type="text"
        placeholder="Edit your display name here..."
        v-model.trim="model.displayName"
      />
      <span class="error">{{ displayNameError }}</span>
    </div>
  </div>
  <div class="input-container">
    <img src="@/assets/icons/text.svg" alt=" bio icon" />
    <div class="input-error-container">
      <textarea
        data-test="profile-bio-input"
        v-model.trim="model.bio"
        placeholder="Edit your bio here..."
        rows="4"
        type="text"
      /><span class="error">{{ bioError }}</span>
    </div>
  </div>
  <div class="input-container">
    <input
      v-if="!isUpdatingProfile"
      @click="updateProfile"
      type="button"
      value="Update"
      data-test="update-profile-save"
    />
    <!-- If the the user sent new profile data to the api it will load the spinner and once it has loaded it will stop  -->
    <SpinnerLoader v-else />
    <!-- spinner is reusable throughout the system -->
  </div>
  <!-- This is where an error will be displayed to the user if there is one -->
  <span class="error">{{ apiError }}</span>
</template>

<script setup lang="ts">
import SpinnerLoader from "@/components/spinners/SpinnerLoader.vue";
import { UpdateUserProfile } from "@/models/user-profile/update-user-profile.model";
import { validateStringInput } from "@/helpers/validate-string-input.helper";
import { UserProfileService } from "@/services/user-profile.service";
import { useProfileStore } from "@/stores/user-profile.store";
import { type Ref, ref } from "vue";
import { removeHtmlLinks } from "@/helpers/remove-html-links.helper";
import { stringToUrls } from "@/helpers/string-to-urls.helper";
import { useRouter, type Router } from "vue-router";
import { AddPhoto } from "@/models/photos/add-photo.model";
import { Photo } from "@/models/photos/photo.model";
import { profilePhotoSelector } from "@/helpers/profile-photo-selector";
import { ConvertToBaseSixtyFour } from "@/helpers/convert-file-to-base64.helper";
import { backgroundPhotoSelector } from "@/helpers/background-photo-selector.helper";

const userStore = useProfileStore();
const displayNameError: Ref<string> = ref("");
const handleError: Ref<string> = ref("");
const bioError: Ref<string> = ref("");
const imageError: Ref<string> = ref("");
const apiError: Ref<string> = ref("");
const isUpdatingProfile: Ref<boolean> = ref(false);
const model: Ref<UpdateUserProfile> = ref(new UpdateUserProfile());
const router: Router = useRouter();
const hover: Ref<boolean> = ref(false);
const hoverBackground: Ref<boolean> = ref(false);
// I created a class so I can pass a model to the api rather than form data due to good practice standards
model.value.bio = userStore.userDetails.bio;
model.value.handle = userStore.userDetails.handle;
model.value.displayName = userStore.userDetails.displayName;
model.value.userId = userStore.userDetails.userId;
model.value.profilePhoto = null;

const emit = defineEmits<{
  (e: "toggleEditState"): void;
  (e: "changeUserData"): void;
}>();

const imageSrc: Ref<string> = ref(
  profilePhotoSelector(userStore.userDetails.profilePhoto?.blobName)
);
const backgroundSrc: Ref<string> = ref(
  backgroundPhotoSelector(userStore.userDetails.backgroundPhoto?.blobName)
);

function chooseProfileImage(): void {
  const imageInput = document.getElementById("input-profile-image");
  if (imageInput != null) {
    imageInput.click();
  }
}
function chooseBackgroundImage(): void {
  const imageInput = document.getElementById("input-background-image");
  if (imageInput != null) {
    imageInput.click();
  }
}
function imageValidator(imageFile: File): void {
  // maximum image size in bytes.
  const maxImageSize: number = 5000000;
  if (!(imageFile.type === "image/jpeg" || imageFile.type === "image/png")) {
    imageError.value = "Image should be a jpg or png.";
  } else if (imageFile.size > maxImageSize) {
    imageError.value = "Image cannot be larger than 5MB.";
  } else if (imageFile.size === 0) {
    imageError.value = "Image cannot be empty.";
  }
}

async function handleNewProfileImage(): Promise<void> {
  imageError.value = "";
  const input = document.getElementById(
    "input-profile-image"
  ) as HTMLInputElement;
  if (input.files != null) {
    const imageFile = input.files[0];
    imageValidator(imageFile);
    const image = (await ConvertToBaseSixtyFour(imageFile)) as string;
    // Get Url to preview selected image;
    const imageURL = URL.createObjectURL(imageFile);
    imageSrc.value = imageURL;
    // create new AddPhoto request object.
    const newProfilePhoto = new AddPhoto(
      image.split(",")[1],
      imageFile.name,
      imageFile.type.split("/")[1],
      imageFile.size
    );
    model.value.profilePhoto = newProfilePhoto;
  }
}

async function handleNewBackgroundImage(): Promise<void> {
  imageError.value = "";
  const input = document.getElementById(
    "input-background-image"
  ) as HTMLInputElement;
  if (input.files != null) {
    const imageFile = input.files[0];
    imageValidator(imageFile);
    const image = (await ConvertToBaseSixtyFour(imageFile)) as string;
    const imageURL = URL.createObjectURL(imageFile);
    backgroundSrc.value = imageURL;
    const newBackgroundPhoto = new AddPhoto(
      image.split(",")[1],
      imageFile.name,
      imageFile.type.split("/")[1],
      imageFile.size
    );
    model.value.backgroundPhoto = newBackgroundPhoto;
  }
}

//once the data has been updating successfully it will close the update profile modal
async function updateProfile(): Promise<void> {
  if (model.value.bio) {
    model.value.bio = removeHtmlLinks(model.value.bio);
  }
  bioError.value = "";
  isUpdatingProfile.value = true;
  displayNameError.value = validateStringInput(
    model.value.displayName,
    2,
    128,
    "display name"
  );
  //the validateStringInput functions are REUSABLE throughout the system
  handleError.value = validateStringInput(model.value.handle, 2, 64, "handle");
  if (model.value.bio) {
    // If the bio contains html tags it will not let the user update the bio until they are removed
    if (stringToUrls(model.value.bio) === "") {
      bioError.value = "invalid characters please remove < and or >";
    }
  }

  if (
    !displayNameError.value &&
    !handleError.value &&
    !bioError.value &&
    !imageError.value
  ) {
    const updateProfile = await new UserProfileService().updateUserProfile(
      model.value
    );
    // If the api response is OK (200) it will save the most recent user profile data to the current userDetails on the store by replacing the old data

    if (updateProfile.statusCode === 200) {
      emit("toggleEditState");

      if (updateProfile.data.output) {
        const profilePhotoBlobName: string = updateProfile.data.output;
        var newProfilePhoto = new Photo();
        newProfilePhoto.blobName = profilePhotoBlobName;
      }
      userStore.$patch((state) => {
        state.userDetails = {
          userId: model.value.userId,
          displayName: model.value.displayName,
          handle: model.value.handle,
          bio: model.value.bio,
          blabs: state.userDetails.blabs,
          profilePhoto: newProfilePhoto,
          backgroundPhoto: newProfilePhoto,
        };
        state.userDetails.blabs.map(
          (blab) => (
            (blab.displayName = model.value.displayName),
            (blab.handle = model.value.handle)
          )
        );
      });

      // This router push will update the params on the UserProfileView page
      router.push({
        name: "UserProfile",
        params: { handle: userStore.userDetails.handle },
      });
      // displays the updated user details
      emit("changeUserData");
    } else {
      //If the api does not return 200 it will display this to the user
      apiError.value = "something went wrong please try again later...";
    }
  }
  isUpdatingProfile.value = false;
  // This stops the spinner from loading
}
</script>

<style scoped lang="scss">
.input-error-container {
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
}
.input-container {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;
  width: 80%;
  &:nth-child(6) {
    align-items: flex-start;
  }
  &:nth-child(7) {
    background-color: var(--secondary-color);
    border-radius: var(--border-radius-amount);
    width: 60%;
    transition: background-color 0.15s ease-in-out;
    &:hover {
      background-color: var(--icon-color);
    }
  }
}

textarea {
  min-height: 100px;
  max-height: 200px;
  flex-grow: 1;
  border-radius: var(--border-radius-amount);
  resize: vertical;
  border: var(--icon-color) solid 2px;
}

input[type="button"] {
  cursor: pointer;
  width: 100%;
  padding: 0.5rem;
  font-size: 2rem;
  font-weight: 700;
  outline: none;
  border: none;
  color: var(--background-color);
  background-color: transparent;
}

img {
  width: 25px;
}

.images-container {
  display: flex;
  gap: 50px;
}

.profile-image {
  margin: 0;
  width: 100px;
  height: 100px;
  border-radius: 50px;
}

.background-image {
  height: 100px;
  width: 200px;
}
</style>
