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
    ///<summary>A Date Time Term
    ///</summary>
    public class DateTimeTerm : Term
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTerm"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        public DateTimeTerm(DateTime date) : base(date)
        {
            _termType = TermType.DateTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTerm"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        public DateTimeTerm(DateTime? date)
            : base(date.Value)
        {
            _termType = TermType.DateTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTerm"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public DateTimeTerm(string s) : this(Convert.ToDateTime(s))
        {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new DateTime Value
        {
            get { return (DateTime)_value; }
            set { _value = value; }
        }

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new DateTimeTerm(Value);
        }

        #endregion

        #region Implicit Conversions

        /// <summary>
        /// Implicit operator to convert the specified value into a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DateTimeTerm</returns>
        public static implicit operator DateTimeTerm(DateTime value)
        {
            return new DateTimeTerm(value);
        }

        /// <summary>
        /// Implicit operator to convert the specified value into a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DateTimeTerm</returns>
        public static implicit operator DateTimeTerm(DateTime? value)
        {
            return new DateTimeTerm(value);
        }

        #endregion
    }
}