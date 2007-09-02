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

namespace Expergent.Reflection
{
    ///<summary>A object instance
    ///</summary>
    public class ObjectInstance
    {
        #region properties

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <value>The handle.</value>
        public virtual String Handle
        {
            get { return handle; }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>The object.</value>
        public virtual Object Object
        {
            get { return obj; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInstance"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="obj1">The obj1.</param>
        public ObjectInstance(String s, Object obj1)
        {
            handle = s;
            obj = obj1;
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override String ToString()
        {
            String s = new String("".ToCharArray());
            s = s + "ObjectInstance: ";
            s = s + " Handle = " + handle;
            s = s + " Object = " + obj;
            return s;
        }

        #endregion

        private Object obj;
        private String handle;
    }
}