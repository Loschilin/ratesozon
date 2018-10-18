using System;
using RateReader.Api.AppServices.Validators.Abstractions;

namespace RateReader.Api.AppServices.Validators
{
    public class WithCurrentDateValidator : ICurrencyRequestConcreteValidator
    {
        public bool IsValid(int year, int? month, out string message)
        {
            message = string.Empty;
            month = month ?? 1;

            var date = new DateTime(year, month.Value, 1);
            if (date > DateTime.Today)
            {
                message = "Selected date is greater than current";
                return false;
            }

            return true;
        }
    }
}