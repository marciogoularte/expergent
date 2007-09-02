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
    ///<summary>a sequence of WMEs
    ///</summary>
    public class Token : IVisitable
    {
        #region Fields

        /// <summary>
        /// points to the higher token, for items 1...i-1
        /// </summary>
        /// <value>The parent.</value>
        protected Token _parent;

        /// <summary>
        /// The wme associated with this token
        /// </summary>
        protected WME _wme;

        /// <summary>
        /// points to the memory this token is in
        /// </summary>
        protected ReteNode _node;

        /// <summary>
        /// the ones with parent=this token
        /// </summary>
        protected LinkedList<Token> _children;

        /// <summary>
        /// used only on tokens in negative nodes
        /// </summary>
        protected LinkedList<NegativeJoinResult> _join_results;

        /// <summary>
        /// similar to join-results but for NCC nodes
        /// </summary>
        protected LinkedList<Token> _ncc_results;

        /// <summary>
        /// The owner of this token
        /// </summary>
        protected Token _owner;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        public Token()
        {
            _children = new LinkedList<Token>();
            _join_results = new LinkedList<NegativeJoinResult>();
            _ncc_results = new LinkedList<Token>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// points to the higher token, for items 1...i-1
        /// </summary>
        /// <value>The parent.</value>
        public virtual Token Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the WME.
        /// </summary>
        /// <value>The WME.</value>
        public virtual WME WME
        {
            get { return _wme; }
            set { _wme = value; }
        }

        /// <summary>
        /// points to the memory this token is in
        /// </summary>
        /// <value>The node.</value>
        public virtual ReteNode Node
        {
            get { return _node; }
            set { _node = value; }
        }

        /// <summary>
        /// the ones with parent=this token
        /// </summary>
        /// <value>The children.</value>
        public virtual LinkedList<Token> Children
        {
            get { return _children; }
        }

        /// <summary>
        /// used only on tokens in negative nodes
        /// </summary>
        /// <value>The join results.</value>
        public virtual LinkedList<NegativeJoinResult> JoinResults
        {
            get { return _join_results; }
        }

        /// <summary>
        /// similar to join-results but for NCC nodes
        /// </summary>
        /// <value>The NCC results.</value>
        public virtual LinkedList<Token> NCCResults
        {
            get { return _ncc_results; }
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public virtual Token Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="Expergent.WME"/> with the specified item.
        /// </summary>
        /// <value></value>
        public virtual WME this[int item]
        {
            get
            {
                if (item == 0)
                    return _wme;

                Token next = this;
                for (int i = 0; i < item; i++)
                {
                    next = next.Parent;
                }
                return next.WME;
            }
        }

        #region Methods

        /// <summary>
        /// Gets the token up some # of steps.
        /// </summary>
        /// <param name="steps">The steps.</param>
        /// <returns></returns>
        public virtual Token GetTokenUp(int steps)
        {
            if (steps == 0)
                return this;

            Token next = this;
            for (int i = 0; i < steps; i++)
            {
                next = next.Parent;
            }
            return next;
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void Accept(IVisitor visitor)
        {
            visitor.OnToken(this);
        }

        #endregion
    }
}