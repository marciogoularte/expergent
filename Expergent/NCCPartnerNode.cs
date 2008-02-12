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

using Expergent.Interfaces;
using Wintellect.PowerCollections;

namespace Expergent
{
    /// <summary>
    /// An NCC partner node
    /// </summary>
    /// <remarks>
    /// An NCC partner node stores a pointer to the corresponding NCC node, plus a count of the
    /// number of conjuncts in the NCC (this is used for stripping of the last several WMEs from each
    /// subnetwork match, as discussed above). It also contains a new-result-buffer, which is used as
    /// a temporary buffer in between the time the subnetwork is activated with a new match for the
    /// preceding conditions and the time the NCC node is activated with that match. It stores the
    /// results (if there are any) from the subnetwork for that match.
    /// </remarks>
    public class NCCPartnerNode : ReteNode
    {
        private NCCNode _ncc_node;
        private int _number_of_conjuncts;
        private readonly BigList<Token> _new_result_buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCCPartnerNode"/> class.
        /// </summary>
        public NCCPartnerNode()
        {
            _type = ReteNodeType.NCCPartner;
            _new_result_buffer = new BigList<Token>();
        }

        /// <summary>
        /// points to the corresponding NCC node
        /// </summary>
        /// <value>The NCC node.</value>
        public NCCNode NCCNode
        {
            get { return _ncc_node; }
            set { _ncc_node = value; }
        }

        /// <summary>
        /// number of conjuncts in the NCC
        /// </summary>
        /// <value>The number of conjuncts.</value>
        public int NumberOfConjuncts
        {
            get { return _number_of_conjuncts; }
            set { _number_of_conjuncts = value; }
        }

        /// <summary>
        /// results for the match the NCC node hasn't heard about
        /// </summary>
        /// <value>The new result buffer.</value>
        public BigList<Token> NewResultBuffer
        {
            get { return _new_result_buffer; }
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnNCCPartnerNode(this);
        }
    }
}