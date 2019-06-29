using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Monads.Tests
{
    [TestFixture]
    public class MaybeTests
    {
        private class FakeClass
        {
            public string Prop1 { get; set; }
            public long Prop2 { get; set; }

            /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
            public FakeClass(long prop2, string prop1 = null)
            {
                Prop1 = prop1;
                Prop2 = prop2;
            }
        }

        [Test]
        public void EmptyMaybeShouldNotHaveValue()
        {
            var maybe = Maybe<FakeClass>.Empty();

            maybe.HasNoValue.Should().BeTrue();
        }

        [Test]
        public void EmptyMaybeShouldThrowIfValueIsAccessed()
        {
            var maybe = Maybe<FakeClass>.Empty();

            Action action = () => { var a = maybe.Value; };

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void FirstOrDefaultCastToMaybeShouldReturnValue()
        {
            var collection = new[] {new FakeClass(1), new FakeClass(2)};

            Maybe<FakeClass> first = collection.FirstOrDefault(x => x.Prop2 == 1);
            Action action = () => { var a = first.Value; };

            first.HasValue.Should().BeTrue();
            action.Should().NotThrow();
        }

        private Maybe<T> Get<T>(T value)
            where T: class
        {
            return value;
        }

        [Test]
        public void FirstOrDefaultCastToMaybeShouldReturnEmptyMaybe()
        {
            var collection = new[] { new FakeClass(1), new FakeClass(2) };

            Maybe<FakeClass> first = collection.FirstOrDefault(x => x.Prop2 == 100);
            Action action = () => { var a = first.Value; };

            first.HasNoValue.Should().BeTrue();
            action.Should().Throw<InvalidOperationException>();
        }
    }
}