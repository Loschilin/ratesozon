using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RateReader.CurrencyContext.Entities;

namespace RateReader.Api.AppServices.Builders.Abstractions
{
    public interface ICurrencyReportResponseBuilder
    {
        IActionResult Build(DateTime startDate, DateTime endDate, string format, 
            IReadOnlyCollection<Currency> currencies);
    }
}