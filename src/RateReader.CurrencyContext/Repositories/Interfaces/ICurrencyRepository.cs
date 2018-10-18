using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RateReader.CurrencyContext.Entities;

namespace RateReader.CurrencyContext.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        Task SaveAsync(params Currency[] currencies);
        Task<IReadOnlyCollection<Currency>> GetAsync(
            DateTime startDate, DateTime endDate, params string[] codes);
    }
}