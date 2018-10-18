using System;
using RateReader.CurrencyContext.Entities;

namespace RateReader.CurrencyContext.Factories.Interfaces
{
    public interface ICurrecyFactory
    {
        Currency Create(string code, int amount, decimal rate, DateTime date);
    }
}