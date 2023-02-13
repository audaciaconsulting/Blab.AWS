import { convertStringToDateArray } from "@/helpers/convert-string-to-date-array.helper";
import { UserProfileService } from "@/services/user-profile.service";
import type { IUserProfileDetails } from "@/models/user-profile/user-profile-details.interface";
import { defineStore } from "pinia";
import { AuthService } from "@/services/auth.service";
import type { User } from "oidc-client-ts";
import type { ApiResponse } from "@/models/api/api-response.model";

export const useProfileStore = defineStore("profile", {
  state: () => ({
    userDetails: {} as IUserProfileDetails,
    currentUser: null as User | null,
  }),
  getters: {},
  actions: {
    async getUser() {
      const auth = new AuthService();
      const user = await auth.getUser();
      if (user) {
        this.currentUser = user;
      }
    },
    async getUserDetails() {
      await this.getUser();
      if (this.currentUser) {
        const loadDetails: UserProfileService = new UserProfileService();
        const id: number = +this.currentUser.profile.sub;
        loadDetails.getUserProfile(id).then((profileDetails: ApiResponse) => {
          this.userDetails = {
            displayName: profileDetails.data.displayName,
            bio: profileDetails.data.bio,
            handle: profileDetails.data.handle,
            userId: id,
            blabs: convertStringToDateArray(
              profileDetails.data.blabs,
              "dateCreated"
            ),
            profilePhoto: profileDetails.data.profilePhoto,
            backgroundPhoto: profileDetails.data.backgroundPhoto,
          };
        });
      }
    },
  },
});
