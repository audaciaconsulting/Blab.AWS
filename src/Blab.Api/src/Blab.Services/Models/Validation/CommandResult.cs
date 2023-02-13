using Audacia.Commands;

namespace Blab.Services.Models.Validation;

/// <summary>
    /// Represents the result of handling an <see cref="ICommand"/>.
    /// If the result represents a failure then a collection of errors will contain the details.
    /// </summary>
    public class CommandResult : IResult
    {
        private readonly List<IError> _errors;

        /// <summary>
        /// Gets a flag indicating whether the handling of the <see cref="ICommand"/> succeeded (true) or failed (false).
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Gets a collection of errors; this collection should only be populated if the result represents a failure.
        /// </summary>
        public IEnumerable<IError> Errors => _errors;

        public CommandResult(ICollection<IError> errors)
        {
            IsSuccess = errors.Count == 0;
            _errors = new List<IError>(errors);
        }

        public CommandResult(bool isSuccess, params IError[] errors)
        {
            IsSuccess = isSuccess;
            _errors = new List<IError>(errors);
        }

        public CommandResult(bool isSuccess, IEnumerable<IError> errors)
        {
            IsSuccess = isSuccess;
            _errors = new List<IError>(errors);
        }

        /// <summary>
        /// Creates a <see cref="CommandResult"/> object representing the successful handling of an <see cref="ICommand"/>.
        /// </summary>
        /// <returns></returns>
        public static CommandResult Success()
        {
            return new CommandResult(true);
        }

        /// <summary>
        /// Creates a <see cref="TOutput"/> object representing the failed handling of an <see cref="ICommand"/>.
        /// The error details are contained in the <paramref name="errors"/> collection.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static CommandResult<TOutput> Failure<TOutput>(params IError[] errors)
        {
            return new CommandResult<TOutput>(false, errors);
        }

        /// <summary>
        /// Creates a <see cref="TOutput"/> object representing the failed handling of an <see cref="ICommand"/>.
        /// The error details are contained in the <paramref name="errors"/> collection.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static CommandResult<TOutput> Failure<TOutput>(ICollection<IError> errors)
        {
            return new CommandResult<TOutput>(errors);
        }

        /// <summary>
        /// Creates a <see cref="CommandResult{TOutput}"/> object from the given <paramref name="result"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CommandResult<TOutput> FromExistingResult<TOutput>(IResult result)
        {
            return new CommandResult<TOutput>(result.IsSuccess, result.Errors.ToArray());
        }

        /// <summary>
        /// Creates a <see cref="CommandResult{TOutput}"/> object from the given <paramref name="result"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CommandResult<TOutput> FromExistingResult<TOutput>(IResult<TOutput> result)
        {
            return new CommandResult<TOutput>(result);
        }

        /// <summary>
        /// Creates a <see cref="CommandResult"/> object representing the failed handling of an <see cref="ICommand"/>.
        /// The error details are contained in the <paramref name="errors"/> collection.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static CommandResult Failure(params IError[] errors)
        {
            return new CommandResult(false, errors);
        }

        /// <summary>
        /// Creates a <see cref="CommandResult{T}"/> object representing the successful handling of an <see cref="ICommand"/>,
        /// with the output of execution given by the <paramref name="output"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="output"></param>
        /// <returns></returns>
        public static CommandResult<T> WithResult<T>(T output)
        {
            return new CommandResult<T>(output);
        }

        /// <summary>
        /// Adds the given <paramref name="error"/> to the <see cref="CommandResult"/>'s <see cref="Errors"/> property.
        /// </summary>
        /// <param name="error"></param>
        public void AddError(IError error)
        {
            _errors.Add(error);
            IsSuccess = false;
        }

        /// <summary>
        /// Adds the given <paramref name="errors"/> to the <see cref="CommandResult"/>'s <see cref="Errors"/> property.
        /// </summary>
        /// <param name="errors"></param>
        public void AddErrors(params IError[] errors)
        {
            _errors.AddRange(errors);
            IsSuccess = !_errors.Any();
        }

        /// <summary>
        /// Adds the errors from the given <paramref name="commandResult"/> to the <see cref="CommandResult"/>'s <see cref="Errors"/> property.
        /// </summary>
        /// <param name="commandResult"></param>
        public void AddErrorsFrom(CommandResult commandResult)
        {
            _errors.AddRange(commandResult.Errors);
            IsSuccess = !_errors.Any();
        }
    }

    /// <summary>
    /// Represents the result of handling an <see cref="ICommand"/> where the handling requires an output (of type <see cref="T"/>) to be returned to the client.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandResult<T> : CommandResult, IResult<T>
    {
        public T Output { get; }

        public CommandResult(bool isSuccess, params IError[] errors)
            : base(isSuccess, errors)
        {
        }

        public CommandResult(ICollection<IError> errors)
            : base(errors)
        {
        }

        public CommandResult(T output, IResult result)
            : base(result.IsSuccess, result.Errors)
        {
            Output = output;
        }

        public CommandResult(T output)
            : base(true)
        {
            Output = output;
        }

        public CommandResult(IResult<T> result)
            : base(result.IsSuccess, result.Errors)
        {
            Output = result.Output;
        }
    }
