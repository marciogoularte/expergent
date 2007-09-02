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
    ///<summary>A Object Term
    ///</summary>
    public class ObjectTerm : Term
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectTerm"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public ObjectTerm(Object obj) : base(obj)
        {
            TermType = TermType.ObjectReference;
        }

        /// <summary>
        /// Gets the name of the object class.
        /// </summary>
        /// <value>The name of the object class.</value>
        public string ObjectClassName
        {
            get { return Value.GetType().FullName; }
        }

        /// <summary>
        /// Method for creating a copy of this term.
        /// </summary>
        /// <returns></returns>
        public override Term Copy()
        {
            return new ObjectTerm(Value);
        }
    }
}