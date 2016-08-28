using System;

namespace import
{
    public static class Runner
    {
        public static void Run<T>(this Destination<T> destination)
        {
            Console.WriteLine("Running...");
            int messageCount = 0;
            while ( true ) 
            {
                try
                {
                    destination.Get();
                    Console.WriteLine($"Message Count: {messageCount++}");
                }
                catch ( AggregateException ex ) 
                {
                    Console.WriteLine($"ERROR: {ex.InnerException.GetType().Name} {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("Done.");
        }
    }
    
}