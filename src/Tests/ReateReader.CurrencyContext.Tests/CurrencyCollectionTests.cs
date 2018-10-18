using System;
using RateReader.CurrencyContext.Entities;
using RateReader.CurrencyContext.Factories;
using Xunit;

namespace ReateReader.CurrencyContext.Tests
{
    public class CurrencyCollectionTests
    {
        private readonly CurrencyFactory _currencyFactory;

        public CurrencyCollectionTests()
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

        [Fact]
        public void MinCurrencyTest()
        {
            var currencyCollection = new CurrencyCollection();

            var currency1 = _currencyFactory.Create("USD", 1, 100, DateTime.Today);
            var currency2 = _currencyFactory.Create("USD", 1, 200, DateTime.Today);
            var currency3 = _currencyFactory.Create("USD", 1, 300, DateTime.Today);

            currencyCollection.Add(currency1);
            currencyCollection.Add(currency2);
            currencyCollection.Add(currency3);

            var min = currencyCollection.Min();
            Assert.True(min == 100.0m, $"{min} <> {100.0m}");
        }

        [Fact]
        public void MaxCurrencyTest()
        {
            var currencyCollection = new CurrencyCollection();

            var currency1 = _currencyFactory.Create("USD", 1, 100, DateTime.Today);
            var currency2 = _currencyFactory.Create("USD", 1, 200, DateTime.Today);
            var currency3 = _currencyFactory.Create("USD", 1, 300, DateTime.Today);

            currencyCollection.Add(currency1);
            currencyCollection.Add(currency2);
            currencyCollection.Add(currency3);

            var max = currencyCollection.Max();
            Assert.True(max == 300.0m, $"{max} <> {300.0m}");
        }

        [Fact]
        public void MedianCurrencyTest()
        {
            var currencyCollection = new CurrencyCollection();

            var currency1 = _currencyFactory.Create("USD", 1, 100, DateTime.Today);
            var currency2 = _currencyFactory.Create("USD", 1, 200, DateTime.Today);
            var currency3 = _currencyFactory.Create("USD", 1, 300, DateTime.Today);

            currencyCollection.Add(currency1);
            currencyCollection.Add(currency2);
            currencyCollection.Add(currency3);

            var median = currencyCollection.Median();

            Assert.True(currencyCollection.Count == 3, "CurrencyCollection len <> 3");
            Assert.True(median == 200.0m, $"{median} <> {200.0m}");
        }
    }
}
