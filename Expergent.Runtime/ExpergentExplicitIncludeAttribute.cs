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
    ///<summary>Forces an Include
    ///</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExpergentExplicitIncludeAttribute : Attribute
    {
        // Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpergentExplicitIncludeAttribute"/> class.
        /// </summary>
        /// <param name="ApplyToInheritingClass">if set to <c>true</c> [apply to inheriting class].</param>
        public ExpergentExplicitIncludeAttribute(bool ApplyToInheritingClass)
        {
            _ApplyToInheritingClass = ApplyToInheritingClass;
        }


        // Properties
        /// <summary>
        /// Gets a value indicating whether [apply to inheriting class].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [apply to inheriting class]; otherwise, <c>false</c>.
        /// </value>
        public bool ApplyToInheritingClass
        {
            get { return _ApplyToInheritingClass; }
        }


        // Fields
        private bool _ApplyToInheritingClass;
    }
}