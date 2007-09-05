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
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Base class for all Aggregators
    /// </summary>
    public abstract class BaseAggregator : IAggregator
    {
        #region Fields

        private StringTerm _alias;
        private Term _matchPredicate;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Count"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public BaseAggregator(Term matchPredicate, StringTerm alias)
        {
            _matchPredicate = matchPredicate;
            _alias = alias;
        }

        #endregion

        #region IAggregator Members

        /// <summary>
        /// Gets or sets the term upon which the evaluation will be performed.
        /// </summary>
        /// <value>The match predicate.</value>
        public Term MatchPredicate
        {
            get { return _matchPredicate; }
            set { _matchPredicate = value; }
        }

        /// <summary>
        /// Gets or sets the alias the will be used when generating inferred facts.
        /// </summary>
        /// <value>The Alias.</value>
        /// <remarks>Will default to the name of the aggregate function. For example 'Count'.</remarks>
        public StringTerm Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        #region Abstract

        /// <summary>
        /// Evaluates the specified list of wmes.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The resulting term.</returns>
        public abstract Term Evaluate(List<WME> values);

        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _alias.Value;
        }

        #endregion
    }
}