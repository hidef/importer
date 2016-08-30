using System;
using System.Globalization;
using Nest;

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
    
    [ElasticsearchType(Name = "sale", IdProperty = "Id")]
    public class SaleModel 
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }
        public GeoLocation Location { get; set; }

        public override string ToString()
        {
            return "SaleModel#{ " + (this.Date.ToString("yyyy-MM-dd") + " - " + this.Price.ToString("C", CultureInfo.CurrentCulture) + $" - ({this.Location}) - " + this.Address).Limit(150) + " }";
        }
    }
}