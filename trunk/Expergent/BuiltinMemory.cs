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
using Expergent.Interfaces;

namespace Expergent
{
    /// <summary>
    /// Used to collect wme's and run a builtin evaluation upon them.
    /// </summary>
    public class BuiltinMemory : ReteNode
    {
        #region Private Fields

        private LinkedList<Token> _items;
        private IBuiltIn _builtin;
        private ISubstitutor _rightArgument;
        private ISubstitutor _leftArgument;
        private List<string> _results;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BuiltinMemory"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public BuiltinMemory(string label)
        {
            _label = label;
            _type = ReteNodeType.Builtin;
            _results = new List<string>();
            _items = new LinkedList<Token>();
        }

        /// <summary>
        /// Performs the evaluation.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public bool PerformEvaluation(Token t)
        {
            if (_leftArgument == null || _rightArgument == null)
            {
                _results.Add(string.Format("{0} {1} {2} FAILED", _leftArgument, _builtin, _rightArgument));
                return false;
            }
            bool result = _builtin.Evaluate(_leftArgument.GetValue(t), _rightArgument.GetValue(t));
            if (result)
            {
                _results.Add(string.Format("{0} {1} {2} SUCCESS", _leftArgument, _builtin, _rightArgument));
            }
            else
            {
                _results.Add(string.Format("{0} {1} {2} FAILED", _leftArgument, _builtin, _rightArgument));
            }
            return result;
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

        #region Public Properties

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public virtual LinkedList<Token> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets or sets the left argument.
        /// </summary>
        /// <value>The left argument.</value>
        public ISubstitutor LeftArgument
        {
            get { return _leftArgument; }
            set { _leftArgument = value; }
        }

        /// <summary>
        /// Gets or sets the right argument.
        /// </summary>
        /// <value>The right argument.</value>
        public ISubstitutor RightArgument
        {
            get { return _rightArgument; }
            set { _rightArgument = value; }
        }

        /// <summary>
        /// Gets or sets the builtin.
        /// </summary>
        /// <value>The builtin.</value>
        public IBuiltIn Builtin
        {
            get { return _builtin; }
            set { _builtin = value; }
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>The results.</value>
        public List<string> Results
        {
            get { return _results; }
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnBuiltinMemory(this);
        }

        #endregion
    }
}