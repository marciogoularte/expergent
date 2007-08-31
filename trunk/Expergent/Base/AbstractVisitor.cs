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
    public abstract class AbstractVisitor : IVisitor
    {
        #region IVisitor Members

        public virtual void OnAlphaMemory(AlphaMemory am)
        {
        }

        public virtual void OnBetaMemory(BetaMemory beta)
        {
        }

        public virtual void OnCondition(Condition c)
        {
        }

        public virtual void OnItemInAlphaMemory(ItemInAlphaMemory item)
        {
        }

        public virtual void OnJoinNode(JoinNode node)
        {
        }

        public virtual void OnNCCNode(NCCNode node)
        {
        }

        public virtual void OnNCCPartnerNode(NCCPartnerNode node)
        {
        }

        public virtual void OnNegativeJoinResult(NegativeJoinResult result)
        {
        }

        public virtual void OnNegativeNode(NegativeNode node)
        {
        }

        public virtual void OnPNode(ProductionNode node)
        {
        }

        public virtual void OnProduction(Production prod)
        {
        }

        public virtual void OntestAtJoinNode(TestAtJoinNode test)
        {
        }

        public virtual void OnToken(Token t)
        {
        }

        public virtual void OnVariable(Variable v)
        {
        }

        public virtual void OnWME(WME wme)
        {
        }

        public virtual void OnTopNode(DummyTopNode node)
        {
        }

        public virtual void OnTopToken(DummyTopToken t)
        {
        }

        public virtual void OnRete(Rete rete)
        {
        }

        public virtual void OnReteNode(ReteNode node)
        {
        }

        public virtual void OnBuiltinMemory(BuiltinMemory memory)
        {
        }

        public virtual void OnTerm(Term term)
        {
        }

        public virtual void OnBindingPair(BindingPair bindingPair)
        {
        }

        public virtual void OnVariableSubstituter(VariableSubstituter substituter)
        {
        }

        public virtual void OnConstantSubstitutor(ConstantSubstitutor substitutor)
        {
        }

        public virtual void OnMutex(Mutex mutex)
        {
        }

        public virtual void OnMutexNode(MutexNode node)
        {
        }

        #endregion
    }
}