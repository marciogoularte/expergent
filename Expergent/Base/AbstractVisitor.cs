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

using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent.Base
{
    ///<summary>
    /// A base visitor implimentation
    ///</summary>
    public abstract class AbstractVisitor : IVisitor
    {
        #region IVisitor Members

        /// <summary>
        /// Called when [alpha memory] is visited.
        /// </summary>
        /// <param name="am">The am.</param>
        public virtual void OnAlphaMemory(AlphaMemory am)
        {
        }

        /// <summary>
        /// Called when [beta memory] is visited.
        /// </summary>
        /// <param name="beta">The beta.</param>
        public virtual void OnBetaMemory(BetaMemory beta)
        {
        }

        /// <summary>
        /// Called when [condition] is visited.
        /// </summary>
        /// <param name="c">The c.</param>
        public virtual void OnCondition(Condition c)
        {
        }

        /// <summary>
        /// Called when [item in alpha memory] is visited.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void OnItemInAlphaMemory(ItemInAlphaMemory item)
        {
        }

        /// <summary>
        /// Called when [join node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnJoinNode(JoinNode node)
        {
        }

        /// <summary>
        /// Called when [NCC node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnNCCNode(NCCNode node)
        {
        }

        /// <summary>
        /// Called when [NCC partner node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnNCCPartnerNode(NCCPartnerNode node)
        {
        }

        /// <summary>
        /// Called when [negative join result] is visited.
        /// </summary>
        /// <param name="result">The result.</param>
        public virtual void OnNegativeJoinResult(NegativeJoinResult result)
        {
        }

        /// <summary>
        /// Called when [negative node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnNegativeNode(NegativeNode node)
        {
        }

        /// <summary>
        /// Called when [P node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnPNode(ProductionNode node)
        {
        }

        /// <summary>
        /// Called when [production] is visited.
        /// </summary>
        /// <param name="prod">The prod.</param>
        public virtual void OnProduction(Production prod)
        {
        }

        /// <summary>
        /// Called when [at join node] is visited.
        /// </summary>
        /// <param name="test">The test.</param>
        public virtual void OntestAtJoinNode(TestAtJoinNode test)
        {
        }

        /// <summary>
        /// Called when [token] is visited.
        /// </summary>
        /// <param name="t">The t.</param>
        public virtual void OnToken(Token t)
        {
        }

        /// <summary>
        /// Called when [variable] is visited.
        /// </summary>
        /// <param name="v">The v.</param>
        public virtual void OnVariable(Variable v)
        {
        }

        /// <summary>
        /// Called when [WME] is visited.
        /// </summary>
        /// <param name="wme">The wme.</param>
        public virtual void OnWME(WME wme)
        {
        }

        /// <summary>
        /// Called when [top node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnTopNode(DummyTopNode node)
        {
        }

        /// <summary>
        /// Called when [top token] is visited.
        /// </summary>
        /// <param name="t">The t.</param>
        public virtual void OnTopToken(DummyTopToken t)
        {
        }

        /// <summary>
        /// Called when [rete] is visited.
        /// </summary>
        /// <param name="rete">The rete.</param>
        public virtual void OnRete(Rete rete)
        {
        }

        /// <summary>
        /// Called when [rete node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnReteNode(ReteNode node)
        {
        }

        /// <summary>
        /// Called when [builtin memory] is visited.
        /// </summary>
        /// <param name="memory">The memory.</param>
        public virtual void OnBuiltinMemory(BuiltinMemory memory)
        {
        }

        /// <summary>
        /// Called when [term] is visited.
        /// </summary>
        /// <param name="term">The term.</param>
        public virtual void OnTerm(Term term)
        {
        }

        /// <summary>
        /// Called when [binding pair] is visited.
        /// </summary>
        /// <param name="bindingPair">The binding pair.</param>
        public virtual void OnBindingPair(BindingPair bindingPair)
        {
        }

        /// <summary>
        /// Called when [variable substituter] is visited.
        /// </summary>
        /// <param name="substituter">The substituter.</param>
        public virtual void OnVariableSubstituter(VariableSubstituter substituter)
        {
        }

        /// <summary>
        /// Called when [constant substitutor] is visited.
        /// </summary>
        /// <param name="substitutor">The substitutor.</param>
        public virtual void OnConstantSubstitutor(ConstantSubstitutor substitutor)
        {
        }

        /// <summary>
        /// Called when [mutex] is visited.
        /// </summary>
        /// <param name="mutex">The mutex.</param>
        public virtual void OnMutex(Mutex mutex)
        {
        }

        /// <summary>
        /// Called when [mutex node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void OnMutexNode(MutexNode node)
        {
        }

        #endregion
    }
}