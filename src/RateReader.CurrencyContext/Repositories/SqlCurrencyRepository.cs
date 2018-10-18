using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RateReader.CurrencyContext.Entities;
using RateReader.CurrencyContext.Factories.Interfaces;
using RateReader.CurrencyContext.Repositories.Interfaces;
using RateReader.Data.Contexts;
using RateReader.Data.Entities;
using RateReader.Data.Factories.Interfaces;

namespace RateReader.CurrencyContext.Repositories
{
    internal class SqlCurrencyRepository: ICurrencyRepository
    {
        private readonly IConetextFactory<DbCurrencyContext> _conetextFactory;
        private readonly ICurrecyFactory _currecyFactory;

        public SqlCurrencyRepository(
            IConetextFactory<DbCurrencyContext> conetextFactory,
            ICurrecyFactory currecyFactory)
        {
            _conetextFactory = conetextFactory;
            _currecyFactory = currecyFactory;
        }

        public async Task SaveAsync(params Currency[] currencies)
        {
            if (currencies == null || !EnumerableExtensions.Any(currencies))
            {
                return;
            }

            var minDate = currencies.Min(e => e.Date);
            var maxDate = currencies.Max(e => e.Date);

            var currecyCroups = currencies.GroupBy(e => e.Code);

            using (var context = _conetextFactory.Create())
            {
                var dbCurrencies = await context.Currencies.ToListAsync();
                var dbValues = await context.DbCurrencyValues
                    .Where(e => minDate <= e.Date && e.Date <= maxDate).ToListAsync();

                foreach (var currencyCroup in currecyCroups)
                {
                    var dbCurrency = dbCurrencies.FirstOrDefault(e => e.Code == currencyCroup.Key);

                    var newCurrency = dbCurrency == null; 

                    if (newCurrency)
                    {
                        dbCurrency = CreateCurrency(currencyCroup.Key);
                        await context.AddAsync(dbCurrency);
                    }

                    foreach (var currency in currencyCroup)
                    {
                        var dbValue = dbValues.FirstOrDefault(
                            e => e.Date == currency.Date && e.CurrencyId == dbCurrency.Id);

                        if (newCurrency || dbValue == null)
                        {
                            dbValue = CreateCurrencyValue(
                                dbCurrency.Id, currency.Amount, 
                                currency.Rate, currency.Date
                            );

                            dbValues.Add(dbValue);
                            await context.AddAsync(dbValue);
                        }

                        dbValue.Rate = currency.Rate;
                        dbValue.Amount = currency.Amount;
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private static DbCurrencyValue CreateCurrencyValue(
            Guid dbCurrencyId, int amount, decimal rate,  DateTime currencyDate)
        {
            var value = new DbCurrencyValue
            {
                CreatedDate = DateTime.Now, //TODO UTC
                CurrencyId = dbCurrencyId,
                Date = currencyDate,
                Id = Guid.NewGuid(), //TODO use identity factory
                Rate = rate,
                Amount = amount
            };

            return value;
        }

        private static DbCurrency CreateCurrency(string currencyCode)
        {
            var currency = new DbCurrency
            {
                Code = currencyCode,
                Id = Guid.NewGuid(), //TODO: use identity factory
                CreatedDate = DateTime.Now //TODO use utc date
            };

            return currency;
        }

        public async Task<IReadOnlyCollection<Currency>> GetAsync(
            DateTime startDate, DateTime endDate, params string[] codes)
        {
            if (codes == null || endDate < startDate)
            {
                return new Currency[0];
            }

            DbCurrencyValue[] currencyValues;

            using (var context = _conetextFactory.Create())
            {
                var currencyValuesRq = context.DbCurrencyValues
                    .Include(e => e.Currency)
                    .Where(e => startDate <= e.Date && e.Date <= endDate)
                    .Where(e => codes.Contains(e.Currency.Code));

                if (codes.Any())
                {
                    currencyValuesRq = currencyValuesRq
                        .Where(e => codes.Contains(e.Currency.Code));
                }

                currencyValues = await currencyValuesRq.ToArrayAsync();
            }

            var result = currencyValues
                .AsParallel()
                .Select(e => _currecyFactory.Create(e.Currency.Code, e.Amount, e.Rate, e.Date))
                .ToArray();

            return result;
        }
    }
}
