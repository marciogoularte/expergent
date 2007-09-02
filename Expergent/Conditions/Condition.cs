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

using System.Collections.Generic;
using Expergent.Evaluators;
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent.Conditions
{
    /// <summary>
    /// Base class for production conditions
    /// </summary>
    public class Condition : IVisitable
    {
        #region Private Fields
        /// <summary>
        /// Subconditions for an NCC node
        /// </summary>
        protected List<LeftHandSideCondition> _subconditions;

        private Term[] _fields;
        private string _label;
        private IEvaluator _evaluator;
        private ConditionType _conditionType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        public Condition() : this("condition")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Condition(string label)
        {
            _label = label;
            _subconditions = new List<LeftHandSideCondition>();
            _conditionType = ConditionType.Positive;
            _fields = new Term[3];
            _evaluator = new Equals();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public Condition(Term id, Term attribute, Term value) : this("condition", id, attribute, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="id">The id.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public Condition(string label, Term id, Term attribute, Term value) : this(label)
        {
            Id = id;
            Attribute = attribute;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="conditionType">Type of the condition.</param>
        /// <param name="id">The id.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public Condition(string label, ConditionType conditionType, Term id, Term attribute, Term value) : this(label, id, attribute, value)
        {
            _conditionType = conditionType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the evaluator.
        /// </summary>
        /// <value>The evaluator.</value>
        public IEvaluator Evaluator
        {
            get { return _evaluator; }
            set { _evaluator = value; }
        }

        /// <summary>
        /// Gets or sets the type of the condition.
        /// </summary>
        /// <value>The type of the condition.</value>
        public ConditionType ConditionType
        {
            get { return _conditionType; }
            set { _conditionType = value; }
        }

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
        /// Gets or sets the fields.
        /// </summary>
        /// <value>The fields.</value>
        public Term[] Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Term Id
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
        /// Gets the sub conditions.
        /// </summary>
        /// <value>The sub conditions.</value>
        public List<LeftHandSideCondition> SubConditions
        {
            get { return _subconditions; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}[{2}, {3}, {4}]", _label, _evaluator, _fields[0], _fields[1], _fields[2]);
        }

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnCondition(this);
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether [contains] [the specified term].
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public int Contains(Term term)
        {
            if (Id == term)
                return 0;
            if (Attribute == term)
                return 1;
            if (Value == term)
                return 2;
            return -1;
        }

        #endregion
    }
}