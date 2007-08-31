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
    internal class RuleObjectInfo
    {
        public RuleObjectInfo()
        {
            _path = "";
            _name = "";
            _type = null;
            _CLPType = null;
            _obj = null;
            _value = null;
            _ordinal = 0;
            _depth = 0;
        }

        public object Obj
        {
            get { return _obj; }
            set { _obj = value; }
        }

        public int Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string CLPType
        {
            get { return _CLPType; }
            set { _CLPType = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        public int Arity
        {
            get { return _arity; }
            set { _arity = value; }
        }

        private string _CLPType;
        private string _path;
        private string _name;
        private Type _type;
        private Object _obj;
        private Object _value;
        private int _ordinal;
        private int _depth;
        private int _arity;
    }
}