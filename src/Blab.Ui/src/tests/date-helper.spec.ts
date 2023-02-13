import { sortByDateArrayProp } from "@/helpers/date.helper";
import { expect, test } from "vitest";
import type { IPost } from "@/models/posts/post.interface";

const userDetailsTest: IPost[] = [
  {
    displayName: "sam",
    id: 3,
    content: "content",
    handle: "handle",
    dateCreated: new Date("01-01-2001 00:03:44"),
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
  {
    displayName: "sam",
    id: 2,
    content: "content",
    handle: "handle",
    dateCreated: new Date("01-01-2003 00:03:44"),
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
  {
    displayName: "sam",
    id: 1,
    content: "content",
    handle: "handle",
    dateCreated: new Date("01-01-2004 00:03:44"),
    userId: 1,
    reaction: null,
    profilePhotoBlob: "",
  },
];

const userDetailsTestNoDate: any[] = [
  { displayName: "sam", content: "content", handle: "handle1" },
  { displayName: "sam", content: "content", handle: "handle2" },
  { displayName: "sam", content: "content", handle: "handle3" },
];

const userDetailsTestDiffKey: any[] = [
  {
    displayName: "sam",
    content: "content",
    handle: "handle1",
    cc: new Date("01-01-2002 00:03:44"),
  },
  {
    displayName: "sam",
    content: "content",
    handle: "handle2",
    cc: new Date("01-01-2003 00:03:44"),
  },
  {
    displayName: "sam",
    content: "content",
    handle: "handle3",
    cc: new Date("01-01-2001 00:03:44"),
  },
];
test(" should return array from newest to oldest by date", () => {
  expect(sortByDateArrayProp(userDetailsTest, "dateCreated")).toEqual([
    {
      displayName: "sam",
      id: 1,
      content: "content",
      handle: "handle",
      dateCreated: new Date("01-01-2004 00:03:44"),
      userId: 1,
      reaction: null,
      profilePhotoBlob: "",
    },
    {
      displayName: "sam",
      id: 2,
      content: "content",
      handle: "handle",
      dateCreated: new Date("01-01-2003 00:03:44"),
      userId: 1,
      reaction: null,
      profilePhotoBlob: "",
    },
    {
      displayName: "sam",
      id: 3,
      content: "content",
      handle: "handle",
      dateCreated: new Date("01-01-2001 00:03:44"),
      userId: 1,
      reaction: null,
      profilePhotoBlob: "",
    },
  ]);
});
test("should return the original array reversed", () => {
  expect(sortByDateArrayProp(userDetailsTestNoDate, "dateCreated")).toEqual([
    { displayName: "sam", content: "content", handle: "handle3" },
    { displayName: "sam", content: "content", handle: "handle2" },
    { displayName: "sam", content: "content", handle: "handle1" },
  ]);
});
test("should return array from newest to oldest by date even if date key is different", () => {
  expect(sortByDateArrayProp(userDetailsTestDiffKey, "cc")).toEqual([
    {
      displayName: "sam",
      content: "content",
      handle: "handle2",
      cc: new Date("01-01-2003 00:03:44"),
    },
    {
      displayName: "sam",
      content: "content",
      handle: "handle1",
      cc: new Date("01-01-2002 00:03:44"),
    },
    {
      displayName: "sam",
      content: "content",
      handle: "handle3",
      cc: new Date("01-01-2001 00:03:44"),
    },
  ]);
});
