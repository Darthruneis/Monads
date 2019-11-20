using System;

namespace Monads
{
    /// <summary>
    /// Contains a single value - <see cref="Right"/> or
    /// <cref href="Left"/>.
    /// </summary>
    /// <typeparam name="TSuccess"></typeparam>
    /// <typeparam name="TFailure"></typeparam>
    public struct Either<TSuccess, TFailure>
    {
        private readonly TSuccess _right;

        /// <summary>
        /// The right (happy path, correct) value. 
        /// </summary> 
        public TSuccess Right
        {
            get
            {
                if (!IsRight)
                    throw new InvalidOperationException($"{nameof(Right)} does not have a value. Check {nameof(IsRight)} before accessing {nameof(Right)}.");

                return _right;
            }
        }

        private readonly TFailure _left;

        /// <summary>
        /// The left (sad path, incorrect) value.
        /// </summary>
        public TFailure Left
        {
            get
            {
                if (!IsLeft)
                    throw new InvalidOperationException($"{nameof(Left)} does not have a value. Check {nameof(IsLeft)} before accessing {nameof(Left)}.");

                return _left;
            }
        }

        /// <summary>
        /// Indicates whether or not the <see cref="Right"/> value is set.
        /// </summary>
        public bool IsRight { get; }

        /// <summary>
        /// Indicates whether or not the <see cref="Left"/> value is set.
        /// </summary>
        public bool IsLeft => !IsRight;

        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}" /> with the
        /// <see cref="Right"/> value.
        /// </summary>
        /// <param name="right"><see cref="Right"/></param>
        public Either(TSuccess right)
        {
            _right = right;
            IsRight = true;
            _left = default;
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}" /> with the
        /// <see cref="Left"/> value.
        /// </summary>
        /// <param name="left"><see cref="Left"/></param>
        public Either(TFailure left)
        {
            _right = default;
            IsRight = false;
            _left = left;
        }

        /// <summary>
        /// Maps the either into a new either depending on whether the either
        /// <see cref="IsRight"/> or <see cref="IsLeft"/>.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="TSuccess" /> for the resulting Either.</typeparam>
        /// <typeparam name="TF">The <typeparamref name="TFailure" /> for the resulting Either.</typeparam>
        /// <param name="right">The function to be applied when the either <see cref="IsRight"/>.</param>
        /// <param name="left">The function to be applied when the either <see cref="IsLeft"/>.</param>
        /// <returns></returns>
        public Either<T, TF> Map<T, TF>(Func<TSuccess, Either<T, TF>> right, Func<TFailure, TF> left)
        {
            if (IsLeft)
                return Either.Left<T, TF>(left(Left));

            return right(Right);
        }

        /// <summary>
        /// Shorthand for <see cref="Map{T,TFailure}" /> when <typeparamref name="TFailure" /> is
        /// the same for the new either. 
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="TSuccess" /> for the resulting either.</typeparam>
        /// <param name="right">The function to be applied when the either <see cref="IsRight"/>.</param>
        /// <example>
        /// Replace `.Map(rightFunc, left => left)` with `.Chain(rightFunc)`.
        /// </example>
        public Either<T, TFailure> Chain<T>(Func<TSuccess, Either<T, TFailure>> right)
        {
            return Map(right, left => left);
        }

        /// <summary>
        /// Applies the provided <paramref name="action" /> when <see cref="IsRight" />.
        /// </summary>
        /// <param name="action">The action to apply which utilizes the <see cref="Right" /> value.</param>
        public void Apply(Action<TSuccess> action)
        {
            if(IsRight)
                action(Right);
        }

        /// <summary>
        /// Applies the provided <paramref name="action" /> when <see cref="IsLeft" />.
        /// </summary>
        /// <param name="action">The action to apply which utilizes the <see cref="Left" /> value.</param>
        public void Apply(Action<TFailure> action)
        {
            if(IsLeft)
                action(Left);
        }
    }

    /// <summary>
    /// Factory class for creating <see cref="Either{TLeft, TRight}" />s.
    /// </summary>
    public static class Either
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}" /> with
        /// the <see cref="Either{TLeft, TRight}.Left"/> value.
        ///
        /// Alias of <see cref="Either.Left{TRight, TLeft}" />.
        /// </summary>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TLeft"></typeparam>
        /// <param name="left"><see cref="Either{TLeft, TRight}.Left"/></param>
        /// <returns></returns>
        public static Either<TRight, TLeft> L<TRight, TLeft>(TLeft left)
            => Left<TRight, TLeft>(left);

        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}" /> with
        /// the <see cref="Either{TLeft, TRight}.Left"/> value.
        /// </summary>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TLeft"></typeparam>
        /// <param name="left"><see cref="Either{TLeft, TRight}.Left"/></param>
        /// <returns></returns>
        public static Either<TRight, TLeft> Left<TRight, TLeft>(TLeft left)
        {
            return new Either<TRight, TLeft>(left);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}" /> with
        /// the <see cref="Either{TLeft, TRight}.Right"/> value.
        ///
        /// Alias of <see cref="Either.Right{TRight, TLeft}" />.
        /// </summary>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TLeft"></typeparam>
        /// <param name="right"><see cref="Either{TLeft, TRight}.Right"/></param>
        public static Either<TRight, TLeft> R<TRight, TLeft>(TRight right)
            => Right<TRight, TLeft>(right);

        /// <summary>
        /// Creates an <see cref="Either{TLeft, TRight}" /> with
        /// the <see cref="Either{TLeft, TRight}.Right"/> value.
        /// </summary>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TLeft"></typeparam>
        /// <param name="right"><see cref="Either{TLeft, TRight}.Right"/></param>
        public static Either<TRight, TLeft> Right<TRight, TLeft>(TRight right)
        {
            return new Either<TRight, TLeft>(right);
        }
    }
}