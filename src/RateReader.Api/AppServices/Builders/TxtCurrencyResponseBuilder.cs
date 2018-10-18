using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RateReader.Api.Controllers;
using RateReader.Api.Models;

namespace RateReader.Api.AppServices.Builders
{
    public class TxtCurrencyResponseBuilder: CommonCurrencyResponseBuilder
    {
        protected override string Format => "txt";

        public TxtCurrencyResponseBuilder(ApplicationParameters parameters) 
            : base(parameters)
        {
        }

        protected override IActionResult BuildInternal(IEnumerable<CurrencyResponseModel> model)
        {
            var mfi = new DateTimeFormatInfo();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            foreach (var currencyModel in model.OrderBy(e=>e.Year).ThenBy(e=>e.Year))
            {
                writer.WriteLine($"Year: {currencyModel.Year}; month: {mfi.GetMonthName(currencyModel.Month)};");
                writer.WriteLine("Week periods:");
                foreach (var weekPeriod in currencyModel.WeekPeriods)
                {
                    var currencyString = string.Join("; ",
                        weekPeriod.Currencies.Select(e => $"{e.Code} - max:{e.Max}, min:{e.Min}, median:{e.Median}"));

                    writer.WriteLine($"{weekPeriod.StartDay}...{weekPeriod.EndDay}: {currencyString};");
                }
                writer.WriteLine();
            }

            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "text/plain");
        }
    }
}