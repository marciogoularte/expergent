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

namespace Expergent.Interfaces
{
    /// <summary>
    /// The IAggregator Interface
    /// </summary>
    public interface IAggregator
    {
        /// <summary>
        /// Gets or sets the term upon which the evaluation will be performed.
        /// </summary>
        /// <value>The match predicate.</value>
        Term MatchPredicate { get; set; }

        /// <summary>
        /// Gets or sets the alias the will be used when generating inferred facts.
        /// </summary>
        /// <remarks>Will default to the name of the aggregate function. For example 'Count'.</remarks>
        /// <value>The Alias.</value>
        StringTerm Alias { get; set; }

        /// <summary>
        /// Performs the evaluation on the list of wme's
        /// </summary>
        /// <param name="values">The list of wmes.</param>
        /// <returns>The resulting term.</returns>
        Term Evaluate(List<WME> values);
    }
}