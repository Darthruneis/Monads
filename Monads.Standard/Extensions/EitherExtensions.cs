using System;
using System.Threading.Tasks;

namespace Monads.Extensions
{
    public static class EitherExtensions
    {
        //source: https://davefancher.com/2015/12/11/functional-c-chaining-async-methods/
        public static async Task<TResult> MapAsync<TSource, TResult>(
            this Task<TSource> @this,
            Func<TSource, Task<TResult>> fn) => await fn(await @this);

        public static async Task<TResult> MapAsync<TSource, TResult>(
            this TSource @this,
            Func<TSource, Task<TResult>> fn) => await fn(@this);

        public static async Task<TResult> MapAsync<TSource, TResult>(
            this Task<TSource> @this,
            Func<TSource, TResult> fn) => fn(await @this);

        public static async Task<Either<TNewSuccess, TFailure>> ChainAsync<TSuccess, TFailure, TNewSuccess>(this Either<TSuccess, TFailure> @this, Func<TSuccess, Task<Either<TNewSuccess, TFailure>>> right)
        {
            return @this.IsLeft 
                ? Either.Left<TNewSuccess, TFailure>(@this.Left)
                : await right(@this.Right);
        }

        public static async Task<Either<TNewSuccess, TFailure>> ChainAsync<TSuccess, TFailure, TNewSuccess>(this Task<Either<TSuccess, TFailure>> @this, Func<TSuccess, Task<Either<TNewSuccess, TFailure>>> right)
        {
            var either = await @this;
            return either.IsLeft 
                ? Either.Left<TNewSuccess, TFailure>(either.Left)
                : await right(either.Right);
        }

        public static async Task<Either<TNewSuccess, TFailure>> ChainAsync<TSuccess, TFailure, TNewSuccess>(this Task<Either<TSuccess, TFailure>> @this, Func<TSuccess, Either<TNewSuccess, TFailure>> right)
        {
            var either = await @this;
            return either.Chain(right);
        }
    }
}