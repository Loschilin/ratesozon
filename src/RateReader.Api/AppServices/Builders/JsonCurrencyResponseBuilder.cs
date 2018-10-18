using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RateReader.Api.Controllers;
using RateReader.Api.Models;

namespace RateReader.Api.AppServices.Builders
{
    public class JsonCurrencyResponseBuilder: CommonCurrencyResponseBuilder
    {
        protected override string Format => "json";

        public JsonCurrencyResponseBuilder(ApplicationParameters parameters) 
            : base(parameters)
        {
        }

        protected override IActionResult BuildInternal(IEnumerable<CurrencyResponseModel> model)
        {
            return new OkObjectResult(model);
        }
    }
}