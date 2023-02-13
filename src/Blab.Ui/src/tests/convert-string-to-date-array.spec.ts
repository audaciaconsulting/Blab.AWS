import { convertStringToDateArray } from "@/helpers/convert-string-to-date-array.helper";
import { expect, test } from "vitest";
import type { IPost } from "@/models/posts/post.interface";
const blabsTest: IPost[] = [
  {
    displayName: "sam",
    id: 1,
    content: "This is content",
    dateCreated: "2022-11-07T11:51:32.2535373",
    handle: "sam",
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
  {
    displayName: "sam",
    id: 2,
    content: "This is content",
    dateCreated: "2022-11-07T11:51:32.2535373",
    handle: "sam",
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
  {
    displayName: "sam",
    id: 3,
    content: "This is content",
    dateCreated: "2022-11-07T11:51:32.2535373",
    handle: "sam",
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
];
const blabsTestInvalid: IPost[] = [
  {
    displayName: "sam",
    id: 1,
    content: "This is content",
    dateCreated: "invalid",
    handle: "sam",
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
  {
    displayName: "sam",
    id: 2,
    content: "This is content",
    dateCreated: "invalid",
    handle: "sam",
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
  {
    displayName: "sam",
    id: 3,
    content: "This is content",
    dateCreated: "invalid",
    handle: "sam",
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
];

test("should return array with date types inside the array", () => {
  expect(convertStringToDateArray(blabsTest, "dateCreated")).toEqual([
    {
      displayName: "sam",
      id: 1,
      content: "This is content",
      dateCreated: new Date("2022-11-07T11:51:32.2535373"),
      handle: "sam",
      userId: 1,
      reaction: null,
      profilePhotoBlob: "",
    },
    {
      displayName: "sam",
      id: 2,
      content: "This is content",
      dateCreated: new Date("2022-11-07T11:51:32.2535373"),
      handle: "sam",
      userId: 1,
      reaction: null,
      profilePhotoBlob: "",
    },
    {
      displayName: "sam",
      id: 3,
      content: "This is content",
      dateCreated: new Date("2022-11-07T11:51:32.2535373"),
      handle: "sam",
      userId: 1,
      reaction: null,
      profilePhotoBlob: "",
    },
  ]);
});

test("should return array thats empty because the date was invalid", () => {
  expect(convertStringToDateArray(blabsTestInvalid, "dateCreated")).toEqual([]);
});
