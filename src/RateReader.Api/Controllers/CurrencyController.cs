using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RateReader.Api.AppServices.Builders.Abstractions;
using RateReader.Api.Models;
using RateReader.CurrencyContext.Repositories.Interfaces;

namespace RateReader.Api.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRepository _repository;
        private readonly ApplicationParameters _appParameters;
        private readonly ICurrencyReportResponseBuilder _currencyReportResponseBuilder;
        private readonly ICurrencyRequestVlidator _requestVlidator;

        public CurrencyController(
            ICurrencyRepository repository,
            ApplicationParameters appParameters,
            ICurrencyReportResponseBuilder currencyReportResponseBuilder,
            ICurrencyRequestVlidator requestVlidator)
        {
            _repository = repository;
            _appParameters = appParameters;
            _currencyReportResponseBuilder = currencyReportResponseBuilder;
            _requestVlidator = requestVlidator;
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<CurrencyResponseModel>))]
        public async Task<IActionResult> GetCurrecies(string format, int year, int? month = null)
        {
            if (!_requestVlidator.IsValid(year, month, out var message))
            {
                return BadRequest(new {message});
            }

            var startDate = new DateTime(year, month ?? 1, 1);

            var endDate = month.HasValue
                ? new DateTime(year, month.Value, DateTime.DaysInMonth(year, month.Value))
                : startDate.AddYears(1).AddDays(-1);

            var currencies = await _repository.GetAsync(
                startDate, endDate, _appParameters.CurrenciesCodes);

            return _currencyReportResponseBuilder.Build(startDate, endDate, format, currencies);
        }
    }
}