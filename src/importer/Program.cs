using System;

namespace import
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var integration = new From()
                .FromCSV("/Users/uatec/data", "*.csv")
                .TransformTo<string[], string>((s) => s[3])
                .TransformTo<string, PriceModel>((s) => new PriceModel { Text = s })
                .ToElasticSearch("my-index");

            Console.WriteLine("-----");

            integration.Run();
        }
    }

}
