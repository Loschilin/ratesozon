using System;
using RateReader.Api.AppServices.Validators.Abstractions;

namespace RateReader.Api.Controllers
{
    public class WrongDateVlidator: ICurrencyRequestConcreteValidator
    {
        public bool IsValid(int year, int? month, out string message)
        {
            message = string.Empty;

            if (year < DateTime.MinValue.Year
                || year > DateTime.MaxValue.Year)
            {
                message = "Incorrect year parameter";
                return false;
            }

            if (month < 1 || month > 12)
            {
                message = "Incorrect month parameter";
                return false;
            }

            return true;
        }
    }
}