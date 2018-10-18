namespace RateReader.Cnb.Providers.Services.Interfaces
{
    internal interface ICurrentCurrencyReader
    {
        bool TryReadCurrency(string line, out CnbCurrentCurrency currency);
    }
}