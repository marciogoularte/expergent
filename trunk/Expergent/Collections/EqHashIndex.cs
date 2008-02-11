using System;

namespace Expergent.Collections
{
    [Serializable]
    public class EqHashIndex
    {
        private int hashCode_Renamed_Field;
        private Object[] values = null;

        /// <summary> 
        /// </summary>
        public EqHashIndex(Object[] thevalues)
        {
            values = thevalues;
            calculateHash();
        }

        /// <summary> This is a very simple implementation that gets the slot hash from
        /// the deffact.
        /// </summary>
        private void calculateHash()
        {
            if (values != null && values.Length > 0)
            {
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (values[idx] != null)
                    {
                        hashCode_Renamed_Field += values[idx].GetHashCode();
                    }
                }
            }
        }

        public virtual void clear()
        {
            values = null;
        }

        /// <summary> The implementation is similar to the index class.
        /// </summary>
        public override bool Equals(Object val)
        {
            if (this == val)
            {
                return true;
            }
            if (val == null || !(val is EqHashIndex))
            {
                return false;
            }
            EqHashIndex eval = (EqHashIndex) val;
            bool eq = true;
            for (int idx = 0; idx < values.Length; idx++)
            {
                if (!eval.values[idx].Equals(values[idx]))
                {
                    eq = false;
                    break;
                }
            }
            return eq;
        }

        /// <summary> Method simply returns the cached hashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return hashCode_Renamed_Field;
        }

        public virtual String toPPString()
        {
            // TODO Auto-generated method stub
            return null;
        }
    }
}