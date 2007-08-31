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
using Neo.Framework;

namespace Expergent.Interfaces
{
    public interface IFactProvider
    {
        /// <summary>
        /// Sets the indicated property to the indicated value.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="propValue">The property value.</param>
        void SetProperty(string propName, object propValue);

        /// <summary>
        /// Invokes the indicated method, passing the method the indicated parameters.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        void InvokeMethod(string methodName, params object[] parameters);

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <returns>The value corresponding to the property name as an object.</returns>
        object GetProperty(string propName);

        /// <summary>
        /// Generates the set of facts for this object as the root object.
        /// </summary>
        /// <returns>An IList of facts.</returns>
        IList<WME> GenerateFactsForRootObject();

        /// <summary>
        /// Generates the set of facts for this object as relation to a parent object.
        /// </summary>
        /// <param name="parent">The parent path.</param>
        /// <param name="parentObject">The parent object.</param>
        /// <returns>An IList of facts.</returns>
        IList<WME> GenerateFactsForRelatedObject(string parent, IFactProvider parentObject);

        /// <summary>
        /// Generates the facts for this object as a member of a collection.
        /// </summary>
        /// <param name="parent">The parent path.</param>
        /// <param name="parentObject">The parent object.</param>
        /// <returns>An IList of facts.</returns>
        IList<WME> GenerateFactsForObjectInCollection(string parent, ObjectRelationBase parentObject);

        /// <summary>
        /// Gets a value indicating whether the facts have been asserted for this instance.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if my facts have been previously asserted; otherwise, <c>false</c>.
        /// </value>
        bool MyFactsHaveBeenAsserted { get; }
    }
}