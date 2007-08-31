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
    /// 
    /// </summary>
    public class MutexNode : ReteNode
    {
        #region Private Fields

        private List<Token> _items;
        private List<WME> _inferredFacts;
        private Mutex _mutex;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MutexNode"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public MutexNode(string label)
        {
            _label = label;
            _type = ReteNodeType.Mutex;
            _items = new List<Token>();
            _inferredFacts = new List<WME>();
        }

        #endregion

        #region Public Methods

        #region Overrides

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

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public virtual List<Token> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the inferred facts.
        /// </summary>
        /// <value>The inferred facts.</value>
        public List<WME> InferredFacts
        {
            get { return _inferredFacts; }
        }

        /// <summary>
        /// Gets or sets the mutex.
        /// </summary>
        /// <value>The mutex.</value>
        public Mutex Mutex
        {
            get { return _mutex; }
            set { _mutex = value; }
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnMutexNode(this);
        }

        #endregion
    }
}