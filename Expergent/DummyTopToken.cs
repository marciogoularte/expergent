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
    /// We note a few things about the above procedures. First, in order to be able to use these
    /// procedures for the uppermost join nodes in the network | the ones that are children of the
    /// dummy top node, as in Figure 2.2 on page 10 | we need to have the dummy top node act as a
    /// beta memory for these join nodes. We always keep a single dummy top token in the dummy top
    /// node, just so there will be one thing to iterate over in the join-node-right-activation procedure.
    /// </summary>
    public class DummyTopToken : Token
    {
        public DummyTopToken()
        {
            _wme = new WME("null");
        }

        public override Token Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public override WME WME
        {
            get { return _wme; }
            set { _wme = value; }
        }

        public override ReteNode Node
        {
            get { return _node; }
            set { _node = value; }
        }

        public override LinkedList<Token> Children
        {
            get { return _children; }
        }

        public override LinkedList<NegativeJoinResult> JoinResults
        {
            get { return _join_results; }
        }

        public override LinkedList<Token> NCCResults
        {
            get { return _ncc_results; }
        }

        public override Token Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public override WME this[int item]
        {
            get { return WME; }
        }

        #region IVisitable Members

        public override void Accept(IVisitor visitor)
        {
            visitor.OnTopToken(this);
        }

        #endregion
    }
}