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

namespace Expergent.Conditions
{
    ///<summary>The Condition Type
    ///</summary>
    public enum ConditionType
    {
        /// <summary>
        /// Assert
        /// </summary>
        Assert,
        /// <summary>
        /// Function
        /// </summary>
        Function,
        /// <summary>
        /// Invoke
        /// </summary>
        Invoke,
        /// <summary>
        /// NCC
        /// </summary>
        NCC,
        /// <summary>
        /// Negative
        /// </summary>
        Negative,
        /// <summary>
        /// Positive
        /// </summary>
        Positive,
        /// <summary>
        /// Set
        /// </summary>
        Set,
        /// <summary>
        /// Retract
        /// </summary>
        Retract
    }
}