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
    ///<summary>A Double Term
    ///</summary>
    public class DoubleTerm : Term
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTerm"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public DoubleTerm(String s) : this(Double.Parse(s))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTerm"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public DoubleTerm(double d) : base(d)
        {
            _termType = TermType.Double;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTerm"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public DoubleTerm(Single d) : base(d)
        {
            _termType = TermType.Double;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTerm"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public DoubleTerm(decimal d) : base(Convert.ToDouble(d))
        {
            _termType = TermType.Double;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTerm"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public DoubleTerm(double? d)
            : base(d.Value)
        {
            _termType = TermType.Double;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTerm"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public DoubleTerm(Single? d)
            : base(d.Value)
        {
            _termType = TermType.Double;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTerm"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public DoubleTerm(decimal? d)
            : base(Convert.ToDouble(d.Value))
        {
            _termType = TermType.Double;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new double Value
        {
            get { return (double) _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new DoubleTerm(Value);
        }

        #endregion

        #region Implicit Conversions

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DoubleTerm</returns>
        public static implicit operator DoubleTerm(Double value)
        {
            return new DoubleTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DoubleTerm</returns>
        public static implicit operator DoubleTerm(Single value)
        {
            return new DoubleTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DoubleTerm</returns>
        public static implicit operator DoubleTerm(Decimal value)
        {
            return new DoubleTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DoubleTerm</returns>
        public static implicit operator DoubleTerm(Double? value)
        {
            return new DoubleTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DoubleTerm</returns>
        public static implicit operator DoubleTerm(Single? value)
        {
            return new DoubleTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DoubleTerm</returns>
        public static implicit operator DoubleTerm(Decimal? value)
        {
            return new DoubleTerm(value);
        }

        #endregion
    }
}