using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Monads.Tests
{
    [TestFixture]
    public class WeightingTests
    {
        #region  Test Methods

        [TestCase(2, 4, 4)]
        [TestCase(3, 5, 15)]
        [TestCase(5, 7, 35)]
        [TestCase(1, 3, 3)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 4, 4)]
        [TestCase(2, 5, 10)]
        public void LowestCommonDenominatorTests(int left, int right, int expected)
        {
            ulong result = Weighting.DetermineCommonDenominator((ulong)left, (ulong)right);

            result.Should().Be((ulong)expected);
        }

        [TestCase(12, 1, 2, 3)]
        [TestCase(24, 4, 8, 12, 6)]
        public void NormalizeTests(int expectedDenominator, params int[] denominators)
        {
            List<Weighting> set = denominators.Select(x => Weighting.Create(1, (ulong)x).Value).ToList();

            ICollection<Weighting> result = Weighting.Normalize(set);

            result.All(x => x.Denominator == (ulong)expectedDenominator).Should().BeTrue();
        }

        [TestCase(2, 4, 1, 2)]
        [TestCase(3, 9, 1, 3)]
        [TestCase(3, 6, 1, 2)]
        [TestCase(65, 735, 13, 147)]
        public void SimplifyTests(int top, int bottom, int expectedTop, int expectedBottom)
        {
            Weighting weighting = Weighting.Create((ulong)top, (ulong)bottom).Value;

            Weighting result = weighting.Simplify();

            result.Numerator.Should().Be((ulong)expectedTop);
            result.Denominator.Should().Be((ulong)expectedBottom);
        }

        #endregion
    }
}