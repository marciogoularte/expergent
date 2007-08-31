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
using System.IO;
using System.Xml;

namespace Expergent.Reflection
{
    public class ObjectMapTable
    {
        #region properties

        public virtual int Status
        {
            get { return status; }
        }

        public virtual Hashtable ObjectMapHashtable
        {
            get { return objectMapHashTable; }
        }

        public virtual Hashtable PredicateMapHashtable
        {
            get { return predicateMapHashTable; }
        }

        public ArrayList MapList
        {
            get { return _objectMapList; }
        }

        #endregion

        #region constructors

        public ObjectMapTable()
        {
            _objectMapList = new ArrayList();
            objectMapHashTable = new Hashtable();
            predicateMapHashTable = new Hashtable();
            status = 0;
        }

        public ObjectMapTable(TextReader bufferedReader)
            : this()
        {
            try
            {
                XmlDocument XmlData = new XmlDocument();
                XmlData.LoadXml(bufferedReader.ReadToEnd());

                //add projects to treeview

                XmlNodeList Classes = XmlData.SelectNodes("ObjectMap/Class");
                if (Classes != null && Classes.Count > 0)
                {
                    foreach (XmlElement nodeClass in Classes)
                    {
                        // Add path and class to the object mapping
                        ObjectMapping theMapping = new ObjectMapping(nodeClass);
                        _objectMapList.Add(theMapping);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error reading map file", ex);
            }
            finally
            {
                bufferedReader.Close();
            }
        }

        public ObjectMapTable(String s)
            : this()
        {
            TextReader bufferedreader = new StreamReader(s);
            try
            {
                XmlDocument XmlData = new XmlDocument();
                XmlData.LoadXml(bufferedreader.ReadToEnd());

                //add projects to treeview

                XmlNodeList Classes = XmlData.SelectNodes("ObjectMap/Class");
                if (Classes != null && Classes.Count > 0)
                {
                    foreach (XmlElement nodeClass in Classes)
                    {
                        // Add path and class to the object mapping
                        ObjectMapping theMapping = new ObjectMapping(nodeClass);
                        _objectMapList.Add(theMapping);
                    }
                }


                bufferedreader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("error reading map file", ex);
            }
            finally
            {
                bufferedreader.Close();
            }
        }

        #endregion

        #region methods

        public virtual void addEntry(String s, String s1)
        {
            objectMapHashTable[s] = s1;
            predicateMapHashTable[s1] = s;
        }

        public void removeEntry(String s, String s1)
        {
            objectMapHashTable.Remove(s);
            predicateMapHashTable.Remove(s1);
        }

        public virtual void clear()
        {
            objectMapHashTable.Clear();
            predicateMapHashTable.Clear();
        }

        public virtual String getPredicateName(String s)
        {
            return (String) objectMapHashTable[s];
        }

        public virtual String getObjectName(String s)
        {
            return (String) predicateMapHashTable[s];
        }

        public ArrayList getObjectMappingsByClassName(String s)
        {
            ArrayList classMapping = new ArrayList();
            foreach (ObjectMapping eachObjectMapping in _objectMapList)
            {
                if (s.Equals(eachObjectMapping.getClassName()))
                {
                    classMapping.Add(eachObjectMapping);
                    return classMapping;
                }
            }

            return classMapping;
        }

        public override String ToString()
        {
            return string.Format("{0}\n{1}end\n", objectMapHashTable, predicateMapHashTable);
        }

        #endregion

        #region member variables

        private Hashtable objectMapHashTable;
        private Hashtable predicateMapHashTable;
        private ArrayList _objectMapList;
        private int status;

        #endregion
    }
}