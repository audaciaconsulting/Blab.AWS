export function removeHtmlLinks(text: string): string {
  const el: HTMLDivElement = document.createElement("div");

  el.innerHTML = text;
  return el.textContent ? el.textContent : "";
}
