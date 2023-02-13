export class CommandResult<T> {
  output!: T;
  isSuccess!: string | boolean;
  errors!: any[];
}
