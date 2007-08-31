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
using System.Reflection;
using System.Text;
using Expergent.Terms;

namespace Expergent.Reflection
{
    internal class ObjectInterface
    {
        private ObjectMapTable objectmaptable;
        private bool Verbose = false;
        private bool CaptureDataValues = false;
        private StringBuilder _MessageText = new StringBuilder("");
        private string _LogFile;

        public ObjectInterface(string LogFile)
        {
            CaptureDataValues = false;
            Verbose = false;
            _LogFile = LogFile;
        }

        public ObjectInterface(string LogFile, bool verbose, bool captureData)
        {
            CaptureDataValues = captureData;
            Verbose = verbose;
            _LogFile = LogFile;
        }

        public List<WME> createFactSetFromObjectInstance(Object obj, ObjectMapTable objectmaptable)
        {
            List<WME> list = new List<WME>();
            if (obj is IEnumerable && !(obj is string))
            {
                if (obj is Hashtable)
                {
                    foreach (object eachObject in ((IDictionary) obj).Values)
                    {
                        list.AddRange(
                            createFactSetFromObjectInstance(eachObject, objectmaptable, obj));
                    }
                }
                else
                {
                    foreach (object eachObject in (IEnumerable) obj)
                    {
                        list.AddRange(
                            createFactSetFromObjectInstance(eachObject, objectmaptable, obj));
                    }
                }
            }

            list.AddRange(createFactSetFromObjectInstance(obj, objectmaptable, null));

            return list;
        }

        /// <summary>
        /// use the objectmaptable to create the facts
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objectMapTable"></param>
        /// <returns></returns>
        public List<WME> createFactSetFromObjectInstance(Object obj, ObjectMapTable objectMapTable, Object parentObj)
        {
            objectmaptable = objectMapTable;
            ArrayList mappedPredicateNames = null;
            WME cliteral = null;
            List<WME> cliteralList = new List<WME>();

            /// TODO:  Fix it
            if (obj == null)
                return cliteralList;

            if (objectmaptable != null)
            {
                mappedPredicateNames = objectmaptable.getObjectMappingsByClassName(obj.GetType().FullName);

                if (mappedPredicateNames.Count > 0)
                {
                    //loop through the object mappings for each class and create the List<WME>
                    //

                    #region build cliterals from an object mapping

                    foreach (ObjectMapping eachMapping in mappedPredicateNames)
                    {
                        bool bParentMapped = false;

                        // Add the object reference
                        List<Term> termlist = new List<Term>();
                        termlist.Add(new ObjectTerm(obj));

                        // Add the parent object reference if contained in a list
                        if ((eachMapping.IsContained) && (!bParentMapped))
                        {
                            if (parentObj != null)
                                termlist.Add(new ObjectTerm(parentObj));
                            else
                                termlist.Add(new StringTerm(""));
                            bParentMapped = true;
                        }

                        //ALiteral aliteral = new ALiteral(eachMapping.getPredicateName());
                        //cliteral = new CLiteral(1, aliteral);
                        //cliteral.ArgList = (termlist);

                        foreach (string eachPred in eachMapping.getAttNames())
                        {
                            PropertyInfo theProp = obj.GetType().GetProperty(eachPred);
                            if (theProp == null)
                            {
                                termlist.Add(new StringTerm(""));
                            }
                            else
                            {
                                object objProp = theProp.GetValue(obj, BindingFlags.GetProperty, null, null, null);
                                if (objProp == null)
                                    objProp = String.Empty;

                                switch (theProp.PropertyType.Name)
                                {
                                    case "String[]":
                                        List<Term> termList1 = new List<Term>();
                                        foreach (string eachString in (String[]) objProp)
                                        {
                                            termList1.Add(new StringTerm(eachString));
                                        }
                                        //FuncTerm functerm = new FuncTerm("list", termList1);
                                        //termlist.Add(functerm);
                                        break;

                                    case "String":
                                        termlist.Add(new StringTerm(objProp.ToString()));
                                        break;
                                    case ("Int32"):
                                        termlist.Add(new IntegerTerm((int) objProp));
                                        break;
                                    case ("DateTime"):
                                        termlist.Add(new DateTimeTerm((DateTime) objProp));
                                        break;
                                    case ("Double"):
                                        termlist.Add(new DoubleTerm((double) objProp));
                                        break;
                                    case ("ArrayList"):
                                        List<Term> termList2_ = new List<Term>();
                                        foreach (string eachString in (IEnumerable) objProp)
                                        {
                                            termList2_.Add(new StringTerm(eachString));
                                        }
                                        //FuncTerm functerm2_ = new FuncTerm("list", termList2_);
                                        //termlist.Add(functerm2_);

                                        foreach (object eachObject in (IEnumerable) objProp)
                                        {
                                            if (!eachObject.GetType().IsPrimitive)
                                            {
                                                List<WME> clist =
                                                    createFactSetFromObjectInstance(eachObject, objectmaptable, objProp);
                                                cliteralList.AddRange(clist);
                                            }
                                        }

                                        break;
                                    case ("Boolean"):
                                        termlist.Add(new BooleanTerm((bool) objProp));
                                        break;
                                    default:
                                        bool bAddObjectTerm = true;
                                        if (objProp is IEnumerable)
                                        {
                                            if (objProp is Hashtable)
                                            {
                                                foreach (object eachObject in ((IDictionary) objProp).Values)
                                                {
                                                    if (eachObject.GetType().FullName == "System.String")
                                                    {
                                                        //ALiteral aliteral_ = new ALiteral(objProp.GetType().Name + "Values");
                                                        //CLiteral cliteral_ = new CLiteral(1, aliteral_);

                                                        //List<Term> termlist_ = new List<Term>();
                                                        //termlist_.Add(new StringTerm(eachObject.ToString()));
                                                        //termlist_.Add(new ObjectTerm(objProp));

                                                        //cliteral_.ArgList = (termlist_);
                                                        //cliteralList.Add(cliteral_);
                                                    }
                                                    else
                                                    {
                                                        List<WME> clist =
                                                            createFactSetFromObjectInstance(eachObject, objectmaptable, objProp);
                                                        cliteralList.AddRange(clist);
                                                    }
                                                }
                                            }
                                            else if (objProp.GetType().BaseType.ToString() == "System.Collections.ArrayList")
                                            {
                                                bAddObjectTerm = false;
                                                // Map as an embedded list t
                                                List<Term> termList1_ = new List<Term>();
                                                foreach (object eachObject in (IEnumerable) objProp)
                                                {
                                                    if (eachObject is string)
                                                    {
                                                        termList1_.Add(new StringTerm(eachObject as string));
                                                    }
                                                    else
                                                    {
                                                        List<WME> clist = createFactSetFromObjectInstance(eachObject, objectmaptable, objProp);
                                                        cliteralList.AddRange(clist);
                                                    }
                                                }
                                                if (termList1_.Count > 0)
                                                {
                                                    //FuncTerm functerm_ = new FuncTerm("list", termList1_);
                                                    //termlist.Add(functerm_);
                                                }

                                                //												foreach (object eachObject in (IEnumerable) objProp)
                                                //												{
                                                //													List<WME> clist = this.createFactSetFromObjectInstance(eachObject, objectmaptable, objProp);
                                                //													cliteralList.addCLiteralList(clist);
                                                //												}
                                            }
                                            else
                                            {
                                                foreach (object eachObject in (IEnumerable) objProp)
                                                {
                                                    List<WME> clist =
                                                        createFactSetFromObjectInstance(eachObject, objectmaptable, objProp);
                                                    cliteralList.AddRange(clist);
                                                }
                                            }
                                        }

                                        if (bAddObjectTerm)
                                            termlist.Add(new ObjectTerm(objProp));
                                        List<WME> newList = createFactSetFromObjectInstance(objProp, objectmaptable);
                                        cliteralList.AddRange(newList);
                                        break;
                                } //end switch
                            } // end if prop null
                        } //end foreach eachpred
                    } //end foreach mapping

                    #endregion

                    cliteralList.Add(cliteral);
                } //if (mappedPredicateNames.Count > 0)
            } //if (objectmaptable != null)
            else
            {
                #region build cliterals without an object mapping

                return createFactSetObjectInstanceWithObjectGrapher(obj);

                #endregion
            } //else
            if (CaptureDataValues)
                WriteDataValues(_LogFile + "." + obj.GetType().FullName, cliteralList.ToString());
            return cliteralList;
        }

        public List<WME> createFactSetObjectInstanceWithObjectGrapher(object theObject)
        {
            ObjectGrapher myMap = new ObjectGrapher();
            myMap.Graph(theObject);

            //this._MessageText.Append(myMap.MessageText);
            if (Verbose)
            {
                //ExpergentLogger.LogByName(this._LogFile, myMap.MessageText);
            }

            if (CaptureDataValues)
                WriteDataValues(_LogFile + "." + theObject.GetType().FullName, myMap.CLiteralList.ToString());
            return myMap.CLiteralList;
        }

        private void WriteDataValues(string filename, string TextToWrite)
        {
            //ExpergentLogger.LogByName(filename + ".datalog", TextToWrite);
        }

        public string MessageText
        {
            get { return _MessageText.ToString(); }
        }
    }
}