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

namespace Expergent
{
    /// <summary>
    /// The data structure for a negative node looks like a combination of those for a beta memory and a join node:
    /// </summary>
    /// <remarks>
    /// So far we have been discussing conditions that test for the presence of a WME in working
    /// memory. We now move on to discuss conditions testing for the absence of items in working
    /// memory. In this section, we discuss negated conditions, which test for the absence of a certain
    /// WME;
    /// </remarks>
    public class NegativeNode : ReteNode
    {
        private LinkedList<Token> _items;
        private AlphaMemory _amem;
        private LinkedList<TestAtJoinNode> _tests;
        private ReteNode _nearest_ancestor_with_same_amem;
        private bool _isRightUnlinked;

        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeNode"/> class.
        /// </summary>
        public NegativeNode()
        {
            _type = ReteNodeType.Negative;
            _items = new LinkedList<Token>();
            _tests = new LinkedList<TestAtJoinNode>();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public LinkedList<Token> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets or sets the alpha memory.
        /// </summary>
        /// <value>The alpha memory.</value>
        public AlphaMemory AlphaMemory
        {
            get { return _amem; }
            set { _amem = value; }
        }

        /// <summary>
        /// Gets the tests.
        /// </summary>
        /// <value>The tests.</value>
        public LinkedList<TestAtJoinNode> Tests
        {
            get { return _tests; }
        }

        /// <summary>
        /// Gets or sets the nearest ancestor with same amem.
        /// </summary>
        /// <value>The nearest ancestor with same amem.</value>
        public ReteNode NearestAncestorWithSameAmem
        {
            get { return _nearest_ancestor_with_same_amem; }
            set { _nearest_ancestor_with_same_amem = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is right unlinked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is right unlinked; otherwise, <c>false</c>.
        /// </value>
        public bool IsRightUnlinked
        {
            get { return _isRightUnlinked; }
            set { _isRightUnlinked = value; }
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnNegativeNode(this);
        }
    }
}