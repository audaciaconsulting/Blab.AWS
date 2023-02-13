export function timeLastMessageSent(time: Date | null): string {
  if (time !== null) {
    const timeAgoInMilliseconds = Date.now().valueOf() - time.valueOf();
    const minuteInMilliseconds = 60000;
    const hourInMilliseconds = 3600000;
    const dayInSeconds = 86400000;
    // the time now minus the time the last message sent.
    // if this is less than one hour ago (3600000 milliseconds) than show time ago in minutes
    if (timeAgoInMilliseconds < hourInMilliseconds) {
      const timeAgoInMins = Math.floor(
        timeAgoInMilliseconds / minuteInMilliseconds
      );
      return `${timeAgoInMins}m`;
    }
    //if the time was sent more than an hour ago but less than a day ago
    else if (
      timeAgoInMilliseconds > hourInMilliseconds &&
      timeAgoInMilliseconds < dayInSeconds
    ) {
      const timeAgoInHours = Math.floor(
        timeAgoInMilliseconds / hourInMilliseconds
      );
      return `${timeAgoInHours}h`;

      // return how many days ago the message was sent
    } else {
      //else the time should be shown in days
      const timeAgoInDays = Math.floor(timeAgoInMilliseconds / dayInSeconds);

      return `${timeAgoInDays} days`;
    }
  }
  // else there is no last message so we can say that the chat is new
  return "New Chat";
}
