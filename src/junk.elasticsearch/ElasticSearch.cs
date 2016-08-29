using System;
using Nest;
using Newtonsoft.Json;

namespace import
{
    public static class ElasticSearch 
    {
        public static Destination<T> ToElasticSearch<T>(this Transformation<T> predecessor, Uri node, string indexName) where T: class
        {
            Console.WriteLine($"Register: Writing to {indexName}");
            return new ElasticSearchDestination<T>(predecessor, node, indexName);
        }
    }

    public class ElasticSearchDestination<T> : Destination<T> where T: class
    {
        private readonly Transformation<T> _predecessor;
        private readonly ElasticClient _client;
        private readonly string _index;
        
        public ElasticSearchDestination(Transformation<T> predecessor, Uri node, string index)
        {
            _predecessor = predecessor;
            _index = index;

            var settings = new ConnectionSettings(node);
            _client = new ElasticClient(settings);
            

            var elasticResponseResult = _client.IndexExists(index);

            // If the index does not already exist, just create it. Otherwise we need to diff it.
            if (!elasticResponseResult.Exists)
            {
                _client.CreateIndex(index);
                _client.Map<T>(ms => ms.AutoMap());
            }


        }

        public T Get()
        {
            T data = _predecessor.Get();

            var response = _client.Index(data, idx => idx.Index(_index));
            Console.WriteLine("Creating document in " + _index);

            if ( !response.Created ) 
            {
                throw new Exception($"Creation of ElasticSearch document failed. - {JsonConvert.SerializeObject(response)}");
            }

            Console.WriteLine($"OUTPUT: {data} - {JsonConvert.SerializeObject(response)}");

            return data;
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