using System;
using Monetair.References.Tests.Uniqueness.Helpers;
using Xunit;

// ReSharper disable once CheckNamespace
namespace Monetair.References.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TransactionReference_IdentityEmpty_FullYear_ReturnValue()
        {
            var fixedMoment = new DateTime(2021, 10, 25, 17, 0, 10, 230);
            var reference = new TransactionReferenceForTest();
            reference.SetIdentity(string.Empty); // not recommended
            var result = reference.GetValue(fixedMoment);

            Assert.Equal("2021298170010230", result);
            Assert.Equal(16, result.Length);
        }

        [Fact]
        public void TransactionReference_IdentitySingleChar_3PosYear_ReturnValue()
        {
            var fixedMoment = new DateTime(2021, 10, 25, 17, 0, 10, 230);
            var reference = new TransactionReferenceForTest();
            var result = reference.GetValue(fixedMoment);

            Assert.Equal("021298A170010230", result);
            Assert.Equal(16, result.Length);
        }

        [Fact]
        public void TransactionReference_IdentitySDoubleChar_2PosYear_ReturnValue()
        {
            var fixedMoment = new DateTime(2021, 10, 25, 17, 0, 10, 230);
            var reference = new TransactionReferenceForTest();
            reference.SetIdentity("MS");

            var result = reference.GetValue(fixedMoment);

            Assert.Equal("21298MS170010230", result);
            Assert.Equal(16, result.Length);
        }
    }
}