using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads
{
    public class Weighting : IEquatable<Weighting>
    {
        /// <summary>
        ///     The lower portion of the weighting. The 'Y' in 'X in Y chance of Z'.
        /// </summary>
        public ulong Denominator { get; }

        /// <summary>
        ///     The upper portion of the weighting. The 'X' in 'X in Y chance of Z'.
        /// </summary>
        public ulong Numerator { get; }

        public static Weighting Zero => new Weighting(0, 1);

        private Weighting(ulong numerator, ulong denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public bool Equals(Weighting other)
        {
            return (other != null)
                   && Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Weighting);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 9323;
                const int multiple = 46807;

                hash = (hash * multiple) + Numerator.GetHashCode();
                hash = (hash * multiple) + Denominator.GetHashCode();

                return hash;
            }
        }

        public static Result<Weighting> Create(ulong numerator, ulong denominator)
        {
            if (numerator > denominator)
                return Result.Fail<Weighting>("A weighting's numerator must not exceed its denominator.");

            if (numerator < 1)
                return Result.Fail<Weighting>("A weighting's numerator must be at least zero.");

            if (denominator < 1)
                return Result.Fail<Weighting>("A weighting's denominator must be positive.");

            if (numerator == 0)
                return Zero;

            return new Weighting(numerator, denominator);
        }

        /// <summary>
        ///     Finds a common denominator with the specified <see cref="Weighting" />.
        ///     This is not guaranteed to be the lowest common denominator.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public ulong DetermineCommonDenominator(Weighting other)
        {
            return DetermineCommonDenominator(Denominator, other.Denominator);
        }

        /// <summary>
        ///     Finds a common denominator between the two specified <see cref="Weighting" />s.
        ///     This is not guaranteed to be the lowest common denominator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ulong DetermineCommonDenominator(ulong left, ulong right)
        {
            var leftMultiples = new List<ulong>();
            var rightMultiples = new List<ulong>();
            ulong smallerDenominator = Math.Min(left, right);
            for (ulong multiplier = 1; multiplier <= smallerDenominator; multiplier++)
            {
                leftMultiples.Add(left * multiplier);
                rightMultiples.Add(right * multiplier);

                foreach (ulong multiple in leftMultiples)
                    if (rightMultiples.Contains(multiple))
                        return multiple;
            }

            return left * right;
        }

        public static ICollection<Weighting> Normalize(IList<Weighting> set)
        {
            if ((set == null) || (set.Count <= 1))
                return set;

            if (set.Select(x => x.Denominator).Distinct().Count() == 1)
                return set;

            set = set.Select(x => x.Simplify()).ToList();

            ulong normalizedDenominator = set[0].Denominator;
            for (int i = 1; i < set.Count; i++)
            {
                ulong commonDenominator = set[i].DetermineCommonDenominator(set[i - 1]);
                normalizedDenominator = DetermineCommonDenominator(commonDenominator, normalizedDenominator);
            }

            return set.Select(x =>
                              {
                                  if (x.Denominator == normalizedDenominator)
                                      return x;

                                  ulong multiplier = normalizedDenominator / x.Denominator;
                                  return new Weighting(x.Numerator * multiplier, normalizedDenominator);
                              }).ToList();
        }

        public static bool operator ==(Weighting left, Weighting right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Weighting left, Weighting right)
        {
            return !(left == right);
        }

        public Weighting Simplify()
        {
            for (ulong i = 2; i <= Numerator; i++)
                if ((Numerator % i == 0)
                    && (Denominator % i == 0))
                    return new Weighting(Numerator / i, Denominator / i);

            return this;
        }
    }
}