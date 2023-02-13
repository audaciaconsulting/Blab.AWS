export const sortByDateArrayProp = <T>(arr: any[], key: string): T[] => {
  if (arr) {
    const newArray = arr.sort((a, b) => {
      const dateA = new Date(a[key]);
      const dateB = new Date(b[key]);
      if (dateA instanceof Date && dateB instanceof Date) {
        return dateA.getTime() - dateB.getTime();
      }
      return 0;
    });
    return newArray.reverse();
  }
  return [];
};
