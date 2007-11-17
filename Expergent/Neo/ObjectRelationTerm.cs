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
using Expergent.Terms;
using Neo.Framework;

namespace Expergent.Neo
{
    ///<summary>
    ///</summary>
    [Serializable]
    public class ObjectRelationTerm : Term, IEnumerable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectRelationTerm"/> class.
        /// </summary>
        /// <param name="objectRelation">The object relation.</param>
        public ObjectRelationTerm(ObjectRelationBase objectRelation) : base(objectRelation)
        {
            _termType = TermType.ObjectRelation;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new ObjectRelationTerm(Value);
        }

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new ObjectRelationBase Value
        {
            get { return (ObjectRelationBase)_value; }
            set { _value = value; }
        }

        #endregion

        #region Implicit Conversions

        /// <summary>
        /// Implicit operator that converts a value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ObjectRelationTerm</returns>
        public static implicit operator ObjectRelationTerm(ObjectRelationBase value)
        {
            return new ObjectRelationTerm(value);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        #endregion
    }
}