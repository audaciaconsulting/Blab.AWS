import { removeDuplicatesFromObjectArray } from "@/helpers/remove-duplicates-objects-from-array.helper";
import { filterArrayBySearchTerm } from "@/helpers/filter-array-with-search.helper";
import { filterBySearchTermsIncludedInArray } from "@/helpers/sort-array-by-included-search-term.helper";
import { removeItemsWithSearchTerm } from "@/helpers/remove-objects-from-array-by-search.helper";
import { sortArrayAlphabetically } from "@/helpers/sort-object-array-alphabetically.helper";

export function sortArrayByKeys(
  arr: any[],
  primaryKey: string,
  secondaryKey: string,
  searchTerm: string
): any[] {
  if (arr.length > 0) {
    //getting the exact matches for the primary key and ordering them alphbetically by their secondary key
    const exactMatch = filterArrayBySearchTerm(arr, primaryKey, searchTerm);

    //removing exact matches from array as they do not need sorting
    const removedExactMatch: any[] = removeItemsWithSearchTerm(
      arr,
      primaryKey,
      searchTerm
    );

    const filteredPrimaryKeys = filterBySearchTermsIncludedInArray(
      removedExactMatch,
      searchTerm,
      primaryKey
    );

    const filteredSecondaryKeys = filterBySearchTermsIncludedInArray(
      removedExactMatch,
      searchTerm,
      secondaryKey
    );

    // removes all the duplicates from both primary and secondary key arrays and sorts them by the secondary key
    const removeAllDuplicates = sortArrayAlphabetically(
      removeDuplicatesFromObjectArray([
        ...filteredPrimaryKeys,
        ...filteredSecondaryKeys,
      ]),
      secondaryKey
    );
    //destructures the exact matches and destructues remove all Duplicates array which combines them together to make one array but adds the exact matches first
    return removeDuplicatesFromObjectArray([
      ...exactMatch,
      ...removeAllDuplicates,
    ]);
  }
  return [];
}
