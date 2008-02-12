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
using Expergent.Interfaces;
using Wintellect.PowerCollections;

namespace Expergent
{
    /// <summary>
    /// An alpha memory stores a list of the WMEs it contains, plus a list of its successors (join nodes
    /// attached to it).
    /// </summary>
    /// <remarks>
    /// This class has been modified with the Unlinking Code.
    /// </remarks>
    public class AlphaMemory : IVisitable
    {
        #region Fields

        private readonly BigList<ItemInAlphaMemory> _items;
        private readonly LinkedList<ReteNode> _successors;
        private int _reference_count;
        private string _label;
        private readonly List<string> _conditions;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AlphaMemory"/> class.
        /// </summary>
        public AlphaMemory()
        {
            _items = new BigList<ItemInAlphaMemory>();
            _successors = new LinkedList<ReteNode>();
            _conditions = new List<string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        public List<string> Conditions
        {
            get { return _conditions; }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public BigList<ItemInAlphaMemory> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the successors.
        /// </summary>
        /// <value>The successors.</value>
        public LinkedList<ReteNode> Successors
        {
            get { return _successors; }
        }

        /// <summary>
        /// Gets or sets the reference count.
        /// </summary>
        /// <value>The reference count.</value>
        public int ReferenceCount
        {
            get { return _reference_count; }
            set { _reference_count = value; }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _label;
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnAlphaMemory(this);
        }

        #endregion
    }
}