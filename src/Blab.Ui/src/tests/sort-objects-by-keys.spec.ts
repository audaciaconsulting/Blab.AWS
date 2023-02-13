// THESE TEST WILL TEST ALL OF THE NEW HELPER ARRAYS AS THIS FUNCTION USES THEM
import type { ISearchUser } from "@/models/search/search-user.interface";
import { expect, test } from "vitest";
import { sortArrayByKeys } from "@/helpers/sort-objects-by-keys.helper";
const correctUsers: ISearchUser[] = [
  {
    displayName: "samz",
    userId: 2,
    handle: "sam markey",
  },
  {
    displayName: "samb",
    userId: 3,
    handle: "zam",
  },
  {
    displayName: "sam",
    userId: 1,
    handle: "sam",
  },
];
const IncorrectUsersArray: ISearchUser[] = [];
//this should return the correct order format
test("Array should return correct order format", () => {
  expect(sortArrayByKeys(correctUsers, "handle", "displayName", "sam")).toEqual(
    [
      {
        displayName: "sam",
        userId: 1,
        handle: "sam",
      },
      {
        displayName: "samb",
        userId: 3,
        handle: "zam",
      },
      {
        displayName: "samz",
        userId: 2,
        handle: "sam markey",
      },
    ]
  );
});

//this test should return an empty array
test("Should return an empty array because it is invalid", () => {
  expect(
    sortArrayByKeys(IncorrectUsersArray, "handle", "displayName", "sam")
  ).toEqual([]);
});
// should return an empty array because the key values are invalid
test("Should return an empty array because the keys do not exist on the array", () => {
  expect(
    sortArrayByKeys(IncorrectUsersArray, "wrongKey", "wrong", "sam")
  ).toEqual([]);
});
