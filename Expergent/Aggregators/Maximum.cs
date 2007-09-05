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
using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Evaluates a list of values and returns the Largest value.
    /// </summary>
    public class Maximum : BaseAggregator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        public Maximum(Term matchPredicate)
            : base(matchPredicate, "Maximum")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public Maximum(Term matchPredicate, StringTerm alias)
            : base(matchPredicate, alias)
        {
        }

        #endregion

        #region BaseAggregator Implimentation

        /// <summary>
        /// Evaluates the specified list of wmes.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The resulting term.</returns>
        public override Term Evaluate(List<WME> values)
        {
            double? d = null;
            foreach (WME item in values)
            {
                if (item.Attribute.Equals(MatchPredicate) && (item.Value.TermType == TermType.Double || item.Value.TermType == TermType.Integer))
                {
                    double tmpDbl = Convert.ToDouble(item.Value.Value);
                    if (d.HasValue)
                    {
                        if (d < tmpDbl)
                        {
                            d = tmpDbl;
                        }
                    }
                    else
                    {
                        d = tmpDbl;
                    }
                }
            }
            return new DoubleTerm(d);
        }

        #endregion
    }
}