using System;
using System.Text;

namespace Expergent.Collections
{
    [Serializable]
    public class NotEqHashIndex
    {
        private int eqhashCode;
        private EqHashIndex negindex;
        private BindingPair[] values = null;

        /// <summary> 
        /// </summary>
        public NotEqHashIndex(BindingPair[] thevalues)
        {
            values = thevalues;
            calculateHash();
        }

        /// <summary> return the subindex
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual EqHashIndex SubIndex
        {
            get { return negindex; }
        }

        /// <summary> The implementation is different than EqHashIndex. It ignores
        /// any Bindings that are negated
        /// </summary>
        private void calculateHash()
        {
            Object[] neg = new Object[values.Length];
            int z = 0;
            if (values != null && values.Length > 0)
            {
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (values[idx] != null && !values[idx].negated)
                    {
                        eqhashCode += values[idx].Value.GetHashCode();
                    }
                    else
                    {
                        neg[z] = values[idx].Value;
                        z++;
                    }
                }
            }
            Object[] neg2 = new Object[z];
            Array.Copy(neg, 0, neg2, 0, z);
            negindex = new EqHashIndex(neg2);
            neg = null;
            neg2 = null;
        }

        public virtual void clear()
        {
            negindex.clear();
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
            if (val == null || !(val is NotEqHashIndex))
            {
                return false;
            }
            NotEqHashIndex eval = (NotEqHashIndex) val;
            bool eq = true;
            for (int idx = 0; idx < values.Length; idx++)
            {
                if (!values[idx].negated && !eval.values[idx].Value.Equals(values[idx].Value))
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
            return eqhashCode;
        }

        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("NotEqHashIndex2: ");
            buf.Append(negindex.toPPString());
            return buf.ToString();
        }
    }
}