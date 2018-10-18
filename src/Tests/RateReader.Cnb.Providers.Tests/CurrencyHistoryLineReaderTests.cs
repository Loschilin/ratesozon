using System;
using RateReader.Cnb.Providers.Services;
using Xunit;

namespace RateReader.Cnb.Providers.Tests
{
    public class CurrencyHistoryLineReaderTests
    {
        private readonly CurrencyHistoryLineReader _reader;

        public CurrencyHistoryLineReaderTests()
        {
            _reader = new CurrencyHistoryLineReader(
                new EmptyLogger<CurrencyHistoryLineReader>());
        }

        [InlineData("02.Jan 2018|16.540|13.033")]
        [Theory]
        public void TryReadTest(string line)
        {
            var readResult = _reader.TryRead(line, out var date, out var values);
            Assert.True(readResult, "Headers not readed");
            Assert.NotNull(values);
            Assert.Equal(2, values.Length);

            Assert.Equal(16.540m, values[0]);
            Assert.Equal(13.033m, values[1]);
            Assert.Equal(date, new DateTime(2018,01,02));
        }
    }
}