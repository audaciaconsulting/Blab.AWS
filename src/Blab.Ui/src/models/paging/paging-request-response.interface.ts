export interface IPagingResponse<T> {
  totalPages: number;
  totalRecords: number;
  data: Array<T>;
}
