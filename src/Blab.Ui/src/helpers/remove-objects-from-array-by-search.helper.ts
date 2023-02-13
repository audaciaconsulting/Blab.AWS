export function removeItemsWithSearchTerm(
  arr: any[],
  key: string,
  searchTerm: string
): any[] {
  //removes items from the array if searchTerm matches the given key
  return arr.filter((val) => {
    if (val[key] && typeof val[key] === "string" && val[key] !== searchTerm) {
      return val;
    }
  });
}
