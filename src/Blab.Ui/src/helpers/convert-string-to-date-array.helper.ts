/* This helper function will convert an array with object 
and if the object key can be a validate date if converted 
from a string it will return a new array with date types instead of strings */
export function convertStringToDateArray(arr: any[], key: string): Array<any> {
  const newArray: any[] = arr.map((obj) => {
    if (!isNaN(new Date(obj[key]).getDate())) {
      return { ...obj, [key]: new Date(obj[key]) };
    } else {
      return false;
    }
  });
  if (newArray[0] === false) {
    return [];
  }
  return newArray;
}
