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
    /// <summary>
    /// Default swap class
    /// </summary>
    public class DefaultSwap<T> : ISwap<T>
    {
        /// <summary>
        /// Swaps the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public void Swap(IList<T> array, int left, int right)
        {
            T swap = array[left];
            array[left] = array[right];
            array[right] = swap;
        }

        /// <summary>
        /// Sets the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public void Set(IList<T> array, int left, int right)
        {
            array[left] = array[right];
        }

        /// <summary>
        /// Sets the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="left">The left.</param>
        /// <param name="obj">The obj.</param>
        public void Set(IList<T> array, int left, T obj)
        {
            array[left] = obj;
        }
    }
}