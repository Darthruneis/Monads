using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads
{
    /// <summary>
    /// Defines a result of an operation.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Indicates whether the operation was unsuccessful. Inverse of <see cref="IsSuccess"/>.
        /// </summary>
        public bool IsFailure => !IsSuccess;
        /// <summary>
        /// A general error message about the operation in the case of an unsuccessful operation.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Detailed error messages about the operation, generally intended to point to specific
        /// relevant keys about the operation (such as a property name that was invalid, for example).
        /// </summary>
        public Dictionary<string, List<string>> ErrorMessages { get; }

        /// <summary>
        /// Default constructor. Creates a success result.
        /// </summary>
        protected Result()
        {
            IsSuccess = true;
            ErrorMessages = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Constructor for failure results with an optional set of detailed error messages.
        /// </summary>
        /// <param name="error">The general error message to use for the result.</param>
        /// <param name="errorMessages">Any additional, detailed error messages to include.</param>
        protected Result(string error, params KeyValuePair<string, ICollection<string>>[] errorMessages)
        {
            IsSuccess = false;
            Error = error;
            ErrorMessages = errorMessages.ToDictionary(x => x.Key, x => x.Value.ToList());
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns></returns>
        public static Result Ok() => new Result();

        /// <summary>
        /// Creates an unsuccessful result with the specified general error message and an optional set of detailed error messages.
        /// </summary>
        /// <param name="error">The general error message to use for the result.</param>
        /// <param name="errorMessages">Any additional, detailed error messages to include.</param>
        /// <returns></returns>
        public static Result Fail(string error, params KeyValuePair<string, ICollection<string>>[] errorMessages)
        {
            if(error == null)
                throw new ArgumentNullException(nameof(error), "A failure result requires an error message.");

            if (string.IsNullOrWhiteSpace(error))
                throw new ArgumentException("A failure result requires an error message.", nameof(error));

            return new Result(error, errorMessages);
        }

        /// <summary>
        /// Allows a bool to be cast as a result with a catch-all error message for false.
        /// </summary>
        /// <param name="isSuccess"></param>
        public static implicit operator Result(bool isSuccess) => isSuccess ? Ok() : Fail("Operation was a failure.");

        /// <summary>
        /// Creates a successful result with a value.
        /// </summary>
        /// <typeparam name="T">The type of value contained in the result.</typeparam>
        /// <param name="value">The value of the result.</param>
        /// <returns></returns>
        public static Result<T> Ok<T>(T value) => Result<T>.Ok(value);

        /// <summary>
        /// Creates an unsuccessful result with the specified general error message and an optional set of detailed error messages.
        /// </summary>
        /// <typeparam name="T">The type of value contained in the result.</typeparam>
        /// <param name="error">The general error message to use for the result.</param>
        /// <param name="errorMessages">Any additional, detailed error messages to include.</param>
        /// <returns></returns>
        public static Result<T> Fail<T>(string error, params KeyValuePair<string, ICollection<string>>[] errorMessages) 
            => Result<T>.Fail(error, errorMessages);

        /// <summary>
        /// Creates a successful result with a success value.
        /// </summary>
        /// <typeparam name="TSuccess">The type of value contained in the result on a success.</typeparam>
        /// <typeparam name="TFailure">The type of value contained in the result on a failure.</typeparam>
        /// <param name="successValue">The value of the result.</param>
        /// <returns></returns>
        public static Result<TSuccess, TFailure> Ok<TSuccess, TFailure>(TSuccess successValue)
            => Result<TSuccess, TFailure>.Ok(successValue);

        /// <summary>
        /// Creates an unsuccessful result with a failure value and the specified general error message and an optional set of detailed error messages.
        /// </summary>
        /// <typeparam name="TSuccess">The type of value contained in the result on a success.</typeparam>
        /// <typeparam name="TFailure">The type of value contained in the result on a failure.</typeparam>
        /// <param name="error">The general error message to use for the result.</param>
        /// <param name="failureValue">The value of the result.</param>
        /// <param name="errorMessages">Any additional, detailed error messages to include.</param>
        /// <returns></returns>
        public static Result<TSuccess, TFailure> Fail<TSuccess, TFailure>(string error, TFailure failureValue, params KeyValuePair<string, ICollection<string>>[] errorMessages)
            => Result<TSuccess, TFailure>.Fail(error, failureValue, errorMessages);
    }

    /// <summary>
    /// Defines a result of an operation which also returns a value on success.
    /// </summary>
    /// <typeparam name="T">The type of value to include on a success.</typeparam>
    public class Result <T> : Result
    {
        private T _value;

        /// <summary>
        /// The value that was returned from the operation on a success.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the result is a failure.</exception>
        public T Value
        {
            get
            {
                if(IsFailure)
                    throw new InvalidOperationException("Result does not have a value.");

                return _value;
            }
            private set => _value = value;
        }

        /// <summary>
        /// Constructor for a failure result.
        /// </summary>
        /// <param name="error">The general error message to use for the result.</param>
        /// <param name="errorMessages">Any additional, detailed error messages to include.</param>
        protected Result(string error, params KeyValuePair<string, ICollection<string>>[] errorMessages) 
            : base(error, errorMessages) { }

        /// <summary>
        /// Constructor for a success result with the specified value.
        /// </summary>
        /// <param name="value">The value returned by the operation.</param>
        protected Result(T value) 
        {
            Value = value;
        }

        /// <summary>
        /// Creates a successful result with the provided value.
        /// </summary>
        /// <param name="value">The value returned by the operation.</param>
        /// <returns></returns>
        public static Result<T> Ok(T value) 
            => new Result<T>(value);

        /// <summary>
        /// Creates an unsuccessful result with the specified general error message and an optional set of detailed error messages.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="errorMessages"></param>
        /// <returns></returns>
        public new static Result<T> Fail(string error, params KeyValuePair<string, ICollection<string>>[] errorMessages) 
            => new Result<T>(error, errorMessages);

        /// <summary>
        /// Allows a value to be cast as a result with a catch-all error message for null.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        public static implicit operator Result<T>(T value) => value == null ? Fail("Operation was a failure.") : Ok(value);
    }

    /// <summary>
    /// A result of an operation which returns values both on success and on failure.
    /// </summary>
    /// <typeparam name="TSuccess">The type of value contained in the result on a success.</typeparam>
    /// <typeparam name="TFailure">The type of value contained in the result on a failure.</typeparam>
    public class Result <TSuccess, TFailure> : Result
    {
        private TSuccess _successValue;
        private TFailure _failureValue;

        /// <summary>
        /// The value returned by the operation on a success.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the result is a failure.</exception>
        public TSuccess SuccessValue
        {
            get
            {
                if(IsFailure)
                    throw new InvalidOperationException("Result does not have a success value.");

                return _successValue;
            }
            private set => _successValue = value;
        }

        /// <summary>
        /// The value returned by the operation on a failure.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the result is a success.</exception>
        public TFailure FailureValue
        {
            get
            {
                if(IsSuccess)
                    throw new InvalidOperationException("Result does not have a failure value.");

                return _failureValue;
            }
            private set => _failureValue = value;
        }

        /// <summary>
        /// Constructor for a success result with a value.
        /// </summary>
        /// <param name="successValue">The value returned by the operation that succeeded.</param>
        protected Result(TSuccess successValue)
        {
            SuccessValue = successValue;
        }

        /// <summary>
        /// Constructor for an unsuccessful result with a failure value and the specified general error message and an optional set of detailed error messages.
        /// </summary>
        /// <param name="error">The general error message to use for the result.</param>
        /// <param name="failureValue">The value returned by the operation that failed.</param>
        /// <param name="errorMessages">Any additional, detailed error messages to include.</param>
        protected Result(string error, TFailure failureValue, params KeyValuePair<string, ICollection<string>>[] errorMessages) 
            : base(error, errorMessages)
        {
            FailureValue = failureValue;
        }

        /// <summary>
        /// Creates a success result with a value.
        /// </summary>
        /// <param name="successValue">The value returned by the operation that succeeded.</param>
        public static Result<TSuccess, TFailure> Ok(TSuccess successValue) 
            => new Result<TSuccess, TFailure>(successValue);

        /// <summary>
        /// Creates an unsuccessful result with a failure value and the specified general error message and an optional set of detailed error messages.
        /// </summary>
        /// <param name="error">The general error message to use for the result.</param>
        /// <param name="failureValue">The value returned by the operation that failed.</param>
        /// <param name="errorMessages">Any additional, detailed error messages to include.</param>
        public static Result<TSuccess, TFailure> Fail(string error, TFailure failureValue, params KeyValuePair<string, ICollection<string>>[] errorMessages) 
            => new Result<TSuccess, TFailure>(error, failureValue, errorMessages);
    }
}
