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

using System;
using System.Collections;
using System.Collections.Generic;

namespace NSort.Generic
{
    /// <summary>
    /// Abstract base class for Swap sort algorithms.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class serves as a base class for swap based sort algorithms.
    /// </para>
    /// </remarks>
    public abstract class SwapSorter<T> : ISorter<T>
    {
        private IComparer<T> comparer;
        private ISwap<T> swapper;

        public SwapSorter()
        {
            comparer = Comparer<T>.Default;
            swapper = new DefaultSwap<T>();
        }

        public SwapSorter(IComparer<T> comparer, ISwap<T> swapper)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (swapper == null)
                throw new ArgumentNullException("swapper");

            this.comparer = comparer;
            this.swapper = swapper;
        }

        /// <summary>
        /// Gets or sets the <see cref="IComparer"/> object
        /// </summary>
        /// <value>
        /// Comparer object
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// Set property, the value is a null reference
        /// </exception>
        public IComparer<T> Comparer
        {
            get { return comparer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("comparer");
                comparer = value;
            }
        }

        /// <summary>
        /// Gets or set the swapper object
        /// </summary>
        /// <value>
        /// The <see cref="ISwap"/> swapper.
        /// </value>
        /// <exception cref="ArgumentNullException">Swapper is a null reference</exception>
        public ISwap<T> Swapper
        {
            get { return swapper; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("swapper");
                swapper = value;
            }
        }

        public abstract void Sort(IList<T> list);
    }
}