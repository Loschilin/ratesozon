using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RateReader.CurrencyContext.Entities;

namespace RateReader.Api.AppServices.Builders.Abstractions
{
    public interface IConcreteCurrencyReportResponseBuilder
    {
        bool Allow(string format);
        IActionResult Build(DateTime startDate, DateTime endDate, 
            IReadOnlyCollection<Currency> currencies);
    }
}