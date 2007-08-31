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
    /// <remarks><para>
    /// As mentioned in the overview, a join node can incur a right activation when a WME is added
    /// to its alpha memory, or a left activation when a token is added to its beta memory. In either
    /// case, the node's other memory is searched for items having variable bindings consistent with the
    /// new item; if any are found, they are passed on to the join node's children.
    /// </para>
    /// <para>
    /// The data structure for a join node, therefore, must contain pointers to its two memory
    /// nodes (so they can be searched), a specification of any variable binding consistency tests to be
    /// performed, and a list of the node's children. From the data common to all nodes (the rete-node
    /// structure on page 22), we already have the children; also, the parent field automatically gives us
    /// a pointer to the join node's beta memory (the beta memory is always its parent). We need two
    /// extra fields for a join node.
    /// </para>
    /// </remarks>
    public class JoinNode : ReteNode
    {
        #region fields

        private AlphaMemory _amem;
        private LinkedList<TestAtJoinNode> _tests;
        private ReteNode _nearest_ancestor_with_same_amem;
        private bool _isRightUnlinked;
        private bool _isLeftUnlinked;
        private bool isHeadOfSubNetwork = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinNode"/> class.
        /// </summary>
        public JoinNode()
        {
            _type = ReteNodeType.Join;
            _tests = new LinkedList<TestAtJoinNode>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// points to the alpha memory this node is attached to
        /// </summary>
        /// <value>The alpha memory.</value>
        public AlphaMemory AlphaMemory
        {
            get { return _amem; }
            set { _amem = value; }
        }

        /// <summary>
        /// Gets the tests to perform
        /// </summary>
        /// <value>The tests.</value>
        public LinkedList<TestAtJoinNode> Tests
        {
            get { return _tests; }
        }

        /// <summary>
        /// Gets or sets the nearest ancestor with same amem.
        /// </summary>
        /// <value>The nearest ancestor with same amem.</value>
        public ReteNode NearestAncestorWithSameAmem
        {
            get { return _nearest_ancestor_with_same_amem; }
            set { _nearest_ancestor_with_same_amem = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is right unlinked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is right unlinked; otherwise, <c>false</c>.
        /// </value>
        public bool IsRightUnlinked
        {
            get { return _isRightUnlinked; }
            set { _isRightUnlinked = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is left unlinked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is left unlinked; otherwise, <c>false</c>.
        /// </value>
        public bool IsLeftUnlinked
        {
            get { return _isLeftUnlinked; }
            set { _isLeftUnlinked = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is head of sub network.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is head of sub network; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeadOfSubNetwork
        {
            get { return isHeadOfSubNetwork; }
            set { isHeadOfSubNetwork = value; }
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnJoinNode(this);
        }

        #endregion
    }
}