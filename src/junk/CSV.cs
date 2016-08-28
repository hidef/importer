using System;
using System.IO;

namespace import 
{
    public static class CSV 
    {
        public static Source<string[]> FromCSV(this From from, string path, string mask) 
        {
            Console.WriteLine("Register: Read CSV");
            return new CSVSource(path, mask);
        }
    } 
    
    public class CSVSource : Source<string[]>
    {
        private readonly string _path;
        private readonly string _mask;
        private string[] fileData = new string[0];
        private int currentLine = -1;

        public CSVSource(string path, string mask)
        {
            _path = path;
            _mask = mask;
        }

        public override string[] Get()
        {
            currentLine++;
            Console.WriteLine($"CSV Line: {currentLine}");
            Console.WriteLine($"\tLines: {fileData.Length}");

            while ( currentLine >= fileData.Length || fileData.Length == 0) 
            {
                Console.WriteLine("Moving to next file");
                moveToNextFile();

                Console.WriteLine($"CSV Line: {currentLine}");
                Console.WriteLine($"\tLines: {fileData.Length}");
            }
            
            string[] fields = fileData[currentLine].Split(',');
            return fields;
        }

        private void moveToNextFile()
        {
            // find next file

            string[] availableFiles = null;
            
            while (availableFiles == null || availableFiles.Length == 0 )
            {
                System.Threading.Thread.Sleep(500); // configure me
                availableFiles = System.IO.Directory.GetFiles(_path, _mask);
            }

            string chosenFile = availableFiles[0];

            Console.WriteLine($"Reading File: {chosenFile}");

            fileData = File.ReadAllLines(chosenFile);
            Console.WriteLine($"Read {fileData.Length} lines.");
            currentLine = 0;

            System.IO.File.Move(chosenFile, chosenFile + ".done");
        }
    }
}