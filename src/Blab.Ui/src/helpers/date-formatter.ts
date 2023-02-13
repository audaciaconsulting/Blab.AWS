export default function formatStringDate(inputDate: Date | string): string {
  const date = new Date(inputDate);
  const dateParts = date.toDateString().split(" ");
  const dateString = dateParts[1] + " " + dateParts[2] + " " + dateParts[3];
  const formattedDate =
    date.getHours() + ":" + date.getMinutes() + " " + dateString;
  return formattedDate;
}
