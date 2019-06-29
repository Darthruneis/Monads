using FluentAssertions;
using NUnit.Framework;

namespace Monads.Tests
{
    [TestFixture]
    public class ProbabilityTests
    {
        [TestCase(0.5, 1.0, true)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(1.0, 0.5, false)]
        public void LessThanTests(double left, double right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            bool result = l < r;

            result.Should().Be(expected);
        }

        [TestCase(0.5, 1.0, false)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(1.0, 0.5, true)]
        public void GreaterThanTests(double left, double right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            bool result = l > r;

            result.Should().Be(expected);
        }

        [TestCase(0.5, 1.0, false)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(1.0, 0.5, true)]
        public void GreaterThanEqualToTests(double left, double right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            bool result = l >= r;

            result.Should().Be(expected);
        }

        [TestCase(0.5, 1.0, true)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(1.0, 0.5, false)]
        public void LessThanEqualToTests(double left, double right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            bool result = l <= r;

            result.Should().Be(expected);
        }

        [TestCase(0.5, 1.0, false)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(1.0, 0.5, false)]
        public void EqualityTests(double left, double right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            bool result = l == r;

            result.Should().Be(expected);
        }

        [TestCase(0.5, 1.0, true)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(1.0, 0.5, true)]
        public void InequalityTests(double left, double right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            bool result = l != r;

            result.Should().Be(expected);
        }

        [TestCase(0.5, 1.0, false)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(1.0, 0.5, false)]
        public void EqualsTests(double left, double right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            bool result = l.Equals(r);

            result.Should().Be(expected);
        }

        [TestCase(1.0, 0.0, false)]
        [TestCase(1.0, "1.0", false)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(1.0, '1', false)]
        public void ObjectEqualityTests(double left, object right, bool expected)
        {
            Probability l = Probability.Create((decimal)left).Value;

            bool result = l.Equals(right);

            result.Should().Be(expected);
        }

        [Test]
        public void ObjectIsEqual()
        {
            Probability left = Probability.Create(1.0m).Value;
            Probability right = Probability.Create(1.0m).Value;
            object obj = (object)right;

            bool result = left.Equals(obj);

            result.Should().BeTrue();
        }

        [TestCase(0.5, 0.7, -1)]
        [TestCase(1.0, 1.0, 0)]
        [TestCase(1.0, 0.5, 1)]
        public void CompareToTests(double left, double right, int expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            int result = l.CompareTo(r);

            result.Should().Be(expected);
        }

        [TestCase(0.5, 0.25, 0.75)]
        [TestCase(0.5, 0.5, 1.0)]
        [TestCase(1.0, 0.5, 1.0)]
        [TestCase(0.0, 0.0, 0.0)]
        public void AdditionTests(double left, double right, double expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            Probability result = l + r;

            result.Chance.Should().Be((decimal)expected);
        }

        [TestCase(0.5, 0.25, 0.25)]
        [TestCase(0.5, 0.5, 0.0)]
        [TestCase(1.0, 0.5, 0.5)]
        [TestCase(0.0, 0.0, 0.0)]
        [TestCase(0.5, 1.0, 0.0)]
        public void SubtractionTests(double left, double right, double expected)
        {
            Probability l = Probability.Create((decimal)left).Value;
            Probability r = Probability.Create((decimal)right).Value;

            Probability result = l - r;

            result.Chance.Should().Be((decimal)expected);
        }
    }
}
