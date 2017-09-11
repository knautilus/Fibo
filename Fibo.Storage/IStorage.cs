namespace Fibo.Storage
{
    public interface IStorage<TKey, TValue>
    {
        bool SetValue(TKey key, TValue value);
        TValue GetValue(TKey key);
    }
}
