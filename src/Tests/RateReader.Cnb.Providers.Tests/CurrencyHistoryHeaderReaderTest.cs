using RateReader.Cnb.Providers.Services;
using Xunit;

namespace RateReader.Cnb.Providers.Tests
{
    public class CurrencyHistoryHeaderReaderTest
    {
        private readonly CurrencyHistoryHeaderReader _reader;

        public CurrencyHistoryHeaderReaderTest()
        {
            _reader = new CurrencyHistoryHeaderReader();
        }

        [InlineData("Date|1 AUD|2 BGN")]
        [Theory]
        public void TyrReadHeaders(string line)
        {
            var readResult = _reader.TyrReadHeaders(line, out var headers);

            Assert.True(readResult, "Headers not readed");
            Assert.NotNull(headers);
            Assert.Equal(2, headers.Length);

            Assert.Equal("AUD", headers[0].Code);
            Assert.Equal(1, headers[0].Amount);

            Assert.Equal("BGN", headers[1].Code);
            Assert.Equal(2, headers[1].Amount);
        }
    }
}