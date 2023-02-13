export function removeDuplicatesFromObjectArray(arr: any[]): any[] {
  // this removes duplicates from an arrayObject
  return [...new Map(arr.map((item) => [item, item])).values()];
}
