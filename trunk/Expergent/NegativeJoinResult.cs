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
    /// A negative-join-result just specifies the (t;w) pair that is the result of the join:
    /// </summary>
    public class NegativeJoinResult : IVisitable
    {
        private Token _owner;
        private WME _wme;

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public Token Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

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
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnNegativeJoinResult(this);
        }
    }
}