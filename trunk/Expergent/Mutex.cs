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
    /// <summary>
    /// A class for the resolution of Mutual Exclusions
    /// </summary>
    public class Mutex : IVisitable
    {
        #region Fields

        private List<LeftHandSideCondition> _lhs;
        private List<RightHandSideCondition> _rhs;
        private string _label;
        private List<Activation> _inferredFacts;
        private ProductionStatus _status;
        private MutexNode _mutexNode;
        private IMutexEvaluator _evaluator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Mutex"/> class.
        /// </summary>
        public Mutex()
        {
            _lhs = new List<LeftHandSideCondition>();
            _rhs = new List<RightHandSideCondition>();
            _inferredFacts = new List<Activation>();
            _status = ProductionStatus.Ready;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mutex"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Mutex(string label) : this()
        {
            _label = label;
        }

        #endregion

        #region Properties

        #region Public

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
        /// Gets a value indicating whether this <see cref="Mutex"/> is activated.
        /// </summary>
        /// <value><c>true</c> if activated; otherwise, <c>false</c>.</value>
        public bool Activated
        {
            get { return _mutexNode.Items.Count > 0; }
        }

        /// <summary>
        /// Gets or sets the Production Status.
        /// </summary>
        /// <value>The status.</value>
        public ProductionStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Gets or sets the mutex node.
        /// </summary>
        /// <value>The mutex node.</value>
        public MutexNode MutexNode
        {
            get { return _mutexNode; }
            set { _mutexNode = value; }
        }

        /// <summary>
        /// Gets or sets the mutex evaluator.
        /// </summary>
        /// <value>The mutex evaluator.</value>
        public IMutexEvaluator Evaluator
        {
            get { return _evaluator; }
            set { _evaluator = value; }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Gets the LHS.
        /// </summary>
        /// <value>The LHS.</value>
        internal List<LeftHandSideCondition> lhs
        {
            get { return _lhs; }
        }

        /// <summary>
        /// Gets the RHS.
        /// </summary>
        /// <value>The RHS.</value>
        internal List<RightHandSideCondition> rhs
        {
            get { return _rhs; }
        }

        #endregion

        #endregion

        #region Methods

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnMutex(this);
        }

        #endregion

        #region Public

        /// <summary>
        /// Adds the condition to LHS.
        /// </summary>
        /// <param name="cond">The cond.</param>
        public void AddConditionToLHS(LeftHandSideCondition cond)
        {
            _lhs.Add(cond);
        }

        /// <summary>
        /// Adds the condition to RHS.
        /// </summary>
        /// <param name="cond">The cond.</param>
        public void AddConditionToRHS(RightHandSideCondition cond)
        {
            _rhs.Add(cond);
        }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        public void Evaluate()
        {
            if (_evaluator == null)
            {
                throw new ApplicationException("Mutex evaluator has not been set.");
            }
            _evaluator.ConditionalPosition = GetTermPositionFromConditions(_evaluator.ConditionalTerm);
            _evaluator.SubjectPosition = GetTermPositionFromConditions(_evaluator.SubjectTerm);
            List<Activation> items = _evaluator.Evaluate(_mutexNode.Items);
            BindRHS(items);
        }

        #endregion

        #region Private

        /// <summary>
        /// Binds the RHS.
        /// </summary>
        private void BindRHS(List<Activation> items)
        {
            int lhsCnt = _lhs.Count - 1;
            foreach (Activation act in items)
            {
                foreach (RightHandSideCondition condition in _rhs)
                {
                    WME newfact = new WME();
                    for (int f = 0; f < 3; f++)
                    {
                        Term term = condition.Fields[f];
                        if (term.TermType == TermType.Variable)
                        {
                            for (int i = lhsCnt; i >= 0; i--)
                            {
                                Condition rh = _lhs[i];
                                if (rh.ConditionType == ConditionType.Positive)
                                {
                                    int pos = rh.Contains(term);
                                    if (pos >= 0)
                                    {
                                        newfact.Fields[f] = act.InferredFact[pos];
                                        i = -1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            newfact.Fields[f] = term;
                        }
                    }
                    Activation newact = new Activation(newfact, condition.ConditionType);
                    if (_inferredFacts.Contains(newact) == false)
                    {
                        _inferredFacts.Add(newact);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the term position from conditions.
        /// </summary>
        /// <param name="conditional">The conditional.</param>
        /// <returns></returns>
        private int GetTermPositionFromConditions(Term conditional)
        {
            for (int i = 0; i < 3; i++)
            {
                Term t = _lhs[0].Fields[i];
                if (t.Value.Equals(conditional.Value))
                    return i;
            }
            return -1;
        }

        #endregion

        #endregion
    }
}