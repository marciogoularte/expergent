using System;

namespace Expergent.Collections
{
    public abstract class AbstractMap : IMap
    {
        internal const int MAX_CAPACITY = 1 << 30;

        protected internal ObjectComparator comparator;

        private EntryIterator eIterator;
        protected internal float loadFactor;
        protected internal int size_Renamed_Field;
        protected internal IEntry[] table;
        protected internal int threshold;

        /// <summary> 
        /// </summary>
        public AbstractMap(int capacity, float factor)
        {
            loadFactor = factor;
            threshold = (int) (capacity*loadFactor);
            table = new IEntry[capacity];
            comparator = EqualityEquals.Instance;
        }

        #region IMap Members

        public virtual bool Empty
        {
            get { return size_Renamed_Field == 0; }
        }

        public virtual int Count
        {
            get { return size_Renamed_Field; }
        }


        public abstract bool ContainsKey(object key);

        public abstract object Get(object key);

        public abstract object Put(object key, object value_Renamed);

        public abstract object Remove(object key);

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
                    IEntry e = table[idx];
                    e.Clear();
                }
            }
            table = null;
            eIterator.reset();
        }

        public virtual Iterator KeysIterator()
        {
            if (eIterator == null)
            {
                eIterator = new EntryIterator(this);
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
            IEntry[] oldTable = table;
            int oldCapacity = oldTable.Length;
            if (oldCapacity == MAX_CAPACITY)
            {
                threshold = Int32.MaxValue;
                return;
            }

            IEntry[] newTable = new IEntry[newCapacity];

            for (int i = 0; i < table.Length; i++)
            {
                IEntry entry = table[i];
                if (entry == null)
                {
                    continue;
                }
                table[i] = null;
                IEntry next = null;
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

        #region Nested type: EntryIterator

        public class EntryIterator : Iterator
        {
            private IEntry entry;
            private readonly AbstractMap hashMap;

            private int length;

            //private IEntry next_Renamed_Field;
            private int row;
            private IEntry[] table;

            public EntryIterator(AbstractMap map)
            {
                hashMap = map;
            }

            #region Iterator Members

            public virtual object Next()
            {
                if (entry == null)
                {
                    // keep skipping rows until we come to the end, or find one that is populated
                    while (entry == null)
                    {
                        row++;
                        if (row == length)
                        {
                            return null;
                        }
                        entry = table[row];
                    }
                }
                else
                {
                    entry = entry.Next;
                    if (entry == null)
                    {
                        entry = (IEntry) Next();
                    }
                }

                return entry;
            }

            #endregion

            public virtual void remove()
            {
                hashMap.Remove(entry);
            }

            public virtual void reset()
            {
                table = hashMap.table;
                length = table.Length;
                row = -1;
                entry = null;
                //next_Renamed_Field = null;
            }
        }

        #endregion

        #region Nested type: EqualityEquals

        public class EqualityEquals : ObjectComparator
        {
            public static ObjectComparator INSTANCE;

            static EqualityEquals()
            {
                INSTANCE = new EqualityEquals();
            }

            public static ObjectComparator Instance
            {
                get { return INSTANCE; }
            }

            #region ObjectComparator Members

            public virtual int hashCodeOf(Object key)
            {
                return rehash(key.GetHashCode());
            }

            public virtual int rehash(int h)
            {
                h += ~(h << 9);
                h ^= (Support.URShift(h, 14));
                h += (h << 4);
                h ^= (Support.URShift(h, 10));
                return h;
            }

            public virtual bool equal(Object object1, Object object2)
            {
                return object1.Equals(object2);
            }

            #endregion
        }

        #endregion

        #region Nested type: InstanceEquals

        public class InstanceEquals : ObjectComparator
        {
            public static ObjectComparator INSTANCE;

            static InstanceEquals()
            {
                INSTANCE = new InstanceEquals();
            }

            public static ObjectComparator Instance
            {
                get { return INSTANCE; }
            }

            #region ObjectComparator Members

            public virtual int hashCodeOf(Object key)
            {
                return rehash(key.GetHashCode());
            }

            public virtual int rehash(int h)
            {
                h += ~(h << 9);
                h ^= (Support.URShift(h, 14));
                h += (h << 4);
                h ^= (Support.URShift(h, 10));
                return h;
            }

            public virtual bool equal(Object object1, Object object2)
            {
                return object1 == object2;
            }

            #endregion
        }

        #endregion

        #region Nested type: ObjectComparator

        /// <summary> Internal interface for comparing objects
        /// </summary>
        /// <author>  pete
        /// *
        /// 
        /// </author>
        public interface ObjectComparator
        {
            int hashCodeOf(Object object_Renamed);
            int rehash(int hashCode);
            bool equal(Object object1, Object object2);
        }

        #endregion
    }
}