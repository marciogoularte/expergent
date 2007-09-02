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
    /// Each node in the beta part of the net will be represented by a rete-node structure.
    /// </summary>
    /// <remarks>
    /// A beta memory node stores a list of the tokens it contains, plus a list of its children (other
    /// nodes in the beta part of the network). Before we give its data structure, though, recall that
    /// we were going to do our procedure calls for left and right activations through a switch or case
    /// statement or a jumptable indexed according to the type of node being activated. Thus, given
    /// a (pointer to a) node, we need to be able to determine its type. This is straightforward if we
    /// use variant records to represent nodes. (A variant record is a record that can contain any one
    /// of several different sets of fields.)
    /// </remarks>
    public abstract class ReteNode : IVisitable
    {
        #region Fields
        /// <summary>
        /// The parent node
        /// </summary>
        protected ReteNode _parent;
        /// <summary>
        /// The Node Type
        /// </summary>
        protected ReteNodeType _type;
        /// <summary>
        /// List of child nodes
        /// </summary>
        protected LinkedList<ReteNode> _children;
        /// <summary>
        /// The node label
        /// </summary>
        protected string _label;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReteNode"/> class.
        /// </summary>
        public ReteNode()
        {
            _children = new LinkedList<ReteNode>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReteNode"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public ReteNode(string label) : this()
        {
            _label = label;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ReteNodeType.
        /// </summary>
        /// <value>The type.</value>
        public virtual ReteNodeType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets the children nodes.
        /// </summary>
        /// <value>The children.</value>
        public virtual LinkedList<ReteNode> Children
        {
            get { return _children; }
        }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>The parent.</value>
        public virtual ReteNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public virtual string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void Accept(IVisitor visitor)
        {
            visitor.OnReteNode(this);
        }

        #endregion
    }
}