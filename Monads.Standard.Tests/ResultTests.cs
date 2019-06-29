using System;
using FluentAssertions;
using NUnit.Framework;

namespace Monads.Tests
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void ErrorResultsMustHaveAnErrorMessage()
        {
            var result = Result.Fail("message");

            result.IsFailure.Should().BeTrue();
        }

        [Test]
        public void SuccessResultsMustNotHaveAnErrorMessage()
        {
            var result = Result.Ok();

            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void FailRequiresAValidErrorMessage()
        {
            Action action = () => Result.Fail("");

            action.Should().Throw<Exception>();       
        }
    }

    [TestFixture]
    public class ResultWithValueTests
    {

    }

    [TestFixture]
    public class ResultWithFailureValueTests
    {

    }
}
