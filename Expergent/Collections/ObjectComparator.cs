using System;

namespace Expergent.Collections
{
    public interface ObjectComparator
    {
        int hashCodeOf(Object object_Renamed);
        int rehash(int hashCode);
        bool equal(Object object1, Object object2);
    }
}