using System;
using System.Runtime.CompilerServices;
using RateReader.CurrencyContext.Entities;
using RateReader.CurrencyContext.Factories.Interfaces;

[assembly: InternalsVisibleTo("ReateReader.CurrencyContext.Tests")]

namespace RateReader.CurrencyContext.Factories
{
    internal class CurrencyFactory : ICurrecyFactory
    {
        public Currency Create(string code, int amount, decimal rate, DateTime date)
        {
            var result = new Currency(code, amount, rate, date);
            return result;
        }
    }
}