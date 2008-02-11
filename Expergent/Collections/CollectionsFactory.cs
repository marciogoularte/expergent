using System;
using System.Collections;

namespace Expergent.Collections
{
    public class CollectionsFactory
    {
        protected internal static CollectionsFactory factory = null;

        protected internal CollectionsFactory()
        {
        }

        public static IMap CustomMap
        {
            get { return new HashMap(); }
        }

        public static void init()
        {
            factory = new CollectionsFactory();
        }

        public static IMap newAlphaMemoryMap(String name)
        {
            return new HashMap();
        }

        public static IDictionary newLinkedHashmap(String name)
        {
            return new LinkedHashMap(new Hashtable());
        }

        public static IMap newBetaMemoryMap(String name)
        {
            return new HashMap();
        }

        public static IMap newTerminalMap()
        {
            return new HashMap();
        }

        public static IMap newClusterableMap(String name)
        {
            return new HashMap();
        }

        public static IMap newMap()
        {
            return new HashMap();
        }

        public static IMap newHashMap()
        {
            return new HashMap();
        }

        public static IMap newNodeMap(String name)
        {
            return new HashMap();
        }

        /// <summary> the sole purpose of this method is to return a Map that is not
        /// clustered. The other methods will return a map, but depending
        /// on the settings, they may return a Map that is hooked into a
        /// JCache compliant product like Tangosol's Coherence.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public static IMap localMap()
        {
            return new HashMap();
        }

        //UPGRADE_TODO: Interface java.util was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
        public static IMap javaHashMap()
        {
            //UPGRADE_TODO: Field java.util was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
            return new HashMap();
        }
    }
}