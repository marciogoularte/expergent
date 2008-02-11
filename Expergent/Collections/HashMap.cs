using System;

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
    public class HashMap : AbstractMap
    {
        public HashMap() : base(101, 0.75f)
        {
        }

        public override bool ContainsKey(Object key)
        {
            return Get(key) != null;
        }

        public override Object Get(Object key)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, table.Length);

            ObjectEntry current = (ObjectEntry) table[index];
            while (current != null)
            {
                if (hashCode == current.GetHashCode() && comparator.equal(key, current.Key))
                {
                    return current.Value;
                }
                current = (ObjectEntry) current.Next;
            }
            return null;
        }

        /// <summary> 
        /// </summary>
        public override Object Put(Object key, Object value_Renamed)
        {
            return Put(key, value_Renamed, false);
        }

        public virtual Object Put(Object key, Object value_Renamed, bool checkExists)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, table.Length);

            // scan the linked entries to see if it exists
            if (checkExists)
            {
                IEntry current = table[index];
                while (current != null)
                {
                    if (hashCode == current.GetHashCode() && key.Equals(current.Key))
                    {
                        Object oldValue = current.Value;
                        current.Value = value_Renamed;
                        return oldValue;
                    }
                    current = (ObjectEntry) current.Next;
                }
            }

            // create a new ObjectEntry
            ObjectEntry entry = new ObjectEntry(key, value_Renamed, hashCode);
            // in case there is already an entry with the same hashcode,
            // set it as the next entry for the new one. this means the older
            // entries are pushed down the bucket
            entry.Next = table[index];
            table[index] = entry;

            if (size_Renamed_Field++ >= threshold)
            {
                resize(2*table.Length);
            }
            return null;
        }

        public override Object Remove(Object key)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, table.Length);

            ObjectEntry previous = (ObjectEntry) table[index];
            ObjectEntry current = previous;
            while (current != null)
            {
                ObjectEntry next = (ObjectEntry) current.Next;
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
                    size_Renamed_Field--;
                    return current.Value;
                }
                previous = current;
                current = next;
            }
            return null;
        }
    }
}