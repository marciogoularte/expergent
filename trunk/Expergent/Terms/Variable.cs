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
using Expergent.Interfaces;

namespace Expergent.Terms
{
    ///<summary>A Term representing a variable binding
    ///</summary>
    public class Variable : Term, IVisitable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Variable(String label)
        {
            TermType = TermType.Variable;
            _value = label;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new string Value
        {
            get { return _value.ToString(); }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public String Label
        {
            get { return _value.ToString(); }

            set { _value = value; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new Variable(_value.ToString());
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override String ToString()
        {
            return "?" + _value;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return 0;
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public new void Accept(IVisitor visitor)
        {
            visitor.OnVariable(this);
        }

        #endregion
    }
}