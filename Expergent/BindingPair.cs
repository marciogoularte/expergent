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
using Expergent.Terms;

namespace Expergent
{
    /// <summary>
    /// Represents a Variable and its bound value, in the form of a substitutor
    /// </summary>
    public class BindingPair : IVisitable
    {
        private Variable _variable;
        private Term _substitutor;
        private bool negated_Renamed_Field;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingPair"/> class.
        /// </summary>
        public BindingPair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingPair"/> class.
        /// </summary>
        /// <param name="lvariable">The lvariable.</param>
        public BindingPair(Variable lvariable)
        {
            _variable = lvariable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingPair"/> class.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="term">The term.</param>
        public BindingPair(Variable variable, Term term)
        {
            _variable = variable;
            _substitutor = term;
        }

        /// <summary>
        /// Gets or sets the variable.
        /// </summary>
        /// <value>The variable.</value>
        public Variable Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public Term Value
        {
            get { return _substitutor; }
            set { _substitutor = value; }
        }

        public virtual bool negated
        {
            get { return this.negated_Renamed_Field; }
            set { negated_Renamed_Field = value; }
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override String ToString()
        {
            return string.Format("({0} = {1})", _variable, _substitutor);
        }

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnBindingPair(this);
        }

        #endregion
    }
}