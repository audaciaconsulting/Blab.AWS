import { expect, test } from "vitest";
import { timeLastMessageSent } from "@/helpers/last-message-time-helper";

test("If message was sent less than an hour ago, the mins since the last message is returned.", () => {
  //Arrange
  const now = Date.now();
  const milliseconds = 3 * 60000;

  //Act
  const messageSent = new Date(now - milliseconds);

  //Assert
  expect(timeLastMessageSent(messageSent)).toEqual("3m");
});

test("If message was sent less than an day ago, the hours since the last message is returned.", () => {
  //Arrange
  const now = Date.now();
  const milliseconds = 3 * 60000 * 60;

  //Act
  const messageSent = new Date(now - milliseconds);

  //Assert
  expect(timeLastMessageSent(messageSent)).toEqual("3h");
});

test("If message was sent more than a day ago, the days since the last message is returned.", () => {
  //Arrange
  const now = Date.now();
  const milliseconds = 3 * 60000 * 60 * 24;

  //Act
  const messageSent = new Date(now - milliseconds);

  //Assert
  expect(timeLastMessageSent(messageSent)).toEqual("3 days");
});
