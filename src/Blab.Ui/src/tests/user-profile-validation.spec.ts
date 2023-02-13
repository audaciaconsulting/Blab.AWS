import { expect, test } from "vitest";
import { validateStringInput } from "@/helpers/validate-string-input.helper";

test("should return an error due to too short handle", () => {
  const handleMinLength: string = validateStringInput("i", 2, 64, "handle");
  expect(handleMinLength).toBe("handle needs to be ateast 2 characters long");
});

test("should return an error due to long handle", () => {
  const handleMinLength: string = validateStringInput(
    "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
    2,
    64,
    "handle"
  );

  expect(handleMinLength).toBe("handle is over 64 characters please shorten");
});
test("should return an error due to handle containing spaces", () => {
  const handleMinLength: string = validateStringInput(
    "test handle",
    2,
    64,
    "handle"
  );
  expect(handleMinLength).toBe("handle cannot contain spaces");
});

test("should return an error due to too short display name", () => {
  const displayNameMinLength: string = validateStringInput(
    "t",
    2,
    128,
    "display name"
  );
  expect(displayNameMinLength).toBe(
    "display name needs to be ateast 2 characters long"
  );
});

test("should return an error due to long display name", () => {
  const displayNameMaxLength: string = validateStringInput(
    "toofidjgusa;lggggggllllllllklll;llllllll;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;l;",
    2,
    128,
    "display name"
  );
  expect(displayNameMaxLength).toBe(
    "display name is over 128 characters please shorten"
  );
});

test("should return an error due length is just empty spaces", () => {
  const displayNameLength: string = validateStringInput(
    "     ",
    2,
    128,
    "display name"
  );
  expect(displayNameLength).toBe(
    "display name needs to be ateast 2 characters long"
  );
});
