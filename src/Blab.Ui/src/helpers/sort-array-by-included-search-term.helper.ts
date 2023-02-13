export function filterBySearchTermsIncludedInArray(
  arr: any[],
  searchTerm: string,
  key: string
): any[] {
  const newArray: any[] = [];
  newArray.push(...filterArrayByIncludes(arr, key, searchTerm));
  return filterArrayByIncludes(arr, key, searchTerm);
}

export function filterArrayByIncludes(
  arr: any[],
  key: string,
  searchTerm: string
) {
  return arr.filter((val) => {
    if (
      val[key] &&
      typeof val[key] === "string" &&
      val[key].toLowerCase().includes(searchTerm.toLowerCase())
    ) {
      return val;
    }
  });
}
