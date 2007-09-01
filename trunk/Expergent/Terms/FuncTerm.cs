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

namespace Expergent.Terms
{
    [Serializable]
    public class FuncTerm : Term
    {
        private IBuiltIn _builtin;

        #region constructors

        public FuncTerm(String s)
        {
            TermType = TermType.Function;
            _value = s;
        }

        public FuncTerm(String s, IBuiltIn builtin) : this(s)
        {
            _builtin = builtin;
        }

        #endregion

        #region properties

        public IBuiltIn Builtin
        {
            get { return _builtin; }
            set { _builtin = value; }
        }

        public new string Value
        {
            get { return _value.ToString(); }
            set { _value = value; }
        }

        #endregion

        #region methods

        public override int GetHashCode()
        {
            return 0;
        }

        public override Term Copy()
        {
            return new FuncTerm(Value, _builtin);
        }

        #endregion
    }
}