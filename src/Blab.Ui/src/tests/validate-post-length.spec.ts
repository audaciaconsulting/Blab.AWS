import {
  maximumBlabLength,
  minimumBlabLength,
} from "@/helpers/validate-lengths.helper";
import { expect, test } from "vitest";

test(" should return false as the string is greater than 0 and less than 256 characters", () => {
  expect(
    maximumBlabLength("123dfgj6y54jh", 256) &&
      minimumBlabLength("123dfgj6y54jh", 1)
  ).toBeFalsy();
});
test("Should return true as the character length is less than 1", () => {
  expect(minimumBlabLength("", 1)).toBeTruthy();
});
test("Should return true as the characters are greater than 256", () => {
  expect(
    maximumBlabLength(
      "sdffffffffffffffffffff fffffdrgergdfgkghjdfjklghdkjghadjfkghuioer ahgdjakfghjadfhjgadhfgjdfhjgjahfgjkadfhgajdhfguaiehrgujkkhjgledhjgdfhjghdfgahdjfghjdfghajkdfhjg dfjhkgadhfghgfgfghjhfjgklsdffffffffffffffffffff fffffdrgergdfgkghjdfjklghdkjghadjfkghuioer ahgdjakfghjadfhjgadhfgjdfhjgjahfgjkadfhgajdhfguaiehrgujkkhjgledhjgdfhjghdfgahdjfghjdfghajkdfhjg dfjhkgadhfghgfgfghjhfjgkl",
      256
    )
  ).toBeTruthy();
});
