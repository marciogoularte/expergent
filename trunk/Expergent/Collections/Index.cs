using System;
using System.Text;

namespace Expergent.Collections
{
    /// <author>  Peter Lin<p/>
    /// *
    /// Index is used for the Alpha and BetaMemories for the index. Instead of the
    /// original quick and dirty String index implementation, this implementation
    /// takes an Array of Fact[] objects and calculates the hash.
    /// The class overrides hashCode and equals(Object) so that it works correctly
    /// as the key for HashMaps. There is an unit test called IndexTest in the
    /// test directory.
    /// 
    /// The implementation for now is very simple. Later on, we may need to update
    /// it and make sure it works for memory snapshots and other features.
    /// 
    /// </author>
    [Serializable]
    public sealed class Index<T>
    {
        private T[] factArray = null;

        private int hashInt;

        /// <summary> 
        /// </summary>
        public Index(T[] facts)
        {
            factArray = facts;
            calculateHash();
        }

        /// <summary>
        /// constructor takes facts and a new hashcode. it expects the
        /// hashcode to be correct.
        /// </summary>
        /// <param name="facts">The facts.</param>
        /// <param name="hashCode">The hash code.</param>
        private Index(T[] facts, int hashCode)
        {
            factArray = facts;
            hashInt = hashCode;
        }

        /// <summary> this method should be refactored,so that we couldn't change the value of the memeber vairable facts
        /// Houzhanbin 10/25/2007
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        internal T[] Items
        {
            get { return factArray; }
        }

        /// <summary> This is a very simple implementation that basically adds the hashCodes
        /// of the Facts in the array.
        /// </summary>
        private void calculateHash()
        {
            int hash = 0;
            for (int idx = 0; idx < factArray.Length; idx++)
            {
                hash += factArray[idx].GetHashCode();
            }
            hashInt = hash;
        }


        /// <summary> The implementation is very close to Drools FactHandleList implemented
        /// by simon. The main difference is that Drools uses interfaces and 
        /// Sumatra doesn't. I don't see a need to abstract this out to an
        /// interface, since no one other than an experience rule engine
        /// developer would be writing a new Index class. And even then, it only
        /// makes sense to replace the implementation. Having multiple index
        /// implementations doesn't really make sense.
        /// </summary>
        public override bool Equals(Object other)
        {
            if (this == other)
            {
                return true;
            }
            // the class will only be an Index instance,
            // so we don't have to check the class type

            Index<T> tmpVal = other as Index<T>;

            if (tmpVal == null)
            {
                return false;
            }
            return ArrayEquals(factArray, tmpVal.Items);
        }

        private bool ArrayEquals(T[] lhs, T[] rhs)
        {
            if (lhs.Length == rhs.Length)
            {
                for (int i = 0; i < lhs.Length; i++)
                {
                    if (lhs[i].Equals(rhs[i]) == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary> Method simply returns the cached hashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return hashInt;
        }

        /// <summary> clear the index
        /// </summary>
        public void Clear()
        {
            factArray = null;
            hashInt = 0;
        }

        public Index<T> Add(T fact)
        {
            T[] facts = new T[factArray.Length + 1];
            Array.Copy(factArray, 0, facts, 0, factArray.Length);
            facts[factArray.Length] = fact;
            return new Index<T>(facts, hashInt + fact.GetHashCode());
        }


        public Index<T> AddAll(Index<T> index)
        {
            T[] facts = new T[factArray.Length + index.factArray.Length];
            Array.Copy(factArray, 0, facts, 0, factArray.Length);
            Array.Copy(index.factArray, 0, facts, factArray.Length, index.factArray.Length);
            return new Index<T>(facts, hashInt + index.hashInt);
        }

        public String ToPPString()
        {
            StringBuilder buf = new StringBuilder();
            for (int idx = 0; idx < factArray.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(",");
                }
                buf.Append(factArray[idx].ToString());
            }
            return buf.ToString();
        }
    }
}