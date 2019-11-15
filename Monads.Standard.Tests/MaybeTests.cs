using System;
using System.Linq;
using FakeItEasy;
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

        [Test]
        public void FirstOrDefaultCastToMaybeShouldReturnEmptyMaybe()
        {
            var collection = new[] { new FakeClass(1), new FakeClass(2) };

            Maybe<FakeClass> first = collection.FirstOrDefault(x => x.Prop2 == 100);
            Action action = () => { var a = first.Value; };

            first.HasNoValue.Should().BeTrue();
            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void MapShouldNotCallFunctionWhenHasNoValue()
        {
            var none = Maybe<int>.Empty();

            var mapFunction = A.Fake<Func<int, Maybe<int>>>();

            none.Map(mapFunction);

            A.CallTo(() => mapFunction.Invoke(A<int>._)).MustNotHaveHappened();
        }
        [Test]
        public void MapShouldCallFunctionWhenHasValue()
        {
            var none = Maybe<int>.Create(5);

            var mapFunction = A.Fake<Func<int, Maybe<int>>>();

            none.Map(mapFunction);

            A.CallTo(() => mapFunction.Invoke(5)).MustHaveHappened();
        }

        [Test]
        public void MapShouldReturnEmptyMaybeWhenNoValue()
        {
            var none = Maybe<int>.Empty();

            var result = none.Map<int>(x => 5);

            result.HasNoValue.Should().BeTrue();
        }

        [Test]
        public void CoalesceShouldReturnValueWhenHasValue()
        {
            var value = Maybe<int>.Create(5);

            var result = value.Coalesce(3);

            result.Should().Be(5);
        }

        [Test]
        public void CoalesceShouldReturnDefaultValueWhenHasNoValue()
        {
            var none = Maybe<int>.Empty();

            var result = none.Coalesce(3);

            result.Should().Be(3);
        }


        [Test]
        public void ApplyShouldNotCallFunctionWhenHasNoValue()
        {
            var none = Maybe<int>.Empty();

            var action = A.Fake<Action<int>>();

            none.Apply(action);

            A.CallTo(() => action.Invoke(A<int>._)).MustNotHaveHappened();
        }

        [Test]
        public void ApplyShouldCallFunctionWhenHasValue()
        {
            var none = Maybe<int>.Create(5);

            var action = A.Fake<Action<int>>();

            none.Apply(action);

            A.CallTo(() => action.Invoke(5)).MustHaveHappened();
        }

        [Test]
        public void ChainingShouldWork()
        {
            Maybe<int> init = 5;

            var mapper1 = A.Fake<Func<int, Maybe<string>>>();

            A.CallTo(() => mapper1.Invoke(5))
             .Returns("5");

            var mapper2 = A.Fake<Func<string, Maybe<int>>>();

            A.CallTo(() => mapper2.Invoke("5"))
             .Returns(5);

            var mapper3 = A.Fake<Func<int, Maybe<string>>>();
            A.CallTo(() => mapper3.Invoke(5))
             .Returns(Maybe<string>.Empty());

            var mapper4 = A.Fake<Func<string, Maybe<int>>>();
            A.CallTo(() => mapper4.Invoke("5"))
             .Returns(5);

            var apply = A.Fake<Action<int>>();

            init.Map(mapper1).Map(mapper2).Map(mapper3)
                .Map(mapper4)
                .Apply(apply);

            A.CallTo(() => mapper1.Invoke(5))
             .MustHaveHappened();

            A.CallTo(() => mapper2.Invoke("5"))
             .MustHaveHappened();

            A.CallTo(() => mapper3.Invoke(5))
             .MustHaveHappened();

            A.CallTo(() => mapper4.Invoke(A<string>._))
             .MustNotHaveHappened();

            A.CallTo(() => apply.Invoke(A<int>._))
             .MustNotHaveHappened();
        }
    }
}