<template>
  <div class="search-component-container">
    <!-- This the is the parent component of the search functionality -->
    <!-- This is the input bar at the top of the website -->
    <SearchInput
      :isLoadingMoreUsers="!allDataLoaded"
      @updateSearchText="searchForUsers"
      @closeSearchContainer="resetSearch"
    />
    <!-- This is the container which contains the users returned and will only show if the searchText is truthy  -->
    <SearchUserContainer
      v-if="searchText"
      :users="users"
      :loadingUsers="loadingUsers"
      :are-more-users="!allDataLoaded"
      :message="displayMessage"
      :loading-more-users="loadingUsers"
      @load-more-users="loadUsers"
      @closeSearchContainer="resetSearch"
    />
  </div>
</template>

<script setup lang="ts">
import SearchInput from "@/components/search/SearchInput.vue";
import SearchUserContainer from "@/components/search/user-container/SearchUserContainer.vue";
import { sortArrayByKeys } from "@/helpers/sort-objects-by-keys.helper";
import type { IPagingResponse } from "@/models/paging/paging-request-response.interface";
import type { ISearchUser } from "@/models/search/search-user.interface";
import { UserSearch } from "@/models/search/search-user.model";
import { UserProfileService } from "@/services/user-profile.service";
import { StatusCodes } from "http-status-codes";
import { type Ref, ref, computed } from "vue";

const searchText: Ref<string> = ref("");
const users: Ref<ISearchUser[]> = ref([]);
let pageSize: number = 10;
let pageNumber: number = 0;
const model: UserSearch = new UserSearch();
const userProfileService: UserProfileService = new UserProfileService();
const displayMessage: Ref<string> = ref("");
const loadingUsers: Ref<boolean> = ref(false);
let page: Ref<IPagingResponse<ISearchUser> | null> = ref(null);

const allDataLoaded = computed(() => {
  return users.value.length === page.value?.totalRecords;
});
function searchForUsers(updatedSearchString: string): void {
  //once the search input has a length of 2 strings and the debounce has not been cleared this function will run
  if (updatedSearchString.length >= 2) {
    loadingUsers.value = true;
    displayMessage.value = "";
    searchText.value = updatedSearchString;
    pageNumber = 0;
    search();
  } else {
    // if the user has only inputted one string length this will reset the search state
    resetSearch();
  }
}
async function loadUsers(): Promise<void> {
  pageNumber++;
  search(true);
}
function resetSearch(): void {
  //resets the search back to its default state e.g. if the user clicks on a user, or the user deletes the input text or focuses out of the search
  users.value = [];
  searchText.value = "";
  pageNumber = 0;
  page.value = null;
}

async function search(loadingMoreUsers: boolean = false): Promise<void> {
  // setting the model to the variables;
  model.pageNumber = pageNumber;
  model.pageSize = pageSize;
  model.searchTerm = searchText.value;
  loadingUsers.value = true;
  // getting the users by passing the model in to the post request body
  const searchRes = await userProfileService.searchForUsers(model);

  if (searchRes.statusCode === StatusCodes.OK) {
    //once the response has been collected the loadingUsers will be false
    loadingUsers.value = false;
    // if the response status code is successful it will create a variable and add the response data to that
    if (searchRes.data.output.data) {
      const responseOfUsers: IPagingResponse<ISearchUser> =
        searchRes.data.output;

      if (responseOfUsers.data !== null) {
        // this is add the users array to a new variable so it can be sorted if data is inside the responded array
        const responseArray = sortArrayByKeys(
          responseOfUsers.data,
          "handle",
          "displayName",
          searchText.value
        );
        //Add the new users to the array if valid
        if (loadingMoreUsers) {
          users.value.push(...responseArray);
        } else {
          users.value = responseArray;
        }

        page.value = responseOfUsers;
      }
    }
  } else {
    //if the response is not successful a message will be displayed to the user
    displayMessage.value = "Something went wrong please try again later....";
  }
}
</script>

<style scoped lang="scss">
.search-component-container {
  height: 15vh;
  position: relative;
  display: flex;
  justify-content: flex-start;
  width: 100%;
  flex-direction: column;
  align-items: center;
}
</style>
