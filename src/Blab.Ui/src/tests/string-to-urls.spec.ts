import { stringToUrls } from "@/helpers/string-to-urls.helper";
import { expect, test } from "vitest";

test("should return an a tag linked to google and the value should be google.com", () => {
  const textToLink: string = stringToUrls("google.com test");
  expect(textToLink).toBe(`google.com test`);
});
test("should return 'test invalid URL' because no urls were valid", () => {
  const textToLink: string = stringToUrls("test invalid URL");
  expect(textToLink).toBe("test invalid URL");
});
