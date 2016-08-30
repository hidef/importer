using System.Collections.Generic;

namespace import
{
    public interface IGenerator<T>
    {
        IEnumerable<T> Get();
    }
}