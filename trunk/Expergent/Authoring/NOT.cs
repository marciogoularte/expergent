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

using Expergent.Conditions;
using Expergent.Terms;

namespace Expergent.Authoring
{
    public class NOT : Condition
    {
        public NOT(Term id, Term attribute, Term value)
            : base("NegativeCondition", ConditionType.Negative, id, attribute, value)
        {
        }

        public NOT(string label, Term id, Term attribute, Term value)
            : base(label, ConditionType.Negative, id, attribute, value)
        {
        }

        public NOT(Term id, Term attribute, Term value, params LeftHandSideCondition[] subConditions)
            : base("NCCCondition", ConditionType.NCC, id, attribute, value)
        {
            _subconditions.AddRange(subConditions);
        }

        public NOT(string label, Term id, Term attribute, Term value, params LeftHandSideCondition[] subConditions)
            : base(label, ConditionType.NCC, id, attribute, value)
        {
            _subconditions.AddRange(subConditions);
        }
    }
}