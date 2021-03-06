using System;
using System.Collections.Generic;

namespace import
{
    public interface Transformation<T> : IGenerator<T>
    {
    }

    public static class Transformer
    {
        public static Transformation<T2> TransformTo<T1, T2>(this IGenerator<T1> predecessor, Func<T1, T2> transform)
        {
            Console.WriteLine($"Register: Transform to {typeof (T1)} => {typeof (T2)}");
            return new LambdaTransformer<T1, T2>(predecessor, transform);
        }
    }

    public class LambdaTransformer<T1, T2> : Transformation<T2>
    {
        private readonly IGenerator<T1> _predecessor;
        private readonly Func<T1, T2> _transformer;
        
        public LambdaTransformer(IGenerator<T1> predecessor, Func<T1, T2> transformer)
        {
            _predecessor = predecessor;
            _transformer = transformer;
        }

        public IEnumerable<T2> Get()
        {
            foreach (T1 data in _predecessor.Get())
            {
                T2 output = _transformer(data);

                Console.WriteLine($"TRANS_: {data} => {output}");

                yield return output;
            }
        }
    }
}