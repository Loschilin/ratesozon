using System;
using RateReader.CurrencyContext.Factories;
using Xunit;

namespace ReateReader.CurrencyContext.Tests
{
    public class CurrencyTests
    {
        private readonly CurrencyFactory _currencyFactory;

        public CurrencyTests()
        {
            _currencyFactory = new CurrencyFactory();
        }

        [Fact]
        public void CurrencyTest()
        {
            var currency = _currencyFactory.Create("USD", 10, 300, DateTime.Today);
            var unitCost = currency.GetUnitCost();

            Assert.True(unitCost == 30, $"{unitCost} <> {30}");
        }
    }
}