using System;

namespace Expergent.Collections
{
    public abstract class AbstractMap<K, V> : IMap<K, V>
    {
        internal const int MAX_CAPACITY = 1 << 30;

        protected internal ObjectComparator comparator;

        private EntryIterator<K, V> eIterator;
        protected internal float loadFactor;
        protected internal int cnt;
        protected internal IEntry<K, V>[] table;
        protected internal int threshold;

        /// <summary> 
        /// </summary>
        public AbstractMap(int capacity, float factor)
        {
            loadFactor = factor;
            threshold = (int) (capacity*loadFactor);
            table = new IEntry<K, V>[capacity];
            comparator = EqualityEquals.Instance;
        }

        #region IMap<K,V> Members

        public virtual bool Empty
        {
            get { return cnt == 0; }
        }

        public virtual int Count
        {
            get { return cnt; }
        }

        public abstract bool ContainsKey(K key);

        public abstract V Get(K key);

        public abstract V Put(K key, V value_Renamed);

        public abstract V Remove(K key);

        /// <summary> clear aggressively clears the table and nulls the
        /// references.
        /// </summary>
        public virtual void Clear()
        {
            for (int idx = 0; idx < table.Length; idx++)
            {
                if (table[idx] != null)
                {
                    // we clear the table
                    IEntry<K, V> e = table[idx];
                    e.Clear();
                }
            }
            table = null;
            eIterator.reset();
        }

        public virtual IEntryIterator<K, V> EntryIterator()
        {
            if (eIterator == null)
            {
                eIterator = new EntryIterator<K, V>(this);
            }
            eIterator.reset();
            return eIterator;
        }

        #endregion

        protected internal virtual int indexOf(int hashCode, int dataSize)
        {
            return hashCode & (dataSize - 1);
        }

        protected internal virtual void resize(int newCapacity)
        {
            IEntry<K, V>[] oldTable = table;
            int oldCapacity = oldTable.Length;
            if (oldCapacity == MAX_CAPACITY)
            {
                threshold = Int32.MaxValue;
                return;
            }

            IEntry<K, V>[] newTable = new IEntry<K, V>[newCapacity];

            for (int i = 0; i < table.Length; i++)
            {
                IEntry<K, V> entry = table[i];
                if (entry == null)
                {
                    continue;
                }
                table[i] = null;
                IEntry<K, V> next = null;
                while (entry != null)
                {
                    next = entry.Next;

                    int index = indexOf(entry.GetHashCode(), newTable.Length);
                    entry.Next = newTable[index];
                    newTable[index] = entry;

                    entry = next;
                }
            }

            table = newTable;
            threshold = (int) (newCapacity*loadFactor);
        }
    }
}