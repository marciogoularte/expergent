using System;

namespace Expergent.Collections
{
    public class ObjectEntry<K, V> : IEntry<K, V>
    {
        private readonly int hashCode_Renamed_Field;
        private K key;

        private IEntry<K, V> next;
        private V value_Renamed;

        public ObjectEntry(K key, V value_Renamed, int hashCode)
        {
            this.key = key;
            this.value_Renamed = value_Renamed;
            hashCode_Renamed_Field = hashCode;
        }

        #region IEntry Members

        public virtual V Value
        {
            get { return value_Renamed; }

            set { value_Renamed = value; }
        }

        public virtual K Key
        {
            get { return key; }
        }

        public virtual IEntry<K, V> Next
        {
            get { return next; }

            set { next = value; }
        }


        public virtual void Clear()
        {
            key = default(K);// null;
            value_Renamed = default(V); 
            next.Clear();
        }

        #endregion

        public override int GetHashCode()
        {
            return hashCode_Renamed_Field;
        }

        public override bool Equals(Object object_Renamed)
        {
            if (object_Renamed == this)
            {
                return true;
            }
            ObjectEntry<K, V> other = (ObjectEntry<K, V>)object_Renamed;
            return key.Equals(other.key) && value_Renamed.Equals(other.value_Renamed);
        }
    }
}