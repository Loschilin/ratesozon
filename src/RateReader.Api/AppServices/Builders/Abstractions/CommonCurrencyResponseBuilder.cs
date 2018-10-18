using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RateReader.Api.AppServices.Builders.Abstractions;
using RateReader.Api.Models;
using RateReader.CurrencyContext.Entities;

namespace RateReader.Api.Controllers
{
    public abstract class CommonCurrencyResponseBuilder
        : IConcreteCurrencyReportResponseBuilder
    {
        private readonly ApplicationParameters _parameters;
        protected abstract string Format { get; }

        protected CommonCurrencyResponseBuilder(ApplicationParameters parameters)
        {
            _parameters = parameters;
        }

        public bool Allow(string format)
        {
            return format.Equals(Format, StringComparison.InvariantCultureIgnoreCase);
        }

        public IActionResult Build(DateTime startDate, DateTime endDate, IReadOnlyCollection<Currency> currencies)
        {
            var model = CreateModel(startDate, endDate, currencies);
            var result = BuildInternal(model);
            return result;
        }

        private IEnumerable<CurrencyResponseModel> CreateModel(
            DateTime startDate, DateTime endDate, IReadOnlyCollection<Currency> currencies)
        {
            var result = new List<CurrencyResponseModel>();

            while (startDate < endDate)
            {
                var date = new DateTime(startDate.Year, startDate.Month, 1);
                var currenciesByDate = currencies.Where(e =>
                    date <= e.Date 
                    && e.Date <= date.AddDays(DateTime.DaysInMonth(date.Year, date.Month))
                ).ToArray();

                var itemModel = new CurrencyResponseModel
                {
                    Year = startDate.Year,
                    Month = startDate.Month,
                    WeekPeriods = CreateWeekPeriods(startDate.Year, startDate.Month)
                };

                foreach (var currencyWeekPeriodModel in itemModel.WeekPeriods)
                {
                    var startWeekDate = new DateTime(
                        startDate.Year, startDate.Month, currencyWeekPeriodModel.StartDay);

                    var endWeekDate = new DateTime(
                        startDate.Year, startDate.Month, currencyWeekPeriodModel.EndDay);

                    var currenciesInPeriod = currenciesByDate
                        .Where(e => startWeekDate <= e.Date && e.Date <= endWeekDate)
                        .GroupBy(e=>e.Code)
                        .ToDictionary(e=>e.Key, v => v.ToArray());

                    foreach (var code in _parameters.CurrenciesCodes)
                    {
                        var currenciesByCode = currenciesInPeriod.ContainsKey(code) 
                            ? currenciesInPeriod[code] 
                            : new Currency[0];

                        var collection = new CurrencyCollection();
                        collection.AddRange(currenciesByCode);
                        
                        var model = new CurrencyResponseValueModel
                        {
                            Max = collection.Max(),
                            Min = collection.Min(),
                            Median = collection.Median(),
                            Code = code
                        };
                        currencyWeekPeriodModel.Currencies.Add(model);
                    }

                }

                result.Add(itemModel);
                startDate = startDate.AddMonths(1);
            }

            return result.OrderBy(e=>e.Year).ThenBy(e=>e.Month);
        }

        private static readonly DayOfWeek[] IgnoreDays
            = {DayOfWeek.Sunday, DayOfWeek.Saturday};

        private static List<CurrencyWeekPeriodModel> CreateWeekPeriods(int year, int month)
        {
            var result = new List<CurrencyWeekPeriodModel>();
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            CurrencyWeekPeriodModel model = null;
            while (startDate < endDate)
            {
                if (model == null && !IgnoreDays.Contains(startDate.DayOfWeek))
                {
                    model = new CurrencyWeekPeriodModel
                    {
                        StartDay = startDate.Day
                    };
                }

                if ((startDate.DayOfWeek == DayOfWeek.Friday
                     || startDate == endDate) && model != null)
                {
                    model.EndDay = startDate.Day;
                    result.Add(model);
                    model = null;
                }

                startDate = startDate.AddDays(1);
            }

            return result;

        }

        protected abstract IActionResult BuildInternal(
            IEnumerable<CurrencyResponseModel> model);
    }
}