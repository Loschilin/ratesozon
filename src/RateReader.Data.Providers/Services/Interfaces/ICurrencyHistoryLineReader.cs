using System;

namespace RateReader.Cnb.Providers.Services.Interfaces
{
    internal interface ICurrencyHistoryLineReader
    {
        bool TryRead(string line, out DateTime date, out decimal[] values);
    }
}