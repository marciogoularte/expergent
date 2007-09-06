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
using Expergent.Conditions;
using Expergent.Interfaces;

namespace Expergent.MutexEvaluators
{
    ///<summary>An Abstract Base Mutex Evaluator
    ///</summary>
    public abstract class AbstractBaseMutexEvaluator : IMutexEvaluator
    {
        #region Fields

        /// <summary>
        /// The conditional's position
        /// </summary>
        protected int _conditional;

        /// <summary>
        /// The predicate term
        /// </summary>
        protected Term _predicate;

        /// <summary>
        /// The subject's position
        /// </summary>
        protected int _subject;

        /// <summary>
        /// Sorts terms into pairs
        /// </summary>
        protected Dictionary<object, List<WME>> _sorter;

        /// <summary>
        /// The conditional term
        /// </summary>
        protected Term _conditionalTerm;

        /// <summary>
        /// The subject term
        /// </summary>
        protected Term _subjectTerm;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractBaseMutexEvaluator"/> class.
        /// </summary>
        /// <param name="conditional">The conditional.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="subject">The subject.</param>
        public AbstractBaseMutexEvaluator(Term conditional, Term predicate, Term subject)
        {
            _conditionalTerm = conditional;
            _predicate = predicate;
            _subjectTerm = subject;
            _sorter = new Dictionary<object, List<WME>>();
        }

        #endregion

        #region IMutexEvaluator Members

        #region Abstract Members

        /// <summary>
        /// Performs the evaluation.
        /// </summary>
        /// <param name="lsTerm">The leftside term.</param>
        /// <param name="rsTerm">The rightside term.</param>
        /// <returns>The result</returns>
        public abstract bool PerformEvaluation(Term lsTerm, Term rsTerm);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the conditional position.
        /// </summary>
        /// <value>The conditional.</value>
        public int ConditionalPosition
        {
            get { return _conditional; }
            set { _conditional = value; }
        }

        /// <summary>
        /// Gets or sets the subject position.
        /// </summary>
        /// <value>The subject.</value>
        public int SubjectPosition
        {
            get { return _subject; }
            set { _subject = value; }
        }

        /// <summary>
        /// Gets or sets the conditional term.
        /// </summary>
        /// <value>The conditional term.</value>
        public Term ConditionalTerm
        {
            get { return _conditionalTerm; }
            set { _conditionalTerm = value; }
        }

        /// <summary>
        /// Gets or sets the predicate term.
        /// </summary>
        /// <value>The predicate.</value>
        public Term PredicateTerm
        {
            get { return _predicate; }
            set { _predicate = value; }
        }

        /// <summary>
        /// Gets or sets the subject term.
        /// </summary>
        /// <value>The subject term.</value>
        public Term SubjectTerm
        {
            get { return _subjectTerm; }
            set { _subjectTerm = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Evaluates the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public List<Activation> Evaluate(IEnumerable<Token> items)
        {
            List<Activation> outlist = new List<Activation>();

            foreach (Token item in items)
            {
                object key = item.WME.Fields[_conditional].Value;

                if (_sorter.ContainsKey(key))
                {
                    List<WME> list = _sorter[key];
                    if (list.Contains(item.WME) == false)
                        list.Add(item.WME);
                }
                else
                {
                    List<WME> list = new List<WME>();
                    list.Add(item.WME);
                    _sorter.Add(key, list);
                }
            }

            foreach (KeyValuePair<object, List<WME>> pair in _sorter)
            {
                while (pair.Value.Count > 1)
                {
                    WME lh = pair.Value[0];
                    WME rh = pair.Value[1];
                    if (PerformEvaluation(lh.Fields[_subject], rh.Fields[_subject]))
                    {
                        pair.Value.Remove(rh);
                    }
                    else
                    {
                        pair.Value.Remove(lh);
                    }
                }
                outlist.Add(new Activation(pair.Value[0], ConditionType.Assert));
            }

            return outlist;
        }

        #endregion

        #endregion
    }
}