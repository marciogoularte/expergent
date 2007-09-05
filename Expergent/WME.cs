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
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent
{
    /// <summary>
    /// Working Memory Entity
    /// </summary>
    public class WME : IVisitable
    {
        #region Fields

        private string _label;
        private Term[] _fields;
        private LinkedList<ItemInAlphaMemory> _alpha_mem_items;
        private LinkedList<Token> _tokens;
        private LinkedList<NegativeJoinResult> _negative_join_results;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WME"/> class.
        /// </summary>
        public WME() : this("WME")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WME"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public WME(string label)
        {
            _label = label;
            _fields = new Term[3];
            _alpha_mem_items = new LinkedList<ItemInAlphaMemory>();
            _tokens = new LinkedList<Token>();
            _negative_join_results = new LinkedList<NegativeJoinResult>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WME"/> class.
        /// </summary>
        /// <param name="v1">The Identifier.</param>
        /// <param name="v2">The Attribute.</param>
        /// <param name="v3">The Value.</param>
        public WME(Term v1, Term v2, Term v3) : this("WME", v1, v2, v3)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WME"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="v1">The Identifier.</param>
        /// <param name="v2">The Attribute.</param>
        /// <param name="v3">The Value.</param>
        public WME(string label, Term v1, Term v2, Term v3) : this(label)
        {
            Identifier = v1;
            Attribute = v2;
            Value = v3;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>The fields.</value>
        public Term[] Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Term Identifier
        {
            get { return _fields[0]; }
            set
            {
                value.Ordinal = 0;
                _fields[0] = value;
            }
        }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public Term Attribute
        {
            get { return _fields[1]; }
            set
            {
                value.Ordinal = 1;
                _fields[1] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public Term Value
        {
            get { return _fields[2]; }
            set
            {
                value.Ordinal = 2;
                _fields[2] = value;
            }
        }

        /// <summary>
        /// the ones containing this WME
        /// </summary>
        /// <value>The alpha memory items.</value>
        public LinkedList<ItemInAlphaMemory> AlphaMemoryItems
        {
            get { return _alpha_mem_items; }
        }

        /// <summary>
        /// the ones with wme=this WME
        /// </summary>
        /// <value>The tokens.</value>
        public LinkedList<Token> Tokens
        {
            get { return _tokens; }
        }

        /// <summary>
        /// We also add a field negative-join-results to the data structure for each WME, to store a list of
        /// all the negative-join-result structures involving the WME. This will be used for handling WME
        /// removals.
        /// </summary>
        /// <value>The negative join results.</value>
        public LinkedList<NegativeJoinResult> NegativeJoinResults
        {
            get { return _negative_join_results; }
        }

        /// <summary>
        /// Determines whether [contains] [the specified obj].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified obj]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(object obj)
        {
            if (obj == null) 
                return false;

            foreach (Term o in _fields)
            {
                if (obj.Equals(o))
                    return true;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="Expergent.Terms.Term"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Term this[int index]
        {
            get { return Fields[index]; }
        }

        #region overrides

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            WME otherWME = obj as WME;
            if (otherWME == null)
                return base.Equals(obj);
            return _fields[0].Equals(otherWME.Fields[0]) && _fields[1].Equals(otherWME.Fields[1]) && _fields[2].Equals(otherWME.Fields[2]);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return _fields[0].GetHashCode() + _fields[1].GetHashCode() + _fields[2].GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}: [<{1}>, <{2}>, <{3}>]", _label, Fields[0], Fields[1], Fields[2]);
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnWME(this);
        }

        #endregion
    }
}