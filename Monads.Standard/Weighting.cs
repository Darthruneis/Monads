using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads
{
    /// <summary>
    /// A weighting represents a fraction that is used to determine the
    /// impact something has relative to other <see cref="Weighting" />s.
    /// </summary>
    /// <seealso cref="Weighting" />
    public class Weighting : IEquatable<Weighting>
    {
        /// <summary>
        ///     The lower portion of the <see cref="Weighting" />. The 'Y' in 'X in Y chance of Z'.
        /// </summary>
        public ulong Denominator { get; }

        /// <summary>
        ///     The upper portion of the <see cref="Weighting" />. The 'X' in 'X in Y chance of Z'.
        /// </summary>
        public ulong Numerator { get; }

        /// <summary>A <see cref="Weighting" /> of zero has no impact.</summary>
        /// <value>The zero.</value>
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

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        /// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Weighting);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
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

        /// <summary>Creates the specified <see cref="Weighting" /> based on the specified fraction.</summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns>A <see cref="Weighting" /> which represents the specified fraction.</returns>
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

        /// <summary>Normalizes the specified set of <see cref="Weighting" />s so that they are all built on the same denominator.</summary>
        /// <param name="set">The set of <see cref="Weighting" />s to be normalized.</param>
        /// <returns>A new set of <see cref="Weighting" />s which have the same denominator and the same relative <see cref="Weighting" /> as the originally provided set.</returns>
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

        /// <summary>Implements the operator ==.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Weighting left, Weighting right)
        {
            return left == null 
                ? right == null 
                : left.Equals(right);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Weighting left, Weighting right)
        {
            return !(left == right);
        }

        /// <summary>Simplifies this <see cref="Weighting" /> to the lowest common denominator.</summary>
        /// <returns>A new <see cref="Weighting" /> with the simplified values.</returns>
        public Weighting Simplify()
        {
            for (ulong i = 2; i <= Numerator; i++)
                if ((Numerator % i == 0)
                 && (Denominator % i == 0))
                    return new Weighting(Numerator / i, Denominator / i);

            return new Weighting(Numerator, Denominator);
        }
    }
}