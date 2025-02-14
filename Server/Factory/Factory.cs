namespace MyFactory
{
    public class Factory<K, D, T> where K : notnull
    {
        private readonly Dictionary<K, Func<D?, T>> creators = new();

        public void Add(K key, Func<D?, T> method)
        {
            creators[key] = method;
        }

        public virtual T Create(K key, D? param)
        {
            if (!creators.TryGetValue(key, out var func))
            {
                throw new ArgumentException($"No command found for key {key}");
            }
            return func(param);
        }
    }
}
