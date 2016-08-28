using System;

namespace import
{
    public static class Runner
    {
        public static void Run<T>(this Destination<T> destination)
        {
            Console.WriteLine("Running...");
            while ( true ) 
            {
                destination.Get();
            }
            Console.WriteLine("Done.");
        }
    }
    
}