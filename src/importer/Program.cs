using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using Nest;

namespace import
{

    public class GeocodeResult
    {
        public Geometry Geometry { get; set; }
    }

    public class Geometry 
    {
        public Location Location { get; set; }
    }
    public class Location { 
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class GeocodeResponse
    {
        public List<GeocodeResult> Results { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();

            var integration = new From()
                .FromResilientFileSystem("/Users/uatec/data", "*.csv")
                .ParseCSV()
                .TransformTo<string[], SaleModel>((s) => {
                    var sm = new SaleModel {
                        Id = s[0],
                        Date = DateTime.Parse(s[2]),
                        Address = s[7] + ", " + s[9] + ", " + s[11]  + ", " + s[12]  + ", " + s[13]  + ", " + s[3],
                        Price = int.Parse(s[1])
                    };

                    var response = httpClient.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={sm.Address}").Result;
                    GeocodeResponse responseObject = JsonConvert.DeserializeObject<GeocodeResponse>(
                        response.Content.ReadAsStringAsync().Result);

                    if ( responseObject.Results.Count == 0 )
                    {
                        throw new Exception("Unable to geocode address. No results found.");    
                    }

                    sm.Location = new GeoLocation(
                        responseObject.Results[0].Geometry.Location.Lat,
                        responseObject.Results[0].Geometry.Location.Lng
                    );

                    return sm;
                })
                .ToElasticSearch(new Uri("http://localhost:9201"), "landregistry");
                
            Console.WriteLine("-----");

            integration.Run(new Runner.Options {
                SleepTime = 10
            });
        }
    }

}
