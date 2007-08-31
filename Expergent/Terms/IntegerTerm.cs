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
    public class IntegerTerm : GenericTerm<int>
    {
        #region Constructors

        public IntegerTerm(String s) : this(Int32.Parse(s))
        {
        }

        public IntegerTerm(Int16 i) : base(i)
        {
            TermType = TermType.Integer;
        }

        public IntegerTerm(Int32 i) : base(i)
        {
            TermType = TermType.Integer;
        }

        public IntegerTerm(Int64 i) : base(Convert.ToInt32(i))
        {
            TermType = TermType.Integer;
        }

        public IntegerTerm(Int16? i)
            : base(i.Value)
        {
            TermType = TermType.Integer;
        }

        public IntegerTerm(Int32? i)
            : base(i.Value)
        {
            TermType = TermType.Integer;
        }

        public IntegerTerm(Int64? i)
            : base(Convert.ToInt32(i.Value))
        {
            TermType = TermType.Integer;
        }

        #endregion

        #region Overrides

        public override Term Copy()
        {
            return new IntegerTerm(Value);
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator IntegerTerm(Int16 value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator IntegerTerm(Int32 value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator IntegerTerm(Int64 value)
        {
            return new IntegerTerm(value);
        }

        public static implicit operator IntegerTerm(Int16? value)
        {
            return new IntegerTerm(value.Value);
        }

        public static implicit operator IntegerTerm(Int32? value)
        {
            return new IntegerTerm(value.Value);
        }

        public static implicit operator IntegerTerm(Int64? value)
        {
            return new IntegerTerm(value.Value);
        }

        #endregion
    }
}