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

using Expergent.Conditions;

namespace Expergent
{
    /// <summary>
    /// Represents an inferred fact and the action to take.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class Activation
    {
        private WME _fact;
        private ConditionType _conditionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Activation"/> class.
        /// </summary>
        /// <param name="prod">The prod.</param>
        /// <param name="tok">The tok.</param>
        public Activation(WME prod, ConditionType tok)
        {
            _fact = prod;
            _conditionType = tok;
        }

        /// <summary>
        /// Gets or sets the inferred fact.
        /// </summary>
        /// <value>The inferred fact.</value>
        public WME InferredFact
        {
            get { return _fact; }
            set { _fact = value; }
        }

        /// <summary>
        /// Gets or sets the type of the condition.
        /// </summary>
        /// <value>The type of the condition.</value>
        public ConditionType ConditionType
        {
            get { return _conditionType; }
            set { _conditionType = value; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            Activation other = obj as Activation;
            if (other == null)
                return base.Equals(obj);
            return _fact.Equals(other.InferredFact);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return _fact.GetHashCode();
        }
    }
}