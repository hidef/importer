using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace import 
{
    public static class FileSystem 
    {
        public static IGenerator<string> FromResilientFileSystem(this From from, string path, string mask) 
        {
            Console.WriteLine("Register: Read resilient file system");
            return new ResilientFileSystemSource(path, mask);
        }

        public static IGenerator<string> FromFileSystem(this From from, string path, string mask) 
        {
            Console.WriteLine("Register: Read file system");
            return new FileSystemSource(path, mask);
        }
    } 

    public enum Status
    {
        Shiny = 0,
        InProgress,
        Done,
        Failed
    }

    public class FileInfo
    {
        public string WorkingFileName { get; set; }
        public string OriginalFileName { get; set; }
        public Status Status { get; set; }
    }
    
    public class ResilientFileSystemSource : IGenerator<string> 
    {
        private readonly string _path;
        private readonly string _mask;
        private string[] fileData = new string[0];
        private int currentLine = -1;

        public ResilientFileSystemSource(string path, string mask)
        {
            _path = path;
            _mask = mask;
        }

        public IEnumerable<string> Get()
        {
            //foreach file
            foreach ( FileInfo file in enumerateFiles())
            {
                Console.WriteLine($"Found file: {file.OriginalFileName}");
                long pointer = 0;
                // if necessary 
                if ( file.Status == Status.Shiny ) 
                {
                    //     move it to .tmp
                    beginFile(file);
                    setPointer(file, pointer);
                }
                // try read pointer
                pointer = getPointer(file);
                // do while (!eof)
                //     readline
                foreach ( string line in File.ReadLines(file.WorkingFileName))
                {
                    //     save pointer
                    setPointer(file, pointer + (long) line.Length);
                    //     return data
                    yield return line;
                    // od
                }
                // move it to .done
                endFile(file);
            }
        }

        private void beginFile(FileInfo file)
        {
            File.Move(file.WorkingFileName, file.OriginalFileName + ".tmp");
            file.WorkingFileName = file.OriginalFileName + ".tmp";
            file.Status = Status.InProgress;
        }

        private long getPointer(FileInfo file)
        {
            string rawPointerValue = File.ReadAllText(file.OriginalFileName + ".ptr");
            return long.Parse(rawPointerValue);
        }

        private void setPointer(FileInfo file, long v)
        {
            File.WriteAllText(file.OriginalFileName + ".ptr", v.ToString());
        }

        private void endFile(FileInfo file)
        {
            File.Move(file.WorkingFileName, file.OriginalFileName + ".done");
            file.WorkingFileName = file.OriginalFileName + ".done";
            file.Status = Status.Done;
        }

        private IEnumerable<FileInfo> enumerateFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(_path);
            while ( true ) 
            {
                FileInfo foundFile = 
                    directory.EnumerateFileSystemInfos(_mask + ".tmp")
                    .Union(directory.EnumerateFileSystemInfos(_mask))
                    .Where(f => !new []{ "done", "ptr" }.Contains(f.Extension))
                    .Select(f => new FileInfo {
                        OriginalFileName = tidyFileName(f.FullName),
                        WorkingFileName = f.FullName,
                        Status = getStatusFromExtension(f.Extension),
                    })
                    .FirstOrDefault();

                if ( foundFile != null ) 
                {
                    yield return foundFile;
                } 
                else 
                {
                    Console.WriteLine("file not found");
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        private string tidyFileName(string fullName)
        {
            if ( fullName.EndsWith("tmp") )
            {
                return fullName.Substring(0, fullName.Length - 4);
            }
            return fullName;
        }

        private Status getStatusFromExtension(string extension)
        {
            switch ( extension )
            {
                case ".done":
                    return Status.Done;
                case ".tmp": 
                    return Status.InProgress;
                case ".failed":
                    return Status.Failed;
                default:
                    return Status.Shiny;
            }
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