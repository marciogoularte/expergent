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

namespace Expergent.Runtime
{
    ///<summary>Stop Attribute
    ///</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExpergentStopAttribute : Attribute
    {
        // Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpergentStopAttribute"/> class.
        /// </summary>
        /// <param name="StopBaseInspection">if set to <c>true</c> [stop base inspection].</param>
        public ExpergentStopAttribute(bool StopBaseInspection)
        {
            _stop = true;
            _stop = StopBaseInspection;
        }


        // Properties
        /// <summary>
        /// Gets a value indicating whether [stop base inspection].
        /// </summary>
        /// <value><c>true</c> if [stop base inspection]; otherwise, <c>false</c>.</value>
        public bool StopBaseInspection
        {
            get { return _stop; }
        }


        // Fields
        private bool _stop;
    }
}