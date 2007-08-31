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
    public interface IMutexEvaluator
    {
        /// <summary>
        /// Evaluates the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        List<Activation> Evaluate(IEnumerable<Token> items);

        /// <summary>
        /// Performs the evaluation.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="o1">The o1.</param>
        /// <returns>The result</returns>
        bool PerformEvaluation(Term o, Term o1);

        /// <summary>
        /// Gets or sets the conditional.
        /// </summary>
        /// <value>The conditional.</value>
        int Conditional { get; set; }

        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        Term Predicate { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        int Subject { get; set; }
    }
}