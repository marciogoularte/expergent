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
    ///<summary>An Integer Term
    ///</summary>
    public class IntegerTerm : Term
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerTerm"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public IntegerTerm(String s) : this(Int32.Parse(s))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerTerm"/> class.
        /// </summary>
        /// <param name="i">The i.</param>
        public IntegerTerm(Int16 i) : base(i)
        {
            _termType = TermType.Integer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerTerm"/> class.
        /// </summary>
        /// <param name="i">The i.</param>
        public IntegerTerm(Int32 i) : base(i)
        {
            _termType = TermType.Integer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerTerm"/> class.
        /// </summary>
        /// <param name="i">The i.</param>
        public IntegerTerm(Int64 i) : base(Convert.ToInt32(i))
        {
            _termType = TermType.Integer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerTerm"/> class.
        /// </summary>
        /// <param name="i">The i.</param>
        public IntegerTerm(Int16? i)
            : base(i.Value)
        {
            _termType = TermType.Integer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerTerm"/> class.
        /// </summary>
        /// <param name="i">The i.</param>
        public IntegerTerm(Int32? i)
            : base(i.Value)
        {
            _termType = TermType.Integer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerTerm"/> class.
        /// </summary>
        /// <param name="i">The i.</param>
        public IntegerTerm(Int64? i)
            : base(Convert.ToInt32(i.Value))
        {
            _termType = TermType.Integer;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new Int32 Value
        {
            get { return (Int32)_value; }
            set { _value = value; }
        }

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new IntegerTerm(Value);
        }

        #endregion

        #region Implicit Conversions

        /// <summary>
        /// Implicit operator to convert the specified value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntegerTerm</returns>
        public static implicit operator IntegerTerm(Int16 value)
        {
            return new IntegerTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert the specified value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntegerTerm</returns>
        public static implicit operator IntegerTerm(Int32 value)
        {
            return new IntegerTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert the specified value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntegerTerm</returns>
        public static implicit operator IntegerTerm(Int64 value)
        {
            return new IntegerTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert the specified value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntegerTerm</returns>
        public static implicit operator IntegerTerm(Int16? value)
        {
            return new IntegerTerm(value.Value);
        }

        /// <summary>
        /// Implicit operator to convert the specified value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntegerTerm</returns>
        public static implicit operator IntegerTerm(Int32? value)
        {
            return new IntegerTerm(value.Value);
        }

        /// <summary>
        /// Implicit operator to convert the specified value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IntegerTerm</returns>
        public static implicit operator IntegerTerm(Int64? value)
        {
            return new IntegerTerm(value.Value);
        }

        #endregion
    }
}