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
using Expergent.Runtime;
using Expergent.Terms;

namespace Expergent.Reflection
{
    internal class ObjectGrapher
    {
        /// <summary>
        /// internal class for keeping an object chain
        /// </summary>
        private class InternalRuleObjectInfo : RuleObjectInfo
        {
            private InternalRuleObjectInfo _myParent;

            internal InternalRuleObjectInfo(InternalRuleObjectInfo parentObj)
            {
                _myParent = parentObj;
            }

            public ParentID ParentID
            {
                //HACK we are going to travel up the chain to find a parent id if we can
                get
                {
                    ParentID theParentID = GetParentID();

                    if (theParentID.theInfo == null && _myParent != null)
                        return _myParent.ParentID;
                    else
                        return theParentID;
                }
            }

            private ParentID GetParentID()
            {
                if (_myParent == null)
                    return new ParentID(null, null);

                PropertyInfo[] objProps = _myParent.Obj.GetType().GetProperties();
                ParentID theParentID = new ParentID(null, null);
                foreach (PropertyInfo eachPropInfo in objProps)
                {
                    object[] theAttrs = eachPropInfo.GetCustomAttributes(typeof (ExpergentParentIDAttribute), true);
                    if (theAttrs.Length > 0)
                    {
                        theParentID.theInfo = eachPropInfo;
                        theParentID.theValue = eachPropInfo.GetValue(_myParent.Obj, BindingFlags.GetProperty, null, null, null);
                        break;
                    }
                }

                return theParentID;
            }
        }


        private struct ParentID
        {
            public PropertyInfo theInfo;
            public object theValue;

            public ParentID(PropertyInfo theInf, object theVal)
            {
                theInfo = theInf;
                theValue = theVal;
            }
        }

        #region member variables

        private List<WME> cliterallist = new List<WME>();
        //private StringBuilder messagetext = new StringBuilder("");
        //private bool _PromiscuousMode = false;
        //private ArrayList _RuleObjectInfoList = new ArrayList();
        private Hashtable _ordinalHash = new Hashtable();

        #endregion

        #region properties

        //public string MessageText
        //{
        //    get { return messagetext.ToString(); }
        //}

        public List<WME> CLiteralList
        {
            get { return cliterallist; }
        }

        //public bool PromiscuousMode
        //{
        //    get { return _PromiscuousMode; }
        //    set { _PromiscuousMode = value; }
        //}

        //public ArrayList RuleObjectInfoList
        //{
        //    get { return _RuleObjectInfoList; }
        //}

        #endregion

        #region constructors

//		public ObjectGrapher2(bool verbose, string logFile)
//		{
//			this.Verbose = verbose;
//			this._LogFile = logFile;
//		}

//		public ObjectGrapher2(bool verbose)
//		{
//			this.Verbose = verbose;
//		}

        public ObjectGrapher()
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// graph an object and build the facts of the object
        /// </summary>
        /// <param name="theObj"></param>
        public void Graph(object theObj)
        {
            InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(null);

            if (theObj != null)
            {
                theInfo.Obj = theObj;
                theInfo.Type = theObj.GetType();
                theInfo.Path = theObj.GetType().FullName;
                theInfo.Value = theObj;

                MakeObjectTerm(theInfo);
            }
            GraphChildren(theInfo);
        }

        #endregion

        #region private methods

        #region Graphing methods

        /// <summary>
        /// graph the children of this object
        /// </summary>
        /// <param name="parentObj"></param>
        private void GraphChildren(InternalRuleObjectInfo parentObj)
        {
//			ParentID theParent = GetParentID(parentObj);
            if (parentObj.Obj is IEnumerable && !(parentObj.Obj is string))
                GraphCollection(parentObj);
            else
                GraphProperties(parentObj, false);
        }

        /// <summary>
        /// graph the public properties of the object
        /// </summary>
        private void GraphProperties(InternalRuleObjectInfo parentInfo, bool graphCollectionProperties)
        {
            PropertyInfo[] theProperties = parentInfo.Obj.GetType().GetProperties();

            foreach (PropertyInfo eachProp in theProperties)
            {
                //check for the expergent stop attribute
                object[] theAttrs = eachProp.GetCustomAttributes(typeof (ExpergentStopAttribute), true);
                if (theAttrs.Length > 0 && ((ExpergentStopAttribute) theAttrs[0]).StopBaseInspection)
                    continue;

                object theVal;
                //HACK try to get the value, sometimes it fails, just keep on if it does
                try
                {
                    //skip the Item Property
                    if (!eachProp.Name.Equals("Item") && !eachProp.Name.Equals("SyncRoot"))
                        theVal = eachProp.GetValue(parentInfo.Obj, BindingFlags.GetProperty, null, null, null);
                    else
                        continue;
                }
                catch //(Exception ex)
                {
                    //ExpergentLogger.LogError(parentInfo.Path + "." + eachProp.Name + " has thrown an error " + Environment.NewLine + ex.ToString());
                    continue;
                }

                //a string is not a primitive type so we must check for it explicitly
                if ((eachProp.PropertyType.IsPrimitive || theVal is string || eachProp.PropertyType.IsValueType) && !eachProp.IsSpecialName)
                {
                    int startOrd = parentInfo.Ordinal;

                    if (_ordinalHash.ContainsKey(parentInfo.Path))
                        startOrd = (int) _ordinalHash[parentInfo.Path];

                    InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(parentInfo);
                    theInfo.Obj = parentInfo.Obj;
                    theInfo.Type = eachProp.PropertyType;
                    theInfo.Path = parentInfo.Path + "." + eachProp.Name;
                    theInfo.Value = theVal;
//					theInfo.Ordinal = parentInfo.Ordinal;
                    theInfo.Ordinal = startOrd;
                    AddTerm(theInfo);


                    continue;
                }

                    //this is not one of the primitive types, it must be an object type 
                    //and we must graph its children
                else
                {
                    //the value is null don't graph it
                    if (theVal == null)
                        continue;

                    InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(parentInfo);
                    theInfo.Obj = theVal;
                    theInfo.Type = eachProp.PropertyType;
                    theInfo.Path = parentInfo.Path + "." + eachProp.Name;
                    theInfo.Value = theVal;
                    theInfo.Depth = parentInfo.Depth + 1;
                    theInfo.Ordinal = parentInfo.Ordinal;

                    //check to see if the object is IEnumerable 
                    //is so then it is a collection and we will have to build all the objects in the collection
                    if (theVal is IEnumerable && !graphCollectionProperties)
                    {
                        MakeObjectTerm(theInfo);
                        GraphCollection(theInfo);
                        //go ahead and skip to the next property 
                        //we dont want to make an object term or graph the properties of a collection
                        //at this point
                        continue;
                    }

                    //get my parents ID property 
//					ParentID newParent = GetParentID(parentInfo);
                    //graph the properties of this complex object and tie it back to the parent through its parentID
                    GraphProperties(theInfo, false);
                    MakeObjectTerm(theInfo);
                }
            } //end foreach
            AddParentID(parentInfo, graphCollectionProperties);
        }

        /// <summary>
        /// add the parent id as a term object
        /// </summary>
        /// <param name="theParent"></param>
        /// <param name="isCollection"></param>
        private void AddParentID(InternalRuleObjectInfo theParent, bool isCollection)
        {
            //manually add in the ParentID
            if (theParent.ParentID.theInfo != null)
            {
                InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(theParent);
                theInfo.Obj = theParent.Obj;
                theInfo.Type = theParent.ParentID.theInfo.PropertyType;
                if (isCollection)
                    theInfo.Path = theParent.Path + ".List.ParentID";
                else
                    theInfo.Path = theParent.Path + ".ParentID";
                theInfo.Value = theParent.ParentID.theValue;
                theInfo.Ordinal = theParent.Ordinal;
                AddTerm(theInfo);
            }
        }

/*
		/// <summary>
		/// find the parent ID of an object
		/// </summary>
		/// <param name="parentInfo"></param>
		/// <returns></returns>
		private ParentID GetParentID(InternalRuleObjectInfo parentInfo)
		{
			PropertyInfo[] objProps = parentInfo.Obj.GetType().GetProperties();
			ParentID theParentID = new ParentID(null, null);
			foreach(PropertyInfo eachPropInfo in objProps)
			{
				object[] theAttrs = eachPropInfo.GetCustomAttributes(typeof(Expergent.Tester.Runtime.ExpergentParentIDAttribute), true);
				if(theAttrs.Length > 0)
				{
					theParentID.theInfo = eachPropInfo;
					theParentID.theValue = eachPropInfo.GetValue(parentInfo.Obj, BindingFlags.GetProperty, null, null, null);
					break;
				}
			}

			return theParentID;
		}
*/

        #region Collection Graphing Methods

        /// <summary>
        /// Graph a collection and all objects within the collection
        /// </summary>
        /// <param name="parentInfo"></param>
        private void GraphCollection(InternalRuleObjectInfo parentInfo)
        {
            //Hashtables work on keys and enumerate differently
            //so we have to graph them a little differently
            if (parentInfo.Obj is Hashtable)
                BuildHashtable(parentInfo, 1);
            else
                BuildCollection(parentInfo, 1);
        }

        /// <summary>
        /// Graph a collection and all objects within the collection
        /// </summary>
        /// <param name="parentInfo">The parent info.</param>
        /// <param name="startOrdinal">The start ordinal.</param>
        private void GraphCollection(InternalRuleObjectInfo parentInfo, int startOrdinal)
        {
            //Hashtables and NameObjectCollectionBase work on keys and enumerate differently
            //so we have to graph them a little differently
            if (parentInfo.Obj is Hashtable) // || parentInfo.Obj is NameObjectCollectionBase)
                BuildHashtable(parentInfo, startOrdinal);
            else
                BuildCollection(parentInfo, startOrdinal);
        }

        #region Hashtable Graphing

        /// <summary>
        /// build the collection
        /// first try to build a simple collection until we find a non primitive
        /// non string type
        /// </summary>
        /// <param name="parentInfo">The parent info.</param>
        /// <param name="ordinal">The ordinal.</param>
        private void BuildHashtable(InternalRuleObjectInfo parentInfo, int ordinal)
        {
            int idx = 0;
            int count = 0;
            bool buildComplex = false;

            InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(parentInfo);
            theInfo.Obj = parentInfo.Obj;
            theInfo.Type = typeof (String[]);
            theInfo.Path = parentInfo.Path;
            theInfo.Ordinal = parentInfo.Ordinal + 1;

            if (parentInfo.Obj is ICollection)
                count = ((ICollection) parentInfo.Obj).Count;

            if (count == 0)
                return;

            string[] theValues = new string[count];

            foreach (object eachObject in ((IDictionary) parentInfo.Obj).Values)
            {
                //this is a primitive type or a string 
                if (eachObject.GetType().IsPrimitive || eachObject is string)
                {
                    theValues[idx] = eachObject.ToString();
                    ++idx;
                    continue;
                }
                else
                {
                    //we found a complex type in our collection break out of this and 
                    //build a complex collection instead
                    buildComplex = true;
                    break;
                }
            }

            if (!buildComplex)
            {
                theInfo.Value = theValues;
                GraphProperties(parentInfo, true);
                AddTerm(theInfo);
            }

            //build a complex array 
            if (buildComplex)
            {
                BuildComplexCollection(parentInfo, ordinal);
            }
        }

/*
		/// <summary>
		/// build a complex collection
		/// </summary>
		/// <param name="parentInfo"></param>
		private void BuildComplexHashtable(InternalRuleObjectInfo parentInfo, int ordinal)
		{
			int idx = ordinal;
			int startOrd = 0;

//			ParentID theParentID = GetParentID(parentInfo);

			foreach(object eachObject in ((System.Collections.IDictionary)parentInfo.Obj).Values)
			{
			
				if(_ordinalHash.ContainsKey(parentInfo.Path))
					startOrd = (int)_ordinalHash[parentInfo.Path];
				else
					startOrd = 1;

				InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(parentInfo);
				theInfo.Obj = eachObject;
				theInfo.Type = eachObject.GetType();
				theInfo.Path = parentInfo.Path + "." + eachObject.GetType().Name;
				theInfo.Value = eachObject;
				theInfo.Depth = parentInfo.Depth + 1;
				theInfo.Ordinal = parentInfo.Ordinal + idx;

				startOrd += 1;
				if(_ordinalHash.ContainsKey(parentInfo.Path))
					_ordinalHash[parentInfo.Path] = startOrd;
				else
					_ordinalHash.Add(parentInfo.Path, startOrd);


				//check to see if the object is IEnumerable 
				//is so then it is a collection and we will have to build all the objects in the collection
				//oh boy oh boy, we got a collection within a collection
				if(eachObject is System.Collections.IEnumerable)
				{
					GraphCollection(theInfo, startOrd);
					//get the startOrdinal for the next pass through it if comes to it
					startOrd = ((System.Collections.ICollection)eachObject).Count;
					//go ahead and skip to the next object 
					//we dont want to make an object term or graph the properties of a collection
					//at this point
					continue;
				}
				//graph this objects children and make an object term
				GraphProperties(theInfo, false);
				++idx;
			}
		}
*/

        #endregion

        #region Collection Graphing

        /// <summary>
        /// build the collection
        /// first try to build a simple collection until we find a non primitive
        /// non string type
        /// </summary>
        /// <param name="parentInfo">The parent info.</param>
        /// <param name="ordinal">The ordinal.</param>
        private void BuildCollection(InternalRuleObjectInfo parentInfo, int ordinal)
        {
            int idx = 0;
            int count = 0;
            bool buildComplex = false;

            InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(parentInfo);
            theInfo.Obj = parentInfo.Obj;
            theInfo.Type = typeof (String[]);
            theInfo.Path = parentInfo.Path;
            theInfo.Ordinal = parentInfo.Ordinal + 1;

            if (parentInfo.Obj is ICollection)
                count = ((ICollection) parentInfo.Obj).Count;

            if (count == 0)
                return;

            string[] theValues = new string[count];

//			ParentID theParentID = GetParentID(parentInfo);

            foreach (object eachObject in (IEnumerable) parentInfo.Obj)
            {
                //check to see if the collection is a NameObjectCollectionBase
                //if so we have to call BaseGet to actually get the stored value, 
                //it only enumerates through the keys by default
                //TODO our nameobjectcollectionbase overrides the enumerator
                object internalObject = null;
                internalObject = eachObject;

                //this is a primitive type or a string 
                if (internalObject.GetType().IsPrimitive || internalObject is string)
                {
                    theValues[idx] = internalObject.ToString();
                    ++idx;
                    continue;
                }
                else
                {
                    //we found a complex type in our collection break out of this and 
                    //build a complex collection instead
                    buildComplex = true;
                    break;
                }
            }

            if (!buildComplex)
            {
                theInfo.Value = theValues;
                GraphProperties(parentInfo, true);
                AddTerm(theInfo);
                AddParentID(parentInfo, false);
            }

            //build a complex array 
            if (buildComplex)
            {
                GraphProperties(parentInfo, true);
                BuildComplexCollection(parentInfo, ordinal);
            }
        }

        /// <summary>
        /// build a complex collection
        /// </summary>
        /// <param name="parentInfo">The parent info.</param>
        /// <param name="ordinal">The ordinal.</param>
        private void BuildComplexCollection(InternalRuleObjectInfo parentInfo, int ordinal)
        {
            int idx = ordinal;
            int startOrd = 0;

//			ParentID theParentID = GetParentID(parentInfo);

            foreach (object eachObject in (IEnumerable) parentInfo.Obj)
            {
                //check to see if the collection is a NameObjectCollectionBase
                //if so we have to call BaseGet to actually get the stored value, 
                //it only enumerates through the keys by default
                object internalObject = null;
                //TODO our nameobjectcollectionbase overrides the enumerator
                internalObject = eachObject;

                if (_ordinalHash.ContainsKey(parentInfo.Path))
                    startOrd = (int) _ordinalHash[parentInfo.Path];
                else
                    startOrd = 1;

                InternalRuleObjectInfo theInfo = new InternalRuleObjectInfo(parentInfo);
                theInfo.Obj = internalObject;
                theInfo.Type = eachObject.GetType();
                theInfo.Path = parentInfo.Path + "." + eachObject.GetType().Name;
                theInfo.Value = internalObject;
                theInfo.Depth = parentInfo.Depth + 1;
                theInfo.Ordinal = parentInfo.Ordinal + idx;

                startOrd += 1;
                if (_ordinalHash.ContainsKey(parentInfo.Path))
                    _ordinalHash[parentInfo.Path] = startOrd;
                else
                    _ordinalHash.Add(parentInfo.Path, startOrd);

                //check to see if the object is IEnumerable 
                //is so then it is a collection and we will have to build all the objects in the collection
                //oh boy oh boy, we got a collection within a collection
                if (internalObject is IEnumerable)
                {
                    GraphCollection(theInfo, startOrd);
                    //get the startOrdinal for the next pass through it if comes to it
                    startOrd = ((ICollection) internalObject).Count;
                    //go ahead and skip to the next object 
                    //we dont want to make an object term or graph the properties of a collection
                    //at this point
                    continue;
                }
                //graph this objects children and make an object term
                GraphProperties(theInfo, false);
                ++idx;
            }
        }

        #endregion

        #endregion

        #endregion

        #region AddTerm and MakeTerm Methods

        /// <summary>
        /// add this term to the term list
        /// </summary>
        /// <param name="objInfo"></param>
        private void AddTerm(InternalRuleObjectInfo objInfo)
        {
            // build an Enum term
            if (objInfo.Type.IsEnum)
            {
                MakeStringTerm(objInfo);
                return;
            }

            switch (objInfo.Type.FullName)
            {
                case "System.String[]":
                    MakeListTerm(objInfo);
                    break;
                case "System.String":
                    MakeStringTerm(objInfo);
                    break;
                case ("System.Int32"):
                    MakeIntegerTerm(objInfo);
                    break;
                case ("System.DateTime"):
                    MakeDateTimeTerm(objInfo);
                    break;
                case ("System.Double"):
                    MakeDoubleTerm(objInfo);
                    break;
                case ("System.Boolean"):
                    MakeBooleanTerm(objInfo);
                    break;
                case ("System.Object"):
                    MakeObjectTerm(objInfo);
                    break;
                case ("System.Type"):
                    //do nothing with types...
                    break;
                default:
                    //messagetext.Append("Unknown System Type: " + objInfo.Type.FullName + " at " + objInfo.Path);
                    break;
            }
        }

        private void MakeListTerm(InternalRuleObjectInfo objInfo)
        {
            WME termList = new WME();
            termList.Value = new ListTerm((Array) objInfo.Value);
            // build the Term
            BuildTerm(objInfo, termList);

            //if (_PromiscuousMode)
            //{
            //    objInfo.Arity = 2;
            //    objInfo.CLPType = typeof (FuncTerm).FullName;
            //    _RuleObjectInfoList.Add(objInfo);
            //}
        }

        private void MakeStringTerm(InternalRuleObjectInfo objInfo)
        {
            objInfo.Arity = 2;
            string tmpValue = objInfo.Value.ToString(); // as string;
            if ((tmpValue.IndexOf(' ') != - 1) || (tmpValue.IndexOf('-') != - 1))
            {
                tmpValue = "\"" + tmpValue + "\"";
            }
            WME termList = new WME();
            termList.Value = tmpValue;
            //if (objInfo.Ordinal > 0)
            //{
            //    objInfo.Arity = 3;
            //    termList.Add(new IntegerTerm(objInfo.Ordinal));
            //}
            // build the Term
            BuildTerm(objInfo, termList);
            //if (_PromiscuousMode)
            //{
            //    objInfo.CLPType = typeof (FuncTerm).FullName;
            //    _RuleObjectInfoList.Add(objInfo);
            //}
        }

        private void MakeBooleanTerm(InternalRuleObjectInfo objInfo)
        {
            objInfo.Arity = 2;
            WME termList = new WME();
            termList.Value = new BooleanTerm(Convert.ToBoolean(objInfo.Value));
            //if (objInfo.Ordinal > 0)
            //{
            //    objInfo.Arity = 3;
            //    termList.Add(new IntegerTerm(objInfo.Ordinal));
            //}
            // build the Term
            BuildTerm(objInfo, termList);
            //if (_PromiscuousMode)
            //{
            //    objInfo.CLPType = typeof (BooleanTerm).FullName;
            //    _RuleObjectInfoList.Add(objInfo);
            //}
        }

        private void MakeDoubleTerm(InternalRuleObjectInfo objInfo)
        {
            objInfo.Arity = 2;
            WME termList = new WME();
            termList.Value = new DoubleTerm(((Double) objInfo.Value));
            //if (objInfo.Ordinal > 0)
            //{
            //    objInfo.Arity = 3;
            //    termList.Add(new IntegerTerm(objInfo.Ordinal));
            //}
            // build the Term
            BuildTerm(objInfo, termList);
            //if (_PromiscuousMode)
            //{
            //    objInfo.CLPType = typeof (DoubleTerm).FullName;
            //    _RuleObjectInfoList.Add(objInfo);
            //}
        }

        private void MakeIntegerTerm(InternalRuleObjectInfo objInfo)
        {
            objInfo.Arity = 2;
            WME termList = new WME();
            termList.Value = new IntegerTerm(((Int32) objInfo.Value));
            //if (objInfo.Ordinal > 0)
            //{
            //    objInfo.Arity = 3;
            //    termList.Add(new IntegerTerm(objInfo.Ordinal));
            //}
            // build the Term
            BuildTerm(objInfo, termList);

            //if (_PromiscuousMode)
            //{
            //    objInfo.CLPType = typeof (IntegerTerm).FullName;
            //    _RuleObjectInfoList.Add(objInfo);
            //}
        }

        private void MakeDateTimeTerm(InternalRuleObjectInfo objInfo)
        {
            objInfo.Arity = 2;
            WME termList = new WME();
            termList.Value = new DateTimeTerm(((DateTime) objInfo.Value));
            //if (objInfo.Ordinal > 0)
            //{
            //    objInfo.Arity = 3;
            //    termList.Add(new IntegerTerm(objInfo.Ordinal));
            //}
            // build the Term
            BuildTerm(objInfo, termList);
            //if (_PromiscuousMode)
            //{
            //    objInfo.CLPType = typeof (DateTimeTerm).FullName;
            //    _RuleObjectInfoList.Add(objInfo);
            //}
        }

        private void MakeObjectTerm(InternalRuleObjectInfo objInfo)
        {
            WME termList = new WME();
            termList.Value = new StringTerm(objInfo.Path);
            //if (objInfo.Ordinal > 0)
            //{
            //    termList.Add(new IntegerTerm(objInfo.Ordinal));
            //}

            // build the Term
            BuildTerm(objInfo, termList);

            //if (_PromiscuousMode)
            //{
            //    objInfo.Arity = 1;
            //    objInfo.CLPType = typeof (FuncTerm).FullName;
            //    _RuleObjectInfoList.Add(objInfo);
            //}
        }

        private void BuildTerm(InternalRuleObjectInfo objInfo, WME theList)
        {
            theList.Fields[0] = new ObjectTerm(objInfo.Obj);
            theList.Fields[1] = new StringTerm(objInfo.Path);
            CLiteralList.Add(theList);
        }

        #endregion

        #endregion
    }
}