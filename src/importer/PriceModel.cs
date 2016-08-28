using System;
using System.Globalization;

namespace import
{

    public static class StringExtensions 
    {
        public static string Limit(this string self, int limit)
        {
            if ( self.Length >= limit ) {
                return self.Substring(0, limit);
            }
            return self;
        }
    }

    public class SaleModel 
    {
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }

        public override string ToString()
        {
            return (this.Date.ToString("yyyy-MM-dd") + " - " + this.Price.ToString("C", CultureInfo.CurrentCulture) + " - " + this.Address).Limit(150);
        }
    }
}