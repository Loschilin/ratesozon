using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateReader.Api.AppServices.Builders;
using RateReader.Api.AppServices.Builders.Abstractions;
using RateReader.Api.AppServices.Validators;
using RateReader.Api.AppServices.Validators.Abstractions;
using RateReader.Api.Controllers;
using RateReader.CurrencyContext;
using RateReader.Data;

namespace RateReader.Api
{
    public static class Config
    {
        public static IServiceCollection AddAppServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var appParams = configuration
                .GetSection("ApplicationParameters")
                .Get<ApplicationParameters>();

            if (appParams == null)
            {
                throw new ApplicationException("Application parameters is empty");
            }

            var dataConnectionString = configuration.GetConnectionString("SqlDataConnection");

            services.AddSingleton(appParams)
                .AddSingleton<ICurrencyReportResponseBuilder, CurrencyReportResponseBuilder>()
                .AddSingleton<IConcreteCurrencyReportResponseBuilder, JsonCurrencyResponseBuilder>()
                .AddSingleton<IConcreteCurrencyReportResponseBuilder, TxtCurrencyResponseBuilder>()
                
                .AddSingleton<ICurrencyRequestVlidator, CurrencyRequestVlidator>()
                .AddSingleton<ICurrencyRequestConcreteValidator, WithCurrentDateValidator>()
                .AddSingleton<ICurrencyRequestConcreteValidator, WrongDateVlidator>()

                .AddCurrencyDataContext(options => { options.ConnectionString = dataConnectionString; })
                .AddCurrencyContext();

            return services;
        }
    }
}