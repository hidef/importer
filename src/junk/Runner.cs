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
                    Console.WriteLine($"ERROR - 1: {ex.InnerException.InnerException.GetType().Name} {ex.InnerException.InnerException.Message}");
                }
                catch ( Exception ex )
                {
                    Console.WriteLine($"ERROR: {ex.GetType().Name} {ex.Message}");
                }
            }
            Console.WriteLine("Done.");
        }
    }
    
}