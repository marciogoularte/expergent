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
    public class DoubleTerm : GenericTerm<double>
    {
        #region constructors

        public DoubleTerm(String s) : this(Double.Parse(s))
        {
        }

        public DoubleTerm(double d) : base(d)
        {
            _termType = TermType.Double;
        }

        public DoubleTerm(Single d) : base(d)
        {
            _termType = TermType.Double;
        }

        public DoubleTerm(decimal d) : base(Convert.ToDouble(d))
        {
            _termType = TermType.Double;
        }

        public DoubleTerm(double? d)
            : base(d.Value)
        {
            _termType = TermType.Double;
        }

        public DoubleTerm(Single? d)
            : base(d.Value)
        {
            _termType = TermType.Double;
        }

        public DoubleTerm(decimal? d)
            : base(Convert.ToDouble(d.Value))
        {
            _termType = TermType.Double;
        }

        #endregion

        #region Overrides

        public override Term Copy()
        {
            return new DoubleTerm(Value);
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator DoubleTerm(Double value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator DoubleTerm(Single value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator DoubleTerm(Decimal value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator DoubleTerm(Double? value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator DoubleTerm(Single? value)
        {
            return new DoubleTerm(value);
        }

        public static implicit operator DoubleTerm(Decimal? value)
        {
            return new DoubleTerm(value);
        }

        #endregion
    }
}