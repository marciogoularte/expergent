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

using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Counts the elements in a list of wmes
    /// </summary>
    public class Count : BaseAggregator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Count"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        public Count(Term matchPredicate) : base(matchPredicate, "Count")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Count"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public Count(Term matchPredicate, StringTerm alias) : base(matchPredicate, alias)
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
            int i = 0;
            foreach (WME item in values)
            {
                if (item.Attribute.Equals(MatchPredicate))
                {
                    i++;
                }
            }
            return new IntegerTerm(i);
        }

        #endregion
    }
}