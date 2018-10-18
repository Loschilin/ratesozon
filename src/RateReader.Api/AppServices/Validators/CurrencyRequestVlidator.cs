using System.Collections.Generic;
using RateReader.Api.AppServices.Validators.Abstractions;
using RateReader.Api.Controllers;

namespace RateReader.Api.AppServices.Validators
{
    public class CurrencyRequestVlidator : ICurrencyRequestVlidator
    {
        private readonly IEnumerable<ICurrencyRequestConcreteValidator> _validators;

        public CurrencyRequestVlidator(IEnumerable<ICurrencyRequestConcreteValidator> validators)
        {
            _validators = validators;
        }

        public bool IsValid(int year, int? month, out string message)
        {
            message = string.Empty;
            foreach (var validator in _validators)
            {
                if (!validator.IsValid(year, month, out message))
                {
                    return false;
                }
            }

            return true;
        }
    }
}