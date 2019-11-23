using System;

namespace Monads
{
    /// <summary>
    /// A probability represents the likelihood of something happening.
    ///
    /// It can range from 0 to 1.0.
    /// </summary>
    /// <seealso cref="IEquatable{Probability}" />
    /// <seealso cref="IComparable{Probability}" />
    public class Probability : IEquatable<Probability>, IComparable<Probability>
    {
        /// <summary>Gets the chance.</summary>
        /// <value>The chance.</value>
        public decimal Chance { get; }

        private Probability(decimal chance)
        {
            if (chance < decimal.Zero)
                throw new ArgumentOutOfRangeException(nameof(chance), chance, "Probability chance must not be negative.");

            if (chance > decimal.One)
                throw new ArgumentOutOfRangeException(nameof(chance), chance, "Probability chance must not exceed 1.");

            Chance = chance;
        }

        private Probability(double chance) : this((decimal)chance) { }

        /// <summary>  A probability of zero is something that will never happen.</summary>
        /// <value>The zero.</value>
        public static Probability Zero => new Probability(decimal.Zero);

        /// <summary>  A probability of one is something that will always happen.</summary>
        /// <value>The one.</value>
        public static Probability One => new Probability(decimal.One);

        /// <summary>Creates the specified probability.</summary>
        /// <param name="chance">The probability.</param>
        /// <returns></returns>
        public static Result<Probability> Create(double chance) => Create((decimal)chance);

        /// <summary>Creates the specified probability.</summary>
        /// <param name="chance">The probability.</param>
        /// <returns></returns>
        public static Result<Probability> Create(decimal chance)
        {
            if (chance < decimal.Zero)
                return Result<Probability>.Fail("Probability chance must not be negative.");

            if (chance > decimal.One)
                return Result<Probability>.Fail("Probability chance must not exceed 1.");

            return new Probability(chance);
        }

        /// <summary>Implements the operator +.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Probability operator +(Probability left, Probability right)
        {
            decimal chance = left.Chance + right.Chance;

            if (chance >= decimal.One) return One;
            if (chance <= decimal.Zero) return Zero;

            return new Probability(left.Chance + right.Chance);
        }

        /// <summary>Implements the operator -.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Probability operator -(Probability left, Probability right)
        {
            decimal chance = left.Chance - right.Chance;

            if (chance >= decimal.One) return One;
            if (chance <= decimal.Zero) return Zero;

            return new Probability(left.Chance - right.Chance);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(Probability other) => Chance.Equals(other.Chance);

        /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object. </summary>
        /// <param name="other">An object to compare with this instance. </param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order. </returns>
        public int CompareTo(Probability other) => Chance.CompareTo(other.Chance);

        /// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => (obj is Probability) && CompareTo((Probability)obj) == 0;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() => 340839689 + Chance.GetHashCode();

        /// <summary>Implements the operator &lt;.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Probability left, Probability right) => left.CompareTo(right) < 0;

        /// <summary>Implements the operator &gt;.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Probability left, Probability right) => left.CompareTo(right) > 0;

        /// <summary>Implements the operator &lt;=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Probability left, Probability right) => left.CompareTo(right) <= 0;

        /// <summary>Implements the operator &gt;=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Probability left, Probability right) => left.CompareTo(right) >= 0;

        /// <summary>Implements the operator ==.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Probability left, Probability right) => left.CompareTo(right) == 0;

        /// <summary>Implements the operator !=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Probability left, Probability right) => left.CompareTo(right) != 0;
    }
}