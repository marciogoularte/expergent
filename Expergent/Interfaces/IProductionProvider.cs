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

namespace Expergent.Interfaces
{
    ///<summary>The IProductionProvider Interface
    ///</summary>
    public interface IProductionProvider
    {
        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>The effective date.</value>
        DateTime EffectiveDate { get; }

        /// <summary>
        /// Gets or sets the termination date.
        /// </summary>
        /// <value>The termination date.</value>
        DateTime TerminationDate { get; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        String Label { get; }

        /// <summary>
        /// Gets the list of Productions.
        /// </summary>
        /// <returns>Productions</returns>
        List<Production> GetProductions();

        /// <summary>
        /// Gets the list of overrides.
        /// </summary>
        /// <returns></returns>
        List<Override> GetOverrides();

        /// <summary>
        /// Gets the list of mutexes.
        /// </summary>
        /// <returns></returns>
        List<Mutex> GetMutexes();

        /// <summary>
        /// Gets the list of aggregators.
        /// </summary>
        /// <returns></returns>
        List<Aggregator> GetAggregators();
    }
}