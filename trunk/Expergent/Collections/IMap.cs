using System;

namespace Expergent.Collections
{
    /// <author>  Peter Lin
    /// 
    /// Why make a map interface that looks identicle to java.util.Map?
    /// The reason is to make it easier to write an optimized HashMap, similar
    /// to what Mark Proctor did for Drools3. By having an interface, it makes
    /// it easier to wrap distributed data grid products like JCache and
    /// JavaSpaces.
    /// 
    /// </author>
    public interface IMap<K, V>
    {
        bool Empty { get; }
        int Count { get; }
        bool ContainsKey(K key);
        V Get(K key);
        V Put(K key, V val);
        V Remove(K key);
        void Clear();
        IEntryIterator<K, V> EntryIterator();
    }
}