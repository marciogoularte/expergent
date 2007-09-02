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
using System.Collections;
using System.Text;

namespace Expergent.Terms
{
    ///<summary>A Collection Term
    ///</summary>
    public class CollectionTerm : Term
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionTerm"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public CollectionTerm(ICollection obj)
        {
            _termType = TermType.Collection;
            if (obj != null)
                _value = obj;
        }

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new ICollection Value
        {
            get { return new ArrayList(_value as ICollection); }
            set { _value = value; }
        }

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("list(");
            bool prePend = false;
            foreach (object o in (ICollection) _value)
            {
                if (prePend)
                {
                    sb.Append(", ").Append(o.ToString());
                }
                else
                {
                    sb.Append(o.ToString());
                    prePend = true;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}