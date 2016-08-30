using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace import 
{
    public static class CSV 
    {
        public static Transformation<string[]> ParseCSV(this IGenerator<string> predecessor)
        {
            Console.WriteLine($"Register: CSV Parser");
            return new CSVParser(predecessor);
        }
    } 
    
    public class CSVParser : Transformation<string[]>
    {
        private readonly IGenerator<string> _predecessor;
        
        public CSVParser(IGenerator<string> predecessor)
        {
            _predecessor = predecessor;
        }

        public IEnumerable<string[]> Get()
        {
            foreach ( string line in _predecessor.Get())
            {
                string[] fields = line.Split(',').Select(s => s.Trim('"')).ToArray();
                yield return fields;
            }
        }
    }
}