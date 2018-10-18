using System;
using System.Collections.Generic;
using System.Linq;

namespace RateReader.CurrencyContext.Entities
{
    public class CurrencyCollection : List<Currency>
    {
        public decimal Median()
        {
            return Count == 0 
                ? 0 
                : Math.Round(this.Sum(e => e.GetUnitCost()) / Count, 3);
        }

        public decimal Min()
        {
            return Count == 0 
                ? 0 
                : this.Min(e => e.GetUnitCost());
        }

        public decimal Max()
        {            
            return Count == 0 
                ? 0 
                : this.Max(e => e.GetUnitCost());
        }
    }
}