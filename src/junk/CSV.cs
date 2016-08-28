using System;

namespace import 
{
    public static class CSV 
    {
        public static Source<string> FromCSV(this From from, string path, string mask) 
        {
            Console.WriteLine("Register: Read CSV");
            return new DummySource();
        }
    } 

    public class DummySource : Source<string>
    {
        private int x = 0;
        public override string Get()
        {
            string data = x++.ToString();

            Console.WriteLine($"FROM__: {data}");
            
            return data;
        }
    }
}