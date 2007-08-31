#region license

// Copyright (c) 2007, Michael A. Rose (The Universal Sun, Inc.) (michael@theuniversalsun.com)
// All rights reserved.
//
// This file is part of Expergent.
//
// Expergent is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation; either version 3 of the License, or
// (at your option) any later version.
// 
// Expergent is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this distribution; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

#endregion

using System.Collections.Generic;

namespace NSort.Generic
{
    /**
 * 
 * SortAlgorithm.java
 *
 *
 * extended with TriMedian and InsertionSort by Denis Ahrens
 * with all the tips from Robert Sedgewick (Algorithms in C++).
 * It uses TriMedian and InsertionSort for lists shorts than 4.
 * <fuhrmann@cs.tu-berlin.de>
 */

    /// <summary>
    /// A fast quick sort demonstration algorithm
    /// </summary>
    /// <remarks>
    /// <para>
    /// Author James Gosling, Kevin A. Smith, 
    /// </para>
    /// <para>
    /// Source: http://www.cs.ubc.ca/spider/harrison/Java/FastQSortAlgorithm.java.html
    /// </para>
    /// <para>
    /// Ported to C# by Jonathand de Halleux
    /// </para>
    /// </remarks>
    public class FastQuickSorter<T> : SwapSorter<T>
    {
        public FastQuickSorter()
        {
        }

        public FastQuickSorter(IComparer<T> comparer, ISwap<T> swapper)
            : base(comparer, swapper)
        {
        }

        public override void Sort(IList<T> list)
        {
            QuickSort(list, 0, list.Count - 1);
            InsertionSort(list, 0, list.Count - 1);
        }

        #region Internal

        /// <summary>
        /// This is a generic version of C.A.R Hoare's Quick Sort 
        /// algorithm.  This will handle arrays that are already
        /// sorted, and arrays with duplicate keys.
        /// </summary>
        /// <remarks>
        /// If you think of a one dimensional array as going from
        /// the lowest index on the left to the highest index on the right
        /// then the parameters to this function are lowest index or
        /// left and highest index or right.  The first time you call
        /// this function it will be with the parameters 0, a.length - 1.
        /// </remarks>
        /// <param name="list">list to sort</param>
        /// <param name="l">left boundary of array partition</param>
        /// <param name="r">right boundary of array partition</param>
        internal void QuickSort(IList<T> list, int l, int r)
        {
            int M = 4;
            int i;
            int j;
            T v;

            if ((r - l) > M)
            {
                i = (r + l)/2;
                if (Comparer.Compare(list[l], list[i]) > 0)
                    Swapper.Swap(list, l, i); // Tri-Median Methode!
                if (Comparer.Compare(list[l], list[r]) > 0)
                    Swapper.Swap(list, l, r);
                if (Comparer.Compare(list[i], list[r]) > 0)
                    Swapper.Swap(list, i, r);

                j = r - 1;
                Swapper.Swap(list, i, j);
                i = l;
                v = list[j];
                for (;;)
                {
                    while (Comparer.Compare(list[++i], v) > 0)
                    {
                    }

                    while (Comparer.Compare(list[--j], v) < 0)
                    {
                    }

                    if (j < i)
                        break;
                    Swapper.Swap(list, i, j);
                }
                Swapper.Swap(list, i, r - 1);
                QuickSort(list, l, j);
                QuickSort(list, i + 1, r);
            }
        }


        internal void InsertionSort(IList<T> list, int lo0, int hi0)
        {
            int i;
            int j;
            T v;

            for (i = lo0 + 1; i <= hi0; i++)
            {
                v = list[i];
                j = i;
                while ((j > lo0) && (Comparer.Compare(list[j - 1], v) > 0))
                {
                    Swapper.Set(list, j, j - 1);
                    j--;
                }
                list[j] = v;
            }
        }

        #endregion
    }
}