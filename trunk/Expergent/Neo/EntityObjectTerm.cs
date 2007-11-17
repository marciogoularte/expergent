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

namespace Expergent.Neo
{
    ///<summary>An Entity Object Term
    ///</summary>
    [Serializable]
    public class EntityObjectTerm : Term
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityObjectTerm"/> class.
        /// </summary>
        /// <param name="entityObject">The entity object.</param>
        public EntityObjectTerm(IFactProvider entityObject)
            : base(entityObject)
        {
            _termType = TermType.EntityObject;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets or sets the underlying value for this Term.
        /// </summary>
        /// <value>The value.</value>
        public new IFactProvider Value
        {
            get { return (IFactProvider) _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new EntityObjectTerm(Value);
        }

        #endregion

        #region Implicit Conversions

        /// <summary>
        /// Implicit operator to convert the specified value to a term.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>EntityObjectTerm</returns>
        public static implicit operator EntityObjectTerm(RulesEnabledEntityObject value)
        {
            return new EntityObjectTerm(value);
        }

        #endregion
    }
}