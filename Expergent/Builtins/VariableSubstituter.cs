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
    ///<summary>A Variable Substituter
    ///</summary>
    public class VariableSubstituter : ISubstitutor, IVisitable
    {
        private int fieldNumber;
        private int numberOfLevelsUp;
        private BindingPair _bindingPair;
        private string _wmeRef;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSubstituter"/> class.
        /// </summary>
        public VariableSubstituter()
        {
            _bindingPair = new BindingPair();
        }

        /// <summary>
        /// Gets or sets the binding pair.
        /// </summary>
        /// <value>The binding pair.</value>
        public BindingPair BindingPair
        {
            get { return _bindingPair; }
            set { _bindingPair = value; }
        }

        /// <summary>
        /// Gets or sets the field number.
        /// </summary>
        /// <value>The field number.</value>
        public int FieldNumber
        {
            get { return fieldNumber; }
            set { fieldNumber = value; }
        }

        /// <summary>
        /// Gets or sets the number of levels up.
        /// </summary>
        /// <value>The number of levels up.</value>
        public int NumberOfLevelsUp
        {
            get { return numberOfLevelsUp; }
            set { numberOfLevelsUp = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", _wmeRef, _bindingPair);
        }

        #region ISubstitutor Members

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>Term</returns>
        public Term GetValue(Token t)
        {
            Token tok = t.GetTokenUp(numberOfLevelsUp);
            _wmeRef = tok.WME.ToString();
            _bindingPair.Value = tok.WME[fieldNumber];
            return _bindingPair.Value;
        }

        /// <summary>
        /// Gets the type of the substituter.
        /// </summary>
        /// <value>The type of the substituter.</value>
        public SubstituterType SubstituterType
        {
            get { return SubstituterType.Variable; }
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnVariableSubstituter(this);
        }

        #endregion
    }
}