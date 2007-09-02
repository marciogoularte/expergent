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
    ///<summary>Provides info
    ///</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExpergentInfoAttribute : Attribute
    {
        // Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpergentInfoAttribute"/> class.
        /// </summary>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="ReturnType">Type of the return.</param>
        /// <param name="UseFriendlyName">if set to <c>true</c> [use friendly name].</param>
        public ExpergentInfoAttribute(string FriendlyName, Type ReturnType, bool UseFriendlyName)
        {
            _FriendlyName = FriendlyName;
            _ReturnType = ReturnType;
            _UseFriendlyName = UseFriendlyName;
        }


        // Properties
        /// <summary>
        /// Gets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName
        {
            get { return _FriendlyName; }
        }

        /// <summary>
        /// Gets the type of the return.
        /// </summary>
        /// <value>The type of the return.</value>
        public Type ReturnType
        {
            get { return _ReturnType; }
        }

        /// <summary>
        /// Gets a value indicating whether [use friendly name].
        /// </summary>
        /// <value><c>true</c> if [use friendly name]; otherwise, <c>false</c>.</value>
        public bool UseFriendlyName
        {
            get { return _UseFriendlyName; }
        }


        // Fields
        private string _FriendlyName;
        private Type _ReturnType;
        private bool _UseFriendlyName;
    }
}