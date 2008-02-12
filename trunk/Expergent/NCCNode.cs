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

using Expergent.Interfaces;
using Wintellect.PowerCollections;

namespace Expergent
{
    /// <summary>
    /// A negated conjunctive condition node
    /// </summary>
    /// <remarks>
    /// In this section, we show how to implement negated conjunctive conditions (NCC's), also called
    /// conjunctive negations, which test for the absence of a certain combination of WMEs. (Conjunctive
    /// negations with only one conjunct are semantically equivalent to the negated conditions
    /// discussed in the previous section.)
    /// </remarks>
    public class NCCNode : ReteNode
    {
        private readonly BigList<Token> _items;
        private NCCPartnerNode _partner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCCNode"/> class.
        /// </summary>
        public NCCNode()
        {
            _type = ReteNodeType.NCC; // "NCC";
            _items = new BigList<Token>();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public BigList<Token> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets or sets the partner.
        /// </summary>
        /// <value>The partner.</value>
        public NCCPartnerNode Partner
        {
            get { return _partner; }
            set { _partner = value; }
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnNCCNode(this);
        }
    }
}