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
    /// Returning to beta memory nodes now, the only extra data a beta memory node stores is a
    /// list of the tokens it contains.
    /// </summary>
    public class BetaMemory : ReteNode, IVisitable
    {
        #region Fields

        protected LinkedList<Token> _items;
        protected LinkedList<ReteNode> _all_children;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BetaMemory"/> class.
        /// </summary>
        public BetaMemory()
        {
            _type = ReteNodeType.BetaMemory; // "beta-memory";
            _items = new LinkedList<Token>();
            _all_children = new LinkedList<ReteNode>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the tokens attached to this node.
        /// </summary>
        /// <value>The items.</value>
        public virtual LinkedList<Token> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets all children.
        /// </summary>
        /// <value>All children.</value>
        public virtual LinkedList<ReteNode> AllChildren
        {
            get { return _all_children; }
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
        public override void Accept(IVisitor visitor)
        {
            visitor.OnBetaMemory(this);
        }

        #endregion
    }
}