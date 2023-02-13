export class GetBlabs {
  pageSize: number;
  pageNumber: number;

  /**
   *
   */
  constructor(pageNumber: number = 0, pageSize: number = 20) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
  }
}
