using FluentAssertions;
using NUnit.Framework;

namespace Monads.Tests
{
    [TestFixture]
    public class FactorTests
    {
        [TestCase(1.0, 2.0, true)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(2.0, 1.0, false)]
        public void LessThanTests(double left, double right, bool expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            bool result = l < r;

            result.Should().Be(expected);
        }

        [TestCase(1.0, 2.0, false)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(2.0, 1.0, true)]
        public void GreaterThanTests(double left, double right, bool expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            bool result = l > r;

            result.Should().Be(expected);
        }

        [TestCase(1.0, 2.0, false)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(2.0, 1.0, true)]
        public void GreaterThanEqualToTests(double left, double right, bool expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            bool result = l >= r;

            result.Should().Be(expected);
        }

        [TestCase(1.0, 2.0, true)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(2.0, 1.0, false)]
        public void LessThanEqualToTests(double left, double right, bool expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            bool result = l <= r;

            result.Should().Be(expected);
        }

        [TestCase(1.0, 2.0, false)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(2.0, 1.0, false)]
        public void EqualityTests(double left, double right, bool expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            bool result = l == r;

            result.Should().Be(expected);
        }

        [TestCase(1.0, 2.0, true)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(2.0, 1.0, true)]
        public void InequalityTests(double left, double right, bool expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            bool result = l != r;

            result.Should().Be(expected);
        }

        [TestCase(1.0, 2.0, false)]
        [TestCase(1.0, 1.0, true)]
        [TestCase(2.0, 1.0, false)]
        public void EqualsTests(double left, double right, bool expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            bool result = l.Equals(r);

            result.Should().Be(expected);
        }

        [TestCase(1.0, 2.0, false)]
        [TestCase(1.0, "1.0", false)]
        [TestCase(1.0, 1.0, false)]
        [TestCase(2.0, '1', false)]
        public void ObjectEqualityTests(double left, object right, bool expected)
        {
            Factor l = new Factor((decimal)left);

            bool result = l.Equals(right);

            result.Should().Be(expected);
        }

        [Test]
        public void ObjectIsEqual()
        {
            Factor left = new Factor(1.0m);
            Factor right = new Factor(1.0m);
            object obj = (object)right;

            bool result = left.Equals(obj);

            result.Should().BeTrue();
        }

        [TestCase(1.0, 2.0, -1)]
        [TestCase(1.0, 1.0, 0)]
        [TestCase(2.0, 1.0, 1)]
        public void CompareToTests(double left, double right, int expected)
        {
            Factor l = new Factor((decimal)left);
            Factor r = new Factor((decimal)right);

            int result = l.CompareTo(r);

            result.Should().Be(expected);
        }
    }
}
