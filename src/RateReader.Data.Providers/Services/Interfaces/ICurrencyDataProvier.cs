using System;
using System.Threading.Tasks;

namespace RateReader.Cnb.Providers.Services.Interfaces
{
    public interface ICurrencyDataProvier
    {
        Task GetCurrentCurrencyAsync(DateTime valueOnDate, Func<CnbCurrentCurrency, Task> handler);
        Task GetCurrencyHistoryAsync(int year, Func<CnbCurrencyHistory, Task> handler);
    }
}