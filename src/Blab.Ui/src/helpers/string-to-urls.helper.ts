export function stringToUrls(text: string): string {
  const normalUrl: RegExp = /(https?:\/\/[^\s]+)/g;
  const searchHTMlTag: RegExp = /(<|>)/;
  let editedString: string = "";
  const searchForHTMlTags: string = text.split(" ").join("");
  if (!searchForHTMlTags.match(searchHTMlTag)) {
    const splitStringArray: string[] = text.split(" ");
    const convertedArray: string[] = splitStringArray.map((url) => {
      if (url.match(normalUrl)) {
        const urlText = url.replace("https://", "").replace("http://", "");
        return `<a href="${url}" target="_blank" >${urlText}</a>`;
      }
      return url;
    });
    editedString = convertedArray.join(" ");
  }
  return editedString;
}
