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
using Expergent.Conditions;
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent
{
    ///<summary>A rule.
    ///</summary>
    public class Production : IVisitable
    {
        #region Fields

        private ProductionNode _p_node;
        private List<LeftHandSideCondition> _lhs;
        private List<RightHandSideCondition> _rhs;
        private string _label;
        private Dictionary<string, int> _variableList;
        private List<Activation> _inferredFacts;
        private int _salience;
        private ProductionStatus _status;
        private DateTime _timeStamp;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Production"/> class.
        /// </summary>
        public Production()
        {
            _lhs = new List<LeftHandSideCondition>();
            _rhs = new List<RightHandSideCondition>();
            _variableList = new Dictionary<string, int>();
            _inferredFacts = new List<Activation>();
            _status = ProductionStatus.Ready;
            _timeStamp = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Production"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Production(string label) : this()
        {
            _salience = 1;
            _label = label;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the production node.
        /// </summary>
        /// <value>The production node.</value>
        public ProductionNode ProductionNode
        {
            get { return _p_node; }
            set { _p_node = value; }
        }

        /// <summary>
        /// Gets or sets the variable list.
        /// </summary>
        /// <value>The variable list.</value>
        public Dictionary<string, int> VariableList
        {
            get { return _variableList; }
            set { _variableList = value; }
        }

        /// <summary>
        /// Gets or sets the salience.
        /// </summary>
        /// <value>The salience.</value>
        public int Salience
        {
            get { return _salience; }
            set { _salience = value; }
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
        /// Gets the inferred facts.
        /// </summary>
        /// <value>The inferred facts.</value>
        public List<Activation> InferredFacts
        {
            get { return _inferredFacts; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Production"/> is activated.
        /// </summary>
        /// <value><c>true</c> if activated; otherwise, <c>false</c>.</value>
        public bool Activated
        {
            get { return ProductionNode.Items.Count > 0; }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ProductionStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _label;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the condition to LHS.
        /// </summary>
        /// <param name="cond">The cond.</param>
        public void AddConditionToLHS(LeftHandSideCondition cond)
        {
            _lhs.Add(cond);
            addVariablesToBindingList(cond);
        }


        /// <summary>
        /// Adds the condition to RHS.
        /// </summary>
        /// <param name="cond">The cond.</param>
        public void AddConditionToRHS(RightHandSideCondition cond)
        {
            _rhs.Add(cond);
        }

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnProduction(this);
        }

        #endregion

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the LHS.
        /// </summary>
        /// <value>The LHS.</value>
        internal List<LeftHandSideCondition> Lhs
        {
            get { return _lhs; }
        }

        /// <summary>
        /// Gets the RHS.
        /// </summary>
        /// <value>The RHS.</value>
        internal List<RightHandSideCondition> Rhs
        {
            get { return _rhs; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds the variables to binding list.
        /// </summary>
        /// <param name="cond">The cond.</param>
        private void addVariablesToBindingList(Condition cond)
        {
            foreach (Term o in cond.Fields)
            {
                if (o.TermType == TermType.Variable)
                {
                    _variableList[o.ToString()] = _lhs.Count;
                }
            }
        }

        #endregion
    }
}