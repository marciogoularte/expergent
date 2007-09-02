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

namespace Expergent.Terms
{
    ///<summary>A term for strings.
    ///</summary>
    public class StringTerm : Term //GenericTerm<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringTerm"/> class.
        /// </summary>
        /// <param name="s">The String.</param>
        public StringTerm(String s) : base(s)
        {
            _termType = TermType.String;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTerm"/> class.
        /// </summary>
        /// <param name="c">The char.</param>
        public StringTerm(char c) : base(new string(c, 1))
        {
            _termType = TermType.String;
        }

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new string Value
        {
            get { return (string)_value; }
            set { _value = value; }
        }

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new StringTerm(Value);
        }

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>StringTerm</returns>
        public static implicit operator StringTerm(String value)
        {
            return new StringTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>StringTerm</returns>
        public static implicit operator StringTerm(char value)
        {
            return new StringTerm(value);
        }
    }
}