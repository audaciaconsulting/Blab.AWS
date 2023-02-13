// This array is to ensure that messages recieved from the API are ordered correctly by time.
export const sortByDateArray = <T>(arr: any[], key: string): T[] => {
  if (arr) {
    const msgArray = arr.sort((a, b) => {
      const sentDateA = new Date(a[key]);
      const sentDateB = new Date(b[key]);
      if (sentDateA instanceof Date && sentDateB instanceof Date) {
        // This orders it from newest to oldest.
        //Since the messages container is column reverse, that will sort the messages by oldest to newest so the newest message is on the bottom of the screen.
        return sentDateB.getTime() - sentDateA.getTime();
      }
      return 0;
    });
    return msgArray;
  }
  return [];
};
