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
using System.Data;
using Expergent.Interfaces;
using Expergent.Terms;
using Neo.Core;
using Neo.Core.Util;
using Neo.Framework;

namespace Expergent.Neo
{
    /// <summary>
    /// Allows Neo EntityObjects to become rules enabled
    /// </summary>
    public abstract class RulesEnabledEntityObject : EntityObject, IFactProvider
    {
        #region Fields
        /// <summary>
        /// Gets a value indicating whether the facts have been asserted for this instance.
        /// </summary>
        protected bool myFactsHaveBeenAsserted;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RulesEnabledEntityObject"/> class.
        /// Use the corresponding Factory to create new instances.
        /// </summary>
        /// <param name="aRow">A row.</param>
        /// <param name="aContext">A context.</param>
        protected RulesEnabledEntityObject(DataRow aRow, ObjectContext aContext)
            : base(aRow, aContext)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the facts have been asserted for this instance.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if my facts have been previously asserted; otherwise, <c>false</c>.
        /// </value>
        public bool MyFactsHaveBeenAsserted
        {
            get { return myFactsHaveBeenAsserted; }
        }

        #endregion

        #region Public Methods

        #region IFactProvider Implementation

        /// <summary>
        /// Invokes the indicated method, passing the method the indicated parameters.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        public virtual void InvokeMethod(string methodName, params object[] parameters)
        {
            ObjectHelper.InvokeMethod(this, methodName, parameters);
        }

        /// <summary>
        /// Sets the indicated property to the indicated value.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="propValue">The property value.</param>
        public virtual void SetProperty(string propName, object propValue)
        {
            ObjectHelper.SetProperty(this, propName, propValue);
        }

        /// <summary>
        /// Generates the set of facts for this object as the root object.
        /// </summary>
        /// <returns>An IList of facts.</returns>
        public virtual IList<WME> GenerateFactsForRootObject()
        {
            List<WME> list = new List<WME>();
            list.Add(new WME(new EntityObjectTerm(this), "$" + GetType().Name, new EntityObjectTerm(this)));
            MakeFacts(list, "$" + GetType().Name);
            myFactsHaveBeenAsserted = true;
            return list;
        }

        /// <summary>
        /// Generates the set of facts for this object as relation to a parent object.
        /// </summary>
        /// <param name="parent">The parent path.</param>
        /// <param name="parentObject">The parent object.</param>
        /// <returns>An IList of facts.</returns>
        public virtual IList<WME> GenerateFactsForRelatedObject(string parent, IFactProvider parentObject)
        {
            List<WME> list = new List<WME>();
            list.Add(new WME(new EntityObjectTerm(this), parent, new EntityObjectTerm(this)));
            MakeFacts(list, parent);
            return list;
        }

        /// <summary>
        /// Generates the facts for this object as a member of a collection.
        /// </summary>
        /// <param name="parent">The parent path.</param>
        /// <param name="parentObject">The parent object.</param>
        /// <returns>An IList of facts.</returns>
        public virtual IList<WME> GenerateFactsForObjectInCollection(string parent, ObjectRelationBase parentObject)
        {
            List<WME> list = new List<WME>();
            parent = MakePredicate(parent, GetType().Name);
            list.Add(new WME(new ObjectRelationTerm(parentObject), parent, new EntityObjectTerm(this)));
            MakeFacts(list, parent);
            return list;
        }

        #endregion

        #endregion

        #region Protected Methods

        #region Abstract Methods

        /// <summary>
        /// Extends the GetProperty method to allow the developer to add custom properties.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <returns>The property value.</returns>
        protected abstract object ExtendGetProperty(string propName);

        /// <summary>
        /// Extends the SetProperty method to allow the developer to add custom properties.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="propValue">The prop value.</param>
        protected abstract void ExtendSetProperty(string propName, object propValue);

        /// <summary>
        /// Extends the MakeFacts method to create custom facts.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="parent">The parent.</param>
        protected abstract void ExtendMakeFacts(List<WME> list, string parent);

        /// <summary>
        /// Gets the default sort order.
        /// </summary>
        /// <value>The sort order.</value>
        public abstract PropertyComparer SortOrder {get;}

        #endregion

        /// <summary>
        /// Adds a set of facts for this instance to list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="parent">The parent.</param>
        protected virtual void MakeFacts(List<WME> list, string parent)
        {
            IEntityMap emap = Context.EntityMapFactory.GetMap(GetType());
            foreach (string property in emap.Attributes)
            {
                list.Add(new WME(new EntityObjectTerm(this), MakePredicate(parent, property), MakeTerm(GetProperty(property))));
            }
            foreach (string property in emap.Relations)
            {
                try
                {
                    object val = GetProperty(property);
                    if (val is RulesEnabledEntityObject)
                    {
                        list.Add(new WME(new EntityObjectTerm(this), MakePredicate(parent, property), new EntityObjectTerm((RulesEnabledEntityObject) val)));
                    }
                    else if (val is ObjectListBase)
                    {
                        GetLogger(typeof (EntityObject)).Debug("Should never be an ObjectList.");
                    }
                    else if (val is ObjectRelationBase)
                    {
                        list.Add(new WME(new EntityObjectTerm(this), MakePredicate(parent, property), new ObjectRelationTerm((ObjectRelationBase) val)));
                    }
                    else
                    {
                        list.Add(new WME(new EntityObjectTerm(this), property, MakeTerm(val)));
                    }
                }
                catch (Exception ex)
                {
                    GetLogger(typeof (EntityObject)).Debug(ex.Message);
                }
            }
            myFactsHaveBeenAsserted = true;
        }

        /// <summary>
        /// A wrapper around the TermFactory.Instance.Create helper method.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>A Term appropriate for the type of the object parameter.</returns>
        protected Term MakeTerm(object val)
        {
            return TermFactory.Instance.Create(val);
        }

        /// <summary>
        /// Helper method to create a dot seperated predicate.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        /// <returns>A dot seperated string.</returns>
        protected string MakePredicate(string parent, string child)
        {
            return parent + "." + child;
        }

        #endregion
    }
}