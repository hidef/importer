namespace import
{
    public class Source<T> : IGenerator<T>
    {
        public virtual T Get()
        {
            return default(T);
        }
    }
}