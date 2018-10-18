using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using RateReader.Cnb.Providers.Services.Interfaces;

[assembly: InternalsVisibleTo("RateReader.Cnb.Providers.Tests")]

namespace RateReader.Cnb.Providers.Services
{
    internal class CurrencyHistoryHeaderReader : ICurrencyHistoryHeaderReader
    {
        private static readonly Regex HeaderResponseRegex
            = new Regex("^Date(\\|[0-9]{1,}\\s[A-Z]{1,}){1,}$");

        public bool TyrReadHeaders(string line, out ResponseHistoryHeader[] headers)
        {
            if (!HeaderResponseRegex.IsMatch(line))
            {
                headers = null;
                return false;
            }

            headers = line
                .Split('|')
                .Skip(1)
                .Select(e =>
                {
                    var cdata = e.Split(' ');
                    return new ResponseHistoryHeader
                    {
                        Amount = int.Parse(cdata[0]),
                        Code = cdata[1]
                    };
                }).ToArray();

            return true;
        }
    }
}