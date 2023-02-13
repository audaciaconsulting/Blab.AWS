export function filterArrayBySearchTerm(
  arr: any[],
  key: string,
  searchTerm: string
): any[] {
  // filters array by the search the search term
  return arr.filter((val) => {
    if (
      val[key] &&
      typeof val[key] === "string" &&
      val[key].toLowerCase() === searchTerm.toLowerCase()
    ) {
      return val;
    }
  });
}
