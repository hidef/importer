using System;

namespace import
{
    public static class ElasticSearch 
    {
        public static Destination<T> ToElasticSearch<T>(this Transformation<T> predecessor, string indexName)
        {
            Console.WriteLine($"Register: Writing to {indexName}");
            return new DummyDestination<T>(predecessor);;
        }
    }


    public class DummyDestination<T> : Destination<T>
    {
        private readonly Transformation<T> _predecessor;
        
        public DummyDestination(Transformation<T> predecessor)
        {
            _predecessor = predecessor;
        }

        public T Get()
        {
            T data = _predecessor.Get();

            Console.WriteLine($"OUTPUT: {data}");
            return data;
        }
    }
}