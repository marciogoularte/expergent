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
using Expergent.Terms;

namespace Expergent.Builtins
{
    ///<summary>Constant value substitutor
    ///</summary>
    public class ConstantSubstitutor : ISubstitutor, IVisitable
    {
        private Term _obj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantSubstitutor"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ConstantSubstitutor(Term value)
        {
            _obj = value;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _obj.ToString();
        }

        #region ISubstitutor Members

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>Term</returns>
        public Term GetValue(Token t)
        {
            return _obj;
        }

        /// <summary>
        /// Gets the type of the substituter.
        /// </summary>
        /// <value>The type of the substituter.</value>
        public SubstituterType SubstituterType
        {
            get { return SubstituterType.Constant; }
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnConstantSubstitutor(this);
        }

        #endregion
    }
}