using System;

namespace Monads
{
    public class Probability : IEquatable<Probability>, IComparable<Probability>
    {
        public decimal Chance { get; }

        private Probability(decimal chance)
        {
            if(chance < decimal.Zero)
                throw new ArgumentOutOfRangeException(nameof(chance), chance, "Probability chance must not be negative.");

            if (chance > decimal.One)
                throw new ArgumentOutOfRangeException(nameof(chance), chance, "Probability chance must not exceed 1.");

            Chance = chance;
        }

        private Probability(double chance) : this((decimal)chance) { }

        public static Probability Zero => new Probability(decimal.Zero);
        public static Probability One => new Probability(decimal.One);

        public static Result<Probability> Create(double chance) => Create((decimal)chance);
        public static Result<Probability> Create(decimal chance)
        {
            if(chance < decimal.Zero)
                return Result<Probability>.Fail("Probability chance must not be negative.");

            if(chance > decimal.One)
                return Result<Probability>.Fail("Probability chance must not exceed 1.");

            return new Probability(chance);
        }

        public static Probability operator +(Probability left, Probability right)
        {
            decimal chance = left.Chance + right.Chance;

            if (chance >= decimal.One) return One;
            if (chance <= decimal.Zero) return Zero;
            return new Probability(left.Chance + right.Chance);
        }

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
        public override bool Equals(object obj) => (obj is Probability) && CompareTo((Probability)obj) == 0;
        public override int GetHashCode() => 340839689 + Chance.GetHashCode();

        public static bool operator <(Probability left, Probability right) => left.CompareTo(right) < 0;
        public static bool operator >(Probability left, Probability right) => left.CompareTo(right) > 0;
        public static bool operator <=(Probability left, Probability right) => left.CompareTo(right) <= 0;
        public static bool operator >=(Probability left, Probability right) => left.CompareTo(right) >= 0;
        public static bool operator ==(Probability left, Probability right) => left.CompareTo(right) == 0;
        public static bool operator !=(Probability left, Probability right) => left.CompareTo(right) != 0;
    }
}
