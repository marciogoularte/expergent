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

namespace Expergent
{
    /// <summary>
    /// Sometimes we cannot know in advance how many different lists an item will be on at once.
    /// For example, a given WME w could be in many different alpha memories, hence could be on
    /// many different alpha memory nodes' items lists. So we can't reserve space in w's data structure
    /// for next and previous pointers. Yet for tree-based removal, we need to be able to quickly splice
    /// w out of each of these lists. The obvious solution is to represent each alpha memory's items list
    /// using a doubly-linked set of auxiliary data structures:
    /// </summary>
    public class ItemInAlphaMemory : IVisitable
    {
        private WME _wme;
        private AlphaMemory _amem;

        /// <summary>
        /// Gets or sets the WME.
        /// </summary>
        /// <value>The WME.</value>
        public WME WME
        {
            get { return _wme; }
            set { _wme = value; }
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

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnItemInAlphaMemory(this);
        }

        #endregion
    }
}