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

using Expergent.Terms;

namespace Expergent.Conditions
{
    ///<summary>A condition suitable for the right hand side (consequent) of a production
    ///</summary>
    public abstract class RightHandSideCondition : Condition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightHandSideCondition"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public RightHandSideCondition(string label, ConditionType type, Term id, Term attribute, Term value)
            : base(label, type, id, attribute, value)
        {
        }
    }
}