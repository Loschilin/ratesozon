using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using RateReader.Cnb.Providers.Services;
using Xunit;

namespace RateReader.Cnb.Providers.Tests
{
    public class CurrentCurrencyReaderTest
    {
        [Fact]
        public void TryReadCurrencyTest()
        {
            var reader = new CurrentCurrencyReader(new EmptyLogger<CurrentCurrencyReader>());
            var resultRead = reader.TryReadCurrency("Australia|dollar|1|AUD|16.255", out var result);

            Assert.True(resultRead, "CurrentCurrencyReader.TryReadCurrency return false");
            Assert.NotNull(result);

            Assert.Equal(1, result.Amount);
            Assert.Equal(16.255m, result.Rate);
            Assert.Equal("AUD", result.Code);
            Assert.Equal("Australia", result.Country);
            Assert.Equal("dollar", result.Currency);
        }
    }
}
