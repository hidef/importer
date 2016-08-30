using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace import 
{
    public static class FileSystem 
    {
        public static IGenerator<string> FromFileSystem(this From from, string path, string mask) 
        {
            Console.WriteLine("Register: Read file systaem");
            return new FileSystemSource(path, mask);
        }
    } 
    
    public class FileSystemSource : IGenerator<string>
    {
        private readonly string _path;
        private readonly string _mask;
        private string[] fileData = new string[0];
        private int currentLine = -1;

        public FileSystemSource(string path, string mask)
        {
            _path = path;
            _mask = mask;
        }

        public IEnumerable<string> Get()
        {
            while ( true )
            {
                currentLine++;
                Console.WriteLine($"Reading Line: {currentLine}/{fileData.Length}");

                while ( currentLine >= fileData.Length || fileData.Length == 0) 
                {
                    Console.WriteLine("Moving to next file");
                    moveToNextFile();

                    continue;
                }
                
                yield return fileData[currentLine];
            }
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