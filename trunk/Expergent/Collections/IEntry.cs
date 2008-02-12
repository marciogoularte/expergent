namespace Expergent.Collections
{
    public interface IEntry<K, V>
    {
        K Key { get; }
        IEntry<K, V> Next { get; set; }
        V Value { get; set; }
        void Clear();
    }
}