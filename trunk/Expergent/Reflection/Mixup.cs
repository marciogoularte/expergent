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
using System.Reflection;
using Expergent.Interfaces;
using Expergent.Terms;
using Neo.Framework;

namespace Expergent.Reflection
{
    public class Mixup : IFactProvider
    {
        private static BindingFlags defaultBF = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        private static BindingFlags invokeBF = defaultBF | BindingFlags.InvokeMethod;
        private static BindingFlags getPropertyBF = defaultBF | BindingFlags.GetProperty | BindingFlags.GetField;
        private static BindingFlags setPropertyBF = defaultBF | BindingFlags.SetProperty | BindingFlags.SetField;
        private Dictionary<string, object> methods;
        private Dictionary<string, object> properties;
        private List<WME> facts;
        private Type type;

        //public Mixup()
        //{
        //    methods = new Dictionary<string, object>();
        //    properties = new Dictionary<string, object>();
        //    facts = new List<WME>();
        //    type = this.GetType();
        //    MixItUp(this);
        //}

        public Mixup(object o)
        {
            methods = new Dictionary<string, object>();
            properties = new Dictionary<string, object>();
            facts = new List<WME>();
            type = o.GetType();
            MixItUp(o);
        }

        private void MixItUp(object o)
        {
            Type t = o.GetType();
            foreach (MethodInfo m in t.GetMethods(invokeBF))
            {
                if (methods.ContainsKey(m.Name) == false)
                {
                    methods.Add(m.Name, o);
                }
            }
            foreach (PropertyInfo p in t.GetProperties(getPropertyBF))
            {
                if (properties.ContainsKey(p.Name) == false)
                {
                    properties.Add(p.Name, o);
                }
            }
            foreach (FieldInfo f in t.GetFields(getPropertyBF))
            {
                if (properties.ContainsKey(f.Name) == false)
                {
                    properties.Add(f.Name, o);
                }
            }
            foreach (string p in properties.Keys)
            {
                facts.Add(new WME(new ObjectTerm(o), p, makeTerm(GetProperty(p))));
            }
        }

        private Term makeTerm(object o)
        {
            return TermFactory.Instance.Create(o);
        }

        #region IFactProvider Members

        public void SetProperty(string propName, object propValue)
        {
            object o = properties[propName];
            if (o == null)
            {
                throw new InvalidOperationException(String.Format("Property '{0}' not found in class '{1}'.", propName, type));
            }
            o.GetType().InvokeMember(propName, setPropertyBF, null, o, new object[] {propValue});
        }

        public object GetProperty(string propName)
        {
            object o = properties[propName];
            if (o == null)
            {
                throw new InvalidOperationException(String.Format("Property '{0}' not found in class '{1}'.", propName, type));
            }
            return o.GetType().InvokeMember(propName, getPropertyBF, null, o, null);
        }

        public void InvokeMethod(string methodName, params object[] parameters)
        {
            object o = methods[methodName];
            if (o == null)
            {
                throw new InvalidOperationException(String.Format("Method '{0}' not found in class '{1}'.", methodName, type));
            }
            o.GetType().InvokeMember(methodName, invokeBF, null, o, parameters);
        }

        public bool MyFactsHaveBeenAsserted
        {
            get { return false; }
        }

        public IList<WME> GenerateFactsForRelatedObject(string parent, IFactProvider parentObject)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<WME> GenerateFactsForObjectInCollection(string parent, ObjectRelationBase parentObject)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<WME> GenerateFactsForRootObject()
        {
            return facts;
        }

        #endregion
    }
}