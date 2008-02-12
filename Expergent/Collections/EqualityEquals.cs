using System;

namespace Expergent.Collections
{

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
}