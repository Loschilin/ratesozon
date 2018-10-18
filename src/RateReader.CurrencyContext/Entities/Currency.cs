using System;

namespace RateReader.CurrencyContext.Entities
{
    public struct Currency
    {
        public Currency(string code, int amount, decimal rate, DateTime date)
        {
            Code = code;
            Amount = amount;
            Date = date;
            Rate = rate;
        }

        public string Code { get; }
        public int Amount { get; }
        public decimal Rate { get; }
        public DateTime Date { get; }

        public decimal GetUnitCost()
        {
            return Amount == 0 ? 0 : Math.Round(Rate / Amount, 3);
        }
    }
}