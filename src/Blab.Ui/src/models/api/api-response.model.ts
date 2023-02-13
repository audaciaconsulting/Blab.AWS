export class ApiResponse {
  data!: any;
  statusCode!: number;
  constructor(statusCode: number) {
    this.statusCode = statusCode;
  }
}
