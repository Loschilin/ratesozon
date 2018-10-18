using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RateReader.Api.AppServices.Builders.Abstractions;
using RateReader.CurrencyContext.Entities;

namespace RateReader.Api.AppServices.Builders
{
    public class CurrencyReportResponseBuilder
        : ICurrencyReportResponseBuilder
    {
        private readonly IEnumerable<IConcreteCurrencyReportResponseBuilder> _builders;

        public CurrencyReportResponseBuilder(
            IEnumerable<IConcreteCurrencyReportResponseBuilder> builders)
        {
            _builders = builders;
        }

        public IActionResult Build(DateTime startDate, DateTime endDate, string format, 
            IReadOnlyCollection<Currency> currencies)
        {
            var curentBuilders = _builders.Where(e => e.Allow(format)).ToArray();

            if (curentBuilders.Length == 0)
            {
                return new BadRequestObjectResult($"Incorrect request parameter format = {format}");
            }

            if (curentBuilders.Length > 1)
            {
                throw new ApplicationException($"Found some many builders {string.Join(",",curentBuilders.Select(e=>e.GetType()))}");
            }

            var result = curentBuilders[0].Build(startDate, endDate, currencies);
            return result;
        }
    }
}