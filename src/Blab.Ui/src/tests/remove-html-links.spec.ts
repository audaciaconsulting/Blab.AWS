import { removeHtmlLinks } from "@/helpers/remove-html-links.helper";
import { expect, test } from "vitest";

test("should return `this is a button`", () => {
  const HtmlToText: string = removeHtmlLinks("<a>this is a button</a>");
  expect(HtmlToText).toBe("this is a button");
});

test("should return `test`", () => {
  const HtmlToText: string = removeHtmlLinks(
    '<img src="test" alt="test alt"><p>test</p></img>'
  );
  expect(HtmlToText).toBe("test");
});
