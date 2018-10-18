namespace RateReader.Cnb.Providers.Services.Interfaces
{
    internal interface ICurrencyHistoryHeaderReader
    {
        bool TyrReadHeaders(string line, out ResponseHistoryHeader[] headers);
    }
}