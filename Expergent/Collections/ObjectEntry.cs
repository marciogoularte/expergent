using System;

namespace Expergent.Collections
{
    public class ObjectEntry : IEntry
    {
        private readonly int hashCode_Renamed_Field;
        private Object key;

        private IEntry next;
        private Object value_Renamed;

        public ObjectEntry(Object key, Object value_Renamed, int hashCode)
        {
            this.key = key;
            this.value_Renamed = value_Renamed;
            hashCode_Renamed_Field = hashCode;
        }

        #region IEntry Members

        public virtual Object Value
        {
            get { return value_Renamed; }

            set { value_Renamed = value; }
        }

        public virtual Object Key
        {
            get { return key; }
        }

        public virtual IEntry Next
        {
            get { return next; }

            set { next = value; }
        }


        public virtual void Clear()
        {
            key = null;
            value_Renamed = null;
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
            ObjectEntry other = (ObjectEntry) object_Renamed;
            return key.Equals(other.key) && value_Renamed.Equals(other.value_Renamed);
        }
    }
}