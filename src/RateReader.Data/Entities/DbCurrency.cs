using System;
using System.Collections.Generic;

namespace RateReader.Data.Entities
{
    public class DbCurrency
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<DbCurrencyValue> Values { get; set; }
    }
}
