using System;

namespace import
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var integration = new From()
                .FromCSV("/Users/uatec/data", "*.csv")
                .TransformTo<string[], SaleModel>((s) => new SaleModel {
                    Date = DateTime.Parse(s[2]),
                    Address = s[7] + ", " + s[9] + ", " + s[11]  + ", " + s[12]  + ", " + s[13]  + ", " + s[3],
                    Price = int.Parse(s[1])
                })
                .ToElasticSearch("my-index");

            Console.WriteLine("-----");

            integration.Run();
        }
    }

}
