
namespace Expergent.Collections
{
    public interface IEntryIterator<K, V>
    {
        IEntry<K,V> Next();
    }
}