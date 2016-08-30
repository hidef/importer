using System;
using System.Threading;

namespace import
{
    public static class Runner
    {
        public class Options
        {
            public int SleepTime { get; set; }
        }

        public static void Run<T>(this IGenerator<T> integrationChain, Options options)
        {
            Console.WriteLine("Running...");
            int messageCount = 0;
            while ( true ) 
            {
                try
                {
                    foreach ( T output in integrationChain.Get() )
                    {
                        Console.WriteLine($"====== Message Count: {messageCount++}");
                        Thread.Sleep(options.SleepTime);
                    }
                }
                catch ( AggregateException ex ) 
                {   
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"ERROR: {ex.InnerException.GetType().Name} {ex.InnerException.Message}");
                    Console.Error.WriteLine($"ERROR - 1: {ex.InnerException.InnerException.GetType().Name} {ex.InnerException.InnerException.Message}");
                }
                catch ( Exception ex )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"ERROR: {ex.GetType().Name} {ex.Message} ");
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Thread.Sleep(options.SleepTime);
            }
        }
    }
}