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

using System.Collections;
using Expergent.Terms;
using Neo.Framework;

namespace Expergent.Neo
{
    public class ObjectRelationTerm : GenericTerm<ObjectRelationBase>, IEnumerable
    {
        #region Constructors

        public ObjectRelationTerm(ObjectRelationBase objectRelation) : base(objectRelation)
        {
            _termType = TermType.ObjectRelation;
        }

        #endregion

        #region Overrides

        public override Term Copy()
        {
            return new ObjectRelationTerm(Value);
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator ObjectRelationTerm(ObjectRelationBase value)
        {
            return new ObjectRelationTerm(value);
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        #endregion
    }
}