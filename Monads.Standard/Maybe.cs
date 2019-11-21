using System;

namespace Monads
{
    /// <summary>
    /// A wrapper for an object that might be null.
    /// </summary>
    /// <typeparam name="T">The type of object that may be contained inside the wrapper.</typeparam>
    public struct Maybe <T> : IEquatable<Maybe<T>>
    {
        private readonly T _value;

        /// <summary>
        /// The value that may or may not be present.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the maybe does not have a value.</exception>
        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException("Cannot access the value of an empty Maybe.");

                return _value;
            }
        }

        /// <summary>
        /// Whether or not the maybe has been created with a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Whether or not the maybe has been created without a value. Inverse of <see cref="HasValue"/>.
        /// </summary>
        public bool HasNoValue => !HasValue;

        private Maybe(T value)
        {
            HasValue = true;
            _value = value;
        }

        /// <summary>
        /// Calls the specified <paramref name="f"></paramref>
        /// </summary>
        /// <typeparam name="TResult">The type of result for the operation that will be mapped.</typeparam>
        /// <param name="f">The function to call on the value when it is present.WW
        /// </param>
        /// <returns></returns>
        public Maybe<TResult> Map <TResult>(Func<T, Maybe<TResult>> f) => !HasValue ? Maybe<TResult>.Empty() : f(Value);

        /// <summary>
        /// Retrieves the value that is stored, or the default value
        /// when no value is available.
        /// </summary>
        /// <param name="defaultValue">The default value to use.</param>
        /// <returns></returns>
        public T Coalesce(T defaultValue) => HasValue ? Value : defaultValue;

        /// <summary>
        /// Applies the provided <paramref name="f"/> to the <see href="Value"/>
        /// when the value is present.
        /// </summary>
        /// <param name="f">The function to be applied if there is a value stored.</param>
        /// <returns></returns>
        public void Apply(Action<T> f)
        {
            if (HasValue) f(Value);
        }

        /// <summary>
        /// Creates a maybe with a value.
        /// </summary>
        /// <param name="value">The value for the maybe.</param>
        /// <returns></returns>
        public static Maybe<T> Create(T value) =>
            ReferenceEquals(null, value)
                ? new Maybe<T>()
                : new Maybe<T>(value);

        /// <summary>
        /// Creates a maybe without a value.
        /// </summary>
        /// <returns></returns>
        public static Maybe<T> Empty() => new Maybe<T>();

        /// <summary>
        /// Allows all values to be cast as a Maybe, for example from a call to FirstOrDefault().
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Maybe<T> (T value) => Create(value);

        public bool Equals(Maybe<T> other)
        {
            return (HasNoValue && other.HasNoValue) 
                || (HasValue && other.HasValue && other == Value);
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            if (obj is T t)
                return this == t;

            return obj is Maybe<T> other && Equals(other);
        }

        public static bool operator ==(Maybe<T> left, T right)
        {
            return left.HasValue && left.Value.Equals(right);
        }

        public static bool operator !=(Maybe<T> left, T right)
        {
            return !(left == right);
        }

        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return (left.HasNoValue && right.HasNoValue)
                || (left.HasValue && right.HasValue &&(left == right.Value));
        }

        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !(left == right);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            if (HasNoValue) return default(T)?.GetHashCode() ?? 0;

            return Value.GetHashCode();
        }
    }
}
