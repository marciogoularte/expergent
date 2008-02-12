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
    /// A special node that represents a set of complete matches for a production. It is the at the bottom of the network.
    /// </summary>
    /// <remarks>
    /// <para>
    /// We mention the implementation of production nodes (p-nodes) here because it is often similar to
    /// that of memory nodes in some respects. The implementation of p-nodes tends to vary from one
    /// system to another, so our discussion here will be rather general and we will not give pseudocode.
    /// A p-node may store tokens, just as beta memories do; these tokens represent complete matches
    /// for the production's conditions. (In traditional production systems, the set of all tokens at all
    /// p-nodes represents the conict set.) On a left activation, a p-node will build a new token, or
    /// some similar representation of the newly found complete match. It then signals the new match
    /// in some appropriate (system-dependent) way.
    /// </para>
    /// <para>
    /// In general, a p-node also contains a specication of what production it corresponds to the
    /// name of the production, its right-hand-side actions, etc. A p-node may also contain information
    /// about the names of the variables that occur in the production. Note that variable names
    /// are not mentioned in any of the Rete node data structures we describe in this chapter. This is
    /// intentional it enables nodes to be shared when two productions have conditions with the same
    /// basic form, but with different variable names. If variable names are recorded somewhere, it is
    /// possible to reconstruct the LHS of a production by looking at (its portion of) the Rete network
    /// together with the variable name information. The ability to reconstruct the LHS eliminates the
    /// need to save an "original copy" of the LHS in case we need to examine the production later.
    /// </para>
    /// </remarks>
    public class ProductionNode : ReteNode
    {
        private Production _production;
        private readonly BigList<Token> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionNode"/> class.
        /// </summary>
        public ProductionNode()
        {
            _type = ReteNodeType.Production;
            _items = new BigList<Token>();
        }

        /// <summary>
        /// Gets or sets the production.
        /// </summary>
        /// <value>The production.</value>
        public Production Production
        {
            get { return _production; }
            set { _production = value; }
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
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_production.Label))
            {
                return base.ToString();
            }
            return _production.Label;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnPNode(this);
        }
    }
}