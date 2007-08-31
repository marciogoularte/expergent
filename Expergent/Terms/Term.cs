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
using Expergent.Neo;
using Neo.Framework;

namespace Expergent.Terms
{
    public abstract class Term : IVisitable
    {
        protected TermType _termType;
        protected object _value;
        protected int _ordinal;


        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public abstract Term Copy();

        /// <summary>
        /// Gets or sets the type of the term.
        /// </summary>
        /// <value>The type of the term.</value>
        public TermType TermType
        {
            get { return _termType; }
            set { _termType = value; }
        }

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public virtual object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the position (0-2) where this term was used.
        /// </summary>
        /// <value>The ordinal.</value>
        public int Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString();
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
            Term other = obj as Term;
            if (other != null)
            {
                return _value.Equals(other.Value);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        #region IVisitable Members

        public virtual void Accept(IVisitor visitor)
        {
            visitor.OnTerm(this);
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator Term(String value)
        {
            return new StringTerm(value);
        }

        public static implicit operator Term(char value)
        {
            return new StringTerm(value);
        }

        public static implicit operator Term(Boolean value)
        {
            return new BooleanTerm(value);
        }

        public static implicit operator Term(Boolean? value)
        {
            return new BooleanTerm(value);
        }

        public static implicit operator Term(Double value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator Term(Single value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator Term(Decimal value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator Term(Double? value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator Term(Single? value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator Term(Decimal? value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator Term(Int16 value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator Term(Int32 value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator Term(Int64 value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator Term(Int16? value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator Term(Int32? value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator Term(Int64? value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator Term(DateTime value)
        {
            return new DateTimeTerm(value);
        }

        public static implicit operator Term(DateTime? value)
        {
            return new DateTimeTerm(value);
        }

        public static implicit operator Term(Array value)
        {
            return new ListTerm(value);
        }

        public static implicit operator Term(Guid value)
        {
            return new GuidTerm(value);
        }

        public static implicit operator Term(Guid? value)
        {
            return new GuidTerm(value);
        }

        public static implicit operator Term(ObjectRelationBase value)
        {
            return new ObjectRelationTerm(value);
        }

        public static implicit operator Term(RulesEnabledEntityObject value)
        {
            return new EntityObjectTerm(value);
        }

        #endregion
    }
}