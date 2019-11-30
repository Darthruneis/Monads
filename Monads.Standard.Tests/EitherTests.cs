using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace Monads.Tests
{
    [TestFixture]
    [Category("Unit")]
    public class EitherTests
    {
        [Test]
        public void RightShouldThrowWhenEitherIsLeft()
        {
            var left = Either.Left<int, string[]>(new string[0]);

            Action action = () =>
                            {
                                var a = left.Right;
                            };

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void LeftShouldThrowWhenEitherIsRight()
        {
            var right = Either.Right<int, string[]>(1);

            Action action = () =>
                            {
                                var a = right.Left;
                            };

            action.Should().Throw<InvalidOperationException>();
        }

        private Either<int, string[]> Execute(Queue<Func<Either<int, string[]>>> chain)
        {
            var result = chain.Dequeue()();
            while (chain.Count > 0)
            {
                //need to call dequeue always - chain might not call the callback,
                //which then causes an infinite loop as Dequeue is never called from
                //that point
                var f = chain.Dequeue();
                result = result.Chain(right => f());

            }

            return result;
        }

        [Test]
        public void RightChainShouldEndWithARightEither()
        {
            var chain = new Queue<Func<Either<int, string[]>>>();
            chain.Enqueue(() => Either.Right<int, string[]>(1_000_000));
            chain.Enqueue(() => Either.Right<int, string[]>(100_000));
            chain.Enqueue(() => Either.Right<int, string[]>(10_000));

            var result = Execute(chain);

            result.IsRight.Should().BeTrue();
        }

        [Test]
        public void RightChainShouldCallAllMethods()
        {
            var first = new Fake<Func<Either<int, string[]>>>();
            first.CallsTo(x => x()).Returns(Either.Right<int, string[]>(1_000_000));

            var second = new Fake<Func<Either<int, string[]>>>();
            second.CallsTo(x => x()).Returns(Either.Right<int, string[]>(100_000));

            var third = new Fake<Func<Either<int, string[]>>>();
            third.CallsTo(x => x()).Returns(Either.Right<int, string[]>(10_000));

            var chain = new Queue<Func<Either<int, string[]>>>();
            chain.Enqueue(first.FakedObject);
            chain.Enqueue(second.FakedObject);
            chain.Enqueue(third.FakedObject);

            var result = Execute(chain);

            first.RecordedCalls.Count().Should().Be(1);
            second.RecordedCalls.Count().Should().Be(1);
            third.RecordedCalls.Count().Should().Be(1);
        }

        [Test]
        public void MixedChainShouldStopCallsPartwayThrough()
        {
            var first = new Fake<Func<Either<int, string[]>>>();
            first.CallsTo(x => x()).Returns(Either.Right<int, string[]>(1_000_000));

            var second = new Fake<Func<Either<int, string[]>>>();
            second.CallsTo(x => x()).Returns(Either.Left<int, string[]>(new string[0]));

            var third = new Fake<Func<Either<int, string[]>>>();
            third.CallsTo(x => x()).Returns(Either.Right<int, string[]>(100_000));

            var chain = new Queue<Func<Either<int, string[]>>>();
            chain.Enqueue(first.FakedObject);
            chain.Enqueue(second.FakedObject);
            chain.Enqueue(third.FakedObject);

            var result = Execute(chain);

            first.RecordedCalls.Count().Should().Be(1);
            second.RecordedCalls.Count().Should().Be(1);
            third.RecordedCalls.Count().Should().Be(0);
        }

        [Test]
        public void MixedChainShouldEndWithLeftEither()
        {
            var chain = new Queue<Func<Either<int, string[]>>>();
            chain.Enqueue(() => Either.Right<int, string[]>(1_000_000));
            chain.Enqueue(() => Either.Left<int, string[]>(new string[0]));
            chain.Enqueue(() => Either.Right<int, string[]>(10_000));

            var result = Execute(chain);

            result.IsLeft.Should().BeTrue();
        }
    }
}