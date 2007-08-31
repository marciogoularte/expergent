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
    [AttributeUsage(AttributeTargets.All)]
    public class ExpergentCollectionAttribute : Attribute
    {
        // Methods
        public ExpergentCollectionAttribute(string FriendlyName, Type ReturnType, bool UseFriendlyName, bool isObjectCollection, bool isCLPList)
        {
            _FriendlyName = FriendlyName;
            _ReturnType = ReturnType;
            _isObjectCollection = isObjectCollection;
            _isCLPList = isCLPList;
            _UseFriendlyName = UseFriendlyName;
        }


        // Properties
        public string FriendlyName
        {
            get { return _FriendlyName; }
        }

        public bool isCLPList
        {
            get { return _isCLPList; }
        }

        public bool isObjectCollection
        {
            get { return _isObjectCollection; }
        }

        public Type ReturnType
        {
            get { return _ReturnType; }
        }

        public bool UseFriendlyName
        {
            get { return _UseFriendlyName; }
        }


        // Fields
        private string _FriendlyName;
        private bool _isCLPList;
        private bool _isObjectCollection;
        private Type _ReturnType;
        private bool _UseFriendlyName;
    }
}