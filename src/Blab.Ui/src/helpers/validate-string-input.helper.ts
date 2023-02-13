export function validateStringInput(
  validateThis: string,
  min: number,
  max: number,
  inputName: string
): string {
  const isEmpty = validateMinMaxLegth(validateThis, min, max, inputName);
  if (isEmpty !== "") {
    return isEmpty;
  } else if (validateThis.includes(" ") && inputName === "handle") {
    return `${inputName} cannot contain spaces`;
  }
  return "";
}
export function validateMinMaxLegth(
  validateThis: string,
  min: number,
  max: number,
  inputName: string
): string {
  validateThis = validateThis.trim();
  if (validateThis.length < min) {
    return `${inputName} needs to be ateast ${min} characters long`;
  } else if (validateThis.length > max) {
    return `${inputName} is over ${max} characters please shorten`;
  }

  return "";
}
