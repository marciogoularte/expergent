namespace Expergent.Collections
{
    public class EntryIterator<K, V> : IEntryIterator<K, V>
    {
        private readonly AbstractMap<K, V> hashMap;
        private IEntry<K, V> entry;

        private int length;

        //private IEntry next_Renamed_Field;
        private int row;
        private IEntry<K, V>[] table;

        public EntryIterator(AbstractMap<K, V> map)
        {
            hashMap = map;
        }

        #region Iterator<K,V> Members

        public virtual IEntry<K, V> Next()
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
                    entry = (IEntry<K, V>) Next();
                }
            }

            return entry;
        }

        #endregion

        public virtual void remove()
        {
            hashMap.Remove(entry.Key);
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
}