using System;

namespace import
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var integration = new From()
                .FromCSV("~/data", "*.csv")
                .TransformTo<string, string>((s) => s.ToLower())
                .TransformTo<string, PriceModel>((s) => new PriceModel { Text = s })
                .ToElasticSearch("my-index");

            Console.WriteLine("-----");

            integration.Run();
        }
    }

}
