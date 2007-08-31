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
using System.Text;
using System.Xml;

namespace Expergent.Reflection
{
    internal class ObjectMapping
    {
        #region "Private Constants"

        private const string PredicateName = "name";
        private const string FullClassPath = "path";
        private const string AttributeName = "name";
        private const string AttributeAlias = "alias";
        private const string isContained = "isContained";
        private const string containedBy = "containedBy";
        private const string isList = "isList";
        private const string listOf = "listOf";
        private const string isPrimative = "isPrimative";

        #endregion

        #region "Private Members"

        private String className;
        private String predicateName;

        private bool isContained_ = false; /* Is this object in a list? */
        private bool isList_ = false; /* Is this object a list?    */

        private String containedBy_ = ""; /* List type that contains this object */
        private String listOf_ = ""; /* List of which type */
        private bool isPrimative_ = false; /* Applies to list.  Is a list of primatives */

        private ArrayList attNames;
        private ArrayList varNames;
        private XmlElement objectMapNode_;

        #endregion

        #region "Constructors"

        public ObjectMapping(XmlElement mapNode)
        {
            attNames = new ArrayList();
            varNames = new ArrayList();

            objectMapNode_ = mapNode;
            className = objectMapNode_.GetAttribute(FullClassPath);
            predicateName = objectMapNode_.GetAttribute(PredicateName);

            // Contained?  If so, by whom?
            string isCont = objectMapNode_.GetAttribute(isContained);
            if (isCont != string.Empty)
                isContained_ = bool.Parse(objectMapNode_.GetAttribute(isContained));

            containedBy_ = objectMapNode_.GetAttribute(containedBy);

            // A container? If so, what type do you contain?
            string isLst = objectMapNode_.GetAttribute(isList);
            if (isLst != string.Empty)
                isList_ = bool.Parse(isLst);

            listOf_ = objectMapNode_.GetAttribute(listOf);

            // Is a list of primatives
            string isPrim = objectMapNode_.GetAttribute(isPrimative);
            if (isPrim != string.Empty)
                isPrimative_ = bool.Parse(isPrim);

            XmlNodeList Properties = objectMapNode_.SelectNodes("./Property");
            foreach (XmlElement nodeProperty in Properties)
            {
                // add the property name and alias
                addEntry(nodeProperty.GetAttribute(AttributeName),
                         nodeProperty.GetAttribute(AttributeAlias));
            }
        }

        public ObjectMapping(string path_, string class_)
        {
            attNames = new ArrayList();
            varNames = new ArrayList();

            className = path_;
            predicateName = class_;
        }

        #endregion

        #region "Public Methods"

        public string Predicate
        {
            get { return predicateName; }
        }

        public bool IsContained
        {
            get { return isContained_; }
        }

        public bool IsList
        {
            get { return isList_; }
        }

        public String ContainedBy
        {
            get { return containedBy_; }
        }

        public String ListOf
        {
            get { return listOf_; }
        }

        public bool IsPrimative
        {
            get { return isPrimative_; }
        }

        public void addEntry(String s, String s1)
        {
            attNames.Add(s);
            varNames.Add(s1);
        }

        public ArrayList getAttNames()
        {
            return attNames;
        }

        public String getClassName()
        {
            return className;
        }

        public String getPredicateName()
        {
            return predicateName;
        }

        public String getVarName(String s)
        {
            for (int i = 0; i < attNames.Count; i++)
            {
                String s1 = (String) attNames[i];
                if (s1.Equals(s))
                    return (String) varNames[i];
            }

            return null;
        }

        public String getAttName(String s)
        {
            for (int i = 0; i < varNames.Count; i++)
            {
                String s1 = (String) varNames[i];
                if (s1.Equals(s))
                    return (String) attNames[i];
            }

            return null;
        }

        public String getAttName(int i)
        {
            return (String) attNames[i];
        }

        public String getVarName(int i)
        {
            return (String) varNames[i];
        }

        public string ToClString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(predicateName);
            sb.Append("(?");
            sb.Append(predicateName);

            if (isContained_)
            {
                sb.Append(", ?");
                sb.Append(containedBy_);
            }
            for (int idx = 0; idx < attNames.Count; idx++)
            {
                if (idx != attNames.Count)
                    sb.Append(", ");
                sb.Append("?");
                sb.Append(attNames[idx]);
            }
            sb.Append(")");

            return sb.ToString();
        }

        public override String ToString()
        {
            String s = "ClassName: " + className + '\n';
            s = s + "PredName: " + predicateName + '\n';
            s = s + "Attribute map: \n";
            for (int i = 0; i < attNames.Count; i++)
                s = s + (String) attNames[i] + ' ' + (String) varNames[i] + '\n';

            return s;
        }

        #endregion
    }
}