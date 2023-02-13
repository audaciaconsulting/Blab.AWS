import type { CommandResult } from "./api-response-command-result.model";

export class ApiResponseWithType<T> {
  data!: CommandResult<T>;
  statusCode!: number;
  constructor(statusCode: number) {
    this.statusCode = statusCode;
  }
}
