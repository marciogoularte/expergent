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
using Expergent.Terms;

namespace Expergent.MutexEvaluators
{
    public abstract class AbstractBaseMutexEvaluator : IMutexEvaluator
    {
        #region Fields

        protected int _conditional;
        protected Term _predicate;
        protected int _subject;
        protected Dictionary<object, List<WME>> _sorter;

        #endregion

        #region Constructors

        public AbstractBaseMutexEvaluator(int conditional, Term predicate, int subject) : this()
        {
            _conditional = conditional;
            _predicate = predicate;
            _subject = subject;
        }

        public AbstractBaseMutexEvaluator()
        {
            _sorter = new Dictionary<object, List<WME>>();
        }

        #endregion

        #region IMutexEvaluator Members

        #region Abstract Members

        public abstract bool PerformEvaluation(Term o, Term o1);

        #endregion

        #region Public Properties

        public int Conditional
        {
            get { return _conditional; }
            set { _conditional = value; }
        }

        public Term Predicate
        {
            get { return _predicate; }
            set { _predicate = value; }
        }

        public int Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        #endregion

        #region Public Methods

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