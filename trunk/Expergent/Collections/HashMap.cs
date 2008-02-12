namespace Expergent.Collections
{
    /// <author>  Peter Lin
    /// 
    /// A basic HashMap implementation inspired by Mark's HashTable for
    /// Drools3. The main difference is this HashMap tries to be compatable
    /// with java.util.HashMap. This means it's not as stripped down as
    /// the super optimized HashTable mark wrote for drools. Hopefully the
    /// extra stuff doesn't make too big of a difference.
    /// 
    /// </author>
    public class HashMap<K, V> : AbstractMap<K, V>
    {
        public HashMap() : base(101, 0.75f)
        {
        }

        public override bool ContainsKey(K key)
        {
            return Get(key).Equals(null) == false;
        }

        public override V Get(K key)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, table.Length);

            ObjectEntry<K, V> current = (ObjectEntry<K, V>) table[index];
            while (current != null)
            {
                if (hashCode == current.GetHashCode() && comparator.equal(key, current.Key))
                {
                    return current.Value;
                }
                current = (ObjectEntry<K, V>) current.Next;
            }
            return default(V);
        }

        /// <summary> 
        /// </summary>
        public override V Put(K key, V value_Renamed)
        {
            return Put(key, value_Renamed, false);
        }

        public virtual V Put(K key, V value_Renamed, bool checkExists)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, table.Length);

            // scan the linked entries to see if it exists
            if (checkExists)
            {
                IEntry<K, V> current = table[index];
                while (current != null)
                {
                    if (hashCode == current.GetHashCode() && key.Equals(current.Key))
                    {
                        V oldValue = current.Value;
                        current.Value = value_Renamed;
                        return oldValue;
                    }
                    current = (ObjectEntry<K, V>) current.Next;
                }
            }

            // create a new ObjectEntry
            ObjectEntry<K, V> entry = new ObjectEntry<K, V>(key, value_Renamed, hashCode);
            // in case there is already an entry with the same hashcode,
            // set it as the next entry for the new one. this means the older
            // entries are pushed down the bucket
            entry.Next = table[index];
            table[index] = entry;

            if (cnt++ >= threshold)
            {
                resize(2*table.Length);
            }
            return default(V);
        }

        public override V Remove(K key)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, table.Length);

            ObjectEntry<K, V> previous = (ObjectEntry<K, V>) table[index];
            ObjectEntry<K, V> current = previous;
            while (current != null)
            {
                ObjectEntry<K, V> next = (ObjectEntry<K, V>) current.Next;
                if (hashCode == current.GetHashCode() && comparator.equal(key, current.Key))
                {
                    if (previous == current)
                    {
                        table[index] = next;
                    }
                    else
                    {
                        previous.Next = next;
                    }
                    current.Next = null;
                    cnt--;
                    return current.Value;
                }
                previous = current;
                current = next;
            }
            return default(V);
        }
    }
}