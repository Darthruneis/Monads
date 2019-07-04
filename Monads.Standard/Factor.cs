using System;

namespace Monads
{
    /// <summary>
    /// A standardized method of defining a multiplier.
    /// </summary>
    public class Factor : IEquatable<Factor>, IComparable<Factor>
    {
        /// <summary>
        /// The rate of the factor. e.g. the multiplier.
        /// </summary>
        public decimal Rate { get; }
        
        /// <summary>
        /// Creates a factor from a double.
        /// </summary>
        /// <param name="rate">The rate of the factor.</param>
        public Factor(double rate) : this((decimal)rate) { }

        /// <summary>
        /// Creates a factor from an int.
        /// </summary>
        /// <param name="rate">The rate of the factor.</param>
        public Factor(int rate) : this((decimal)rate) { }

        /// <summary>
        /// Creates a factor from a long.
        /// </summary>
        /// <param name="rate">The rate of the factor.</param>
        public Factor(long rate) : this((decimal)rate) { }

        /// <summary>
        /// Creates a factor from a decimal.
        /// </summary>
        /// <param name="rate">The rate of the factor.</param>
        public Factor(decimal rate)
        {
            Rate = rate;
        }

        /// <summary>
        /// A factor of zero.
        /// </summary>
        public static Factor Zero => new Factor(decimal.Zero);

        /// <summary>
        /// A factor of one.
        /// </summary>
        public static Factor One => new Factor(1m);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>
        /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj) => (obj is Factor factor) && Equals(factor);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(Factor other) => !(other is null) && Rate.Equals(other.Rate);

        /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object. </summary>
        /// <param name="other">An object to compare with this instance. </param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order. </returns>
        public int CompareTo(Factor other) => other is null ? -1 : Rate.CompareTo(other.Rate);

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => 439619743 + Rate.GetHashCode();

        /// <summary>
        /// Compares two factors to determine if the left factor is less than the right factor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(Factor left, Factor right) 
            => !(left is null) && !(right is null) && left.CompareTo(right) < 0;

        /// <summary>
        /// Compares two factors to determine if the left factor is greater than the right factor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(Factor left, Factor right) 
            => !(left is null) && !(right is null) && left.CompareTo(right) > 0;

        /// <summary>
        /// Compares two factors to determine if the left factor is less than or equal to the right factor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(Factor left, Factor right) 
            => left is null ? right is null : left.CompareTo(right) <= 0;

        /// <summary>
        /// Compares two factors to determine if the left factor is greater than or equal to the right factor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(Factor left, Factor right) 
            => left is null ? right is null : left.CompareTo(right) >= 0;

        /// <summary>
        /// Compares two factors to determine if the left factor is equal to the right factor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Factor left, Factor right) 
            => left is null ? right is null : left.CompareTo(right) == 0;

        /// <summary>
        /// Compares two factors to determine if the left factor is not equal to the right factor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Factor left, Factor right) => !(left == right);
        
        public static implicit operator Factor(decimal value) => new Factor(value);
        public static implicit operator decimal(Factor value) => value.Rate;

        public static implicit operator Factor(double value) => new Factor(value);
        public static implicit operator double(Factor value) => (double)value.Rate;
    }
}
