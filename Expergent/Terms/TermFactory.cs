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
using System.Collections;
using System.Collections.Generic;

namespace Expergent.Terms
{
    public sealed class TermFactory
    {
        private static readonly TermFactory instance = new TermFactory();
        private Dictionary<Type, Type> _typeDictionary;

        static TermFactory()
        {
        }

        private TermFactory()
        {
            _typeDictionary = new Dictionary<Type, Type>(10);
            // Strings
            _typeDictionary.Add(typeof (string), typeof (StringTerm));
            _typeDictionary.Add(typeof (char), typeof (StringTerm));
            // Bools
            _typeDictionary.Add(typeof (Boolean), typeof (BooleanTerm));
            // numbers not int
            _typeDictionary.Add(typeof (Double), typeof (DoubleTerm));
            _typeDictionary.Add(typeof (Single), typeof (DoubleTerm));
            _typeDictionary.Add(typeof (decimal), typeof (DoubleTerm));
            // ints
            _typeDictionary.Add(typeof (Int16), typeof (IntegerTerm));
            _typeDictionary.Add(typeof (Int32), typeof (IntegerTerm));
            _typeDictionary.Add(typeof (Int64), typeof (IntegerTerm));
            // dates
            _typeDictionary.Add(typeof (DateTime), typeof (DateTimeTerm));
            // Guid
            _typeDictionary.Add(typeof (Guid), typeof (GuidTerm));
        }

        public static TermFactory Instance
        {
            get { return instance; }
        }

        public Term Create(object obj)
        {
            if (obj == null)
            {
                return new NullTerm(null);
            }

            Term arg = obj as Term;
            if (arg != null)
            {
                return arg;
            }

            ICollection collection = obj as ICollection;
            if (collection != null)
            {
                return new ListTerm(collection);
            }

            return (Term) Activator.CreateInstance(GetTermType(obj), obj);
        }

        private Type GetTermType(object obj)
        {
            if (_typeDictionary.ContainsKey(obj.GetType()))
            {
                return _typeDictionary[obj.GetType()];
            }
            return typeof (ObjectTerm);
        }
    }
}