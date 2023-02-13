export function sortArrayAlphabetically<T, K extends keyof T>(
  values: T[],
  orderType: K
) {
  //sorts object array alphabetically and enables strict type checking
  return values.sort((a, b) => {
    if (a[orderType] && b[orderType]) {
      // checks if both are truthy
      return a[orderType] < b[orderType] ? -1 : 1;
    }

    if (a[orderType]) {
      return -1;
    }

    if (b[orderType]) {
      return 1;
    }

    return 0;
  });
}
