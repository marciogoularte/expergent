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

using System;
using System.Collections.Generic;
using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.Interfaces;
using Expergent.Neo;
using Expergent.Terms;
using Neo.Framework;

namespace Expergent
{
    /// <summary>
    /// The Network for many-to-many pattern matching
    /// </summary>
    public class Rete : IVisitable
    {
        #region Private Fields

        private DummyTopNode _dummy_top_node;
        private List<WME> _working_memory;
        private int _next_beta_node = 0;
        private int _next_alpha_node = 0;
        private Dictionary<int, Dictionary<int, Dictionary<int, AlphaMemory>>> _alpha_network;

        #endregion

        #region Events

        //public event ActivationDelegate OnActivation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Rete"/> class.
        /// </summary>
        public Rete()
        {
            _working_memory = new List<WME>();
            _dummy_top_node = new DummyTopNode();
            _alpha_network = new Dictionary<int, Dictionary<int, Dictionary<int, AlphaMemory>>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the dummy top node.
        /// </summary>
        /// <value>The dummy top node.</value>
        public BetaMemory DummyTopNode
        {
            get { return _dummy_top_node; }
        }

        /// <summary>
        /// Gets the working memory.
        /// </summary>
        /// <value>The working memory.</value>
        public List<WME> WorkingMemory
        {
            get { return _working_memory; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a production.
        /// </summary>
        /// <remarks>
        /// We are now ready to give the add-production procedure, which takes a production (actually,
        /// just the conditions of the production | the match algorithm doesn't care what the actions are)
        /// and adds it to the network. It follows the basic procedure given at the beginning of this section,
        /// and uses the helper functions we have just defined. The last two lines of the procedure are a
        /// bit vague, because the implementation of production nodes tends to vary from one system to
        /// another. It is important to note that we build the net top-down, and each time we build a
        /// new join node, we insert it at the head of its alpha memory's list of successors; these two facts
        /// guarantee that descendents are on each alpha memory's list before any of their ancestors, just
        /// as we required in Section 2.4.1 in order to avoid duplicate tokens.
        /// </remarks>
        /// <param name="prod">The Production.</param>
        public void AddProduction(Production prod)
        {
            ProductionNode new_production = new ProductionNode();
            new_production.Production = prod;
            AddProduction(new_production, prod.Lhs);
            prod.ProductionNode = new_production;
        }

        /// <summary>
        /// Adds a mutex.
        /// </summary>
        /// <param name="m">The Mutex.</param>
        public void AddMutex(Mutex m)
        {
            MutexNode new_production = new MutexNode(m.label);
            new_production.Mutex = m;
            AddProduction(new_production, m.lhs);
            m.MutexNode = new_production;
        }

        /// <summary>
        /// Removes the production.
        /// </summary>
        /// <param name="prod">The Production.</param>
        public void RemoveProduction(Production prod)
        {
            delete_node_and_any_unused_ancestors(prod.ProductionNode);
        }

        /// <summary>
        /// Adds the WME.
        /// </summary>
        /// <param name="w">The WME.</param>
        public void AddWME(WME w)
        {
            int v1 = w.Fields[0].GetHashCode();
            int v2 = w.Fields[1].GetHashCode();
            int v3 = w.Fields[2].GetHashCode();

            _working_memory.Add(w);

            AlphaMemory alpha_mem;

            alpha_mem = lookup_in_hash_table(0, 0, 0);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
            alpha_mem = lookup_in_hash_table(0, 0, v3);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
            alpha_mem = lookup_in_hash_table(0, v2, 0);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
            alpha_mem = lookup_in_hash_table(0, v2, v3);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
            alpha_mem = lookup_in_hash_table(v1, 0, 0);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
            alpha_mem = lookup_in_hash_table(v1, 0, v3);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
            alpha_mem = lookup_in_hash_table(v1, v2, 0);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
            alpha_mem = lookup_in_hash_table(v1, v2, v3);
            if (alpha_mem != null)
                alpha_memory_activation(alpha_mem, w);
        }

        /// <summary>
        /// Now, to remove a WME, we just remove it from each alpha memory containing it (these
        /// alpha memories are now conveniently on a list) and call the helper routine delete-token-and-
        /// descendents to delete all the tokens involving it (all the necessary "root" tokens involving it are
        /// also conveniently on a list):
        /// </summary>
        /// <param name="wme">The wme.</param>
        public void RemoveWME(WME wme)
        {
            int pos = _working_memory.IndexOf(wme);
            if (pos < 0)
                return;

            WME w = _working_memory[pos];

            foreach (ItemInAlphaMemory item in w.AlphaMemoryItems)
            {
                item.AlphaMemory.Items.Remove(item);

                // *** Left Unlinking ***
                if (item.AlphaMemory.Items.Count == 0)
                {
                    foreach (ReteNode node in item.AlphaMemory.Successors)
                    {
                        if (node.Type == ReteNodeType.Join)
                        {
                            node.Parent.Children.Remove(node);
                            ((JoinNode) node).IsLeftUnlinked = true;
                        }
                    }
                }
                // *** End Left Unlinking ***
            }
            while (w.Tokens.Count > 0)
            {
                delete_token_and_descendents(w.Tokens.First.Value);
            }


            foreach (NegativeJoinResult jr in w.NegativeJoinResults)
            {
                jr.Owner.JoinResults.Remove(jr);
                if (jr.Owner.JoinResults.Count == 0)
                {
                    foreach (ReteNode child in jr.Owner.Node.Children)
                    {
                        left_activation(child, jr.Owner, null);
                    }
                }
            }

            _working_memory.Remove(w);
        }

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnRete(this);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds the production.
        /// </summary>
        /// <param name="new_production">The new_production.</param>
        /// <param name="lhs">The LHS.</param>
        private void AddProduction(ReteNode new_production, List<LeftHandSideCondition> lhs)
        {
            ReteNode current_node = build_or_share_network_for_conditions(_dummy_top_node, lhs, null);
            new_production.Parent = current_node;
            current_node.Children.AddFirst(new_production);
            update_new_node_with_matches_from_above(new_production);
        }

        /// <summary>
        /// Left_activations the specified new_node.
        /// </summary>
        /// <param name="new_node">The new_node.</param>
        /// <param name="tok">The tok.</param>
        /// <param name="w">The w.</param>
        private void left_activation(ReteNode new_node, Token tok, WME w)
        {
            switch (new_node.Type)
            {
                case ReteNodeType.BetaMemory:
                    beta_memory_left_activation(new_node as BetaMemory, tok, w);
                    break;
                case ReteNodeType.Negative:
                    negative_node_left_activation(new_node as NegativeNode, tok, w);
                    break;
                case ReteNodeType.Builtin:
                    builtin_node_left_activation(new_node as BuiltinMemory, tok, w);
                    break;
                case ReteNodeType.NCCPartner:
                    ncc_partner_node_left_activation(new_node as NCCPartnerNode, tok, w);
                    break;
                case ReteNodeType.NCC:
                    ncc_node_left_activation(new_node as NCCNode, tok, w);
                    break;
                case ReteNodeType.Join:
                    join_node_left_activation(new_node as JoinNode, tok);
                    break;
                case ReteNodeType.Production:
                    p_node_activation(new_node as ProductionNode, tok, w);
                    break;
                case ReteNodeType.Mutex:
                    mutex_node_activation(new_node as MutexNode, tok, w);
                    break;
                default:
                    throw new ApplicationException("Unknown left_activation type: " + new_node.GetType().Name);
            }
        }

        /// <summary>
        /// Mutex_node_activations the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="tok">The tok.</param>
        /// <param name="w">The w.</param>
        private void mutex_node_activation(MutexNode node, Token tok, WME w)
        {
            Token new_token = make_token(node, tok, w);
            node.Items.Add(new_token);
        }

        /// <summary>
        /// P_node_activations the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="tok">The tok.</param>
        /// <param name="w">The w.</param>
        private void p_node_activation(ProductionNode node, Token tok, WME w)
        {
            Token new_token = make_token(node, tok, w);
            node.Items.AddFirst(new_token);

            int lhsCnt = node.Production.Lhs.Count - 1;

            foreach (RightHandSideCondition rhsCondition in node.Production.Rhs)
            {
                WME newfact = new WME();
                for (int f = 0; f < 3; f++)
                {
                    Term term = rhsCondition.Fields[f];
                    if (term.TermType == TermType.Variable)
                    {
                        for (int i = lhsCnt; i >= 0; i--)
                        {
                            Condition lhsCondition = node.Production.Lhs[i];
                            if (lhsCondition.ConditionType == ConditionType.Positive)
                            {
                                int pos = lhsCondition.Contains(term);
                                if (pos >= 0)
                                {
                                    Token tok2 = new_token.GetTokenUp(lhsCnt - i);
                                    newfact.Fields[f] = tok2.WME[pos];
                                    i = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        newfact.Fields[f] = term;
                    }
                }
                Activation newact = new Activation(newfact, rhsCondition.ConditionType);
                if (node.Production.InferredFacts.Contains(newact) == false)
                {
                    node.Production.InferredFacts.Add(newact);
                }
            }
        }

        /// <summary>
        /// Lookup_in_hash_tables the specified id hash.
        /// </summary>
        /// <param name="idHash">The id hash.</param>
        /// <param name="attributeHash">The attribute hash.</param>
        /// <param name="valueHash">The value hash.</param>
        /// <returns></returns>
        private AlphaMemory lookup_in_hash_table(int idHash, int attributeHash, int valueHash)
        {
            if (_alpha_network.ContainsKey(attributeHash))
            {
                Dictionary<int, Dictionary<int, AlphaMemory>> valueDict = _alpha_network[attributeHash];
                if (valueDict.ContainsKey(valueHash))
                {
                    Dictionary<int, AlphaMemory> idDict = valueDict[valueHash];
                    if (idDict.ContainsKey(idHash))
                    {
                        return idDict[idHash];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// We will use several helper functions to make the main add-production procedure simpler.
        /// The first one, build-or-share-beta-memory-node, looks for an existing beta memory node that is
        /// a child of the given parent node. If there is one, it returns it so it can be shared by the new
        /// production; otherwise the function builds a new one and returns it. This pseudocode assumes
        /// that beta memories are not indexed; if indexing is used, the procedure would take an extra
        /// argument specifying which field(s) the memory must be indexed on.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        private ReteNode build_or_share_beta_memory_node(ReteNode parent)
        {
            if (parent is DummyTopNode)
            {
                return parent;
            }

            foreach (ReteNode child in parent.Children)
            {
                if (child.Type == ReteNodeType.BetaMemory)
                {
                    return child;
                }
            }

            BetaMemory new_rete = new BetaMemory();
            new_rete.Type = ReteNodeType.BetaMemory;
            new_rete.Parent = parent;
            new_rete.Label = "B" + (++_next_beta_node);
            parent.Children.AddFirst(new_rete);

            update_new_node_with_matches_from_above(new_rete);
            return new_rete;
        }

        /// <summary>
        /// The build-or-share-network-for-conditions helper function takes a list of conditions, builds
        /// or shares a network structure for them underneath the given parent node, and returns the
        /// lowermost node in the new-or-shared network. Note that the list of conditions can contain
        /// negative conditions or NCC's, not just positive conditions.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="conds">The conds.</param>
        /// <param name="earlier_conds">The earlier_conds.</param>
        /// <returns></returns>
        private ReteNode build_or_share_network_for_conditions(ReteNode parent, List<LeftHandSideCondition> conds, List<LeftHandSideCondition> earlier_conds)
        {
            List<LeftHandSideCondition> conds_higher_up;

            ReteNode current_node = parent;

            if (earlier_conds == null)
                conds_higher_up = new List<LeftHandSideCondition>();
            else
                conds_higher_up = earlier_conds;

            foreach (LeftHandSideCondition cond in conds)
            {
                if (cond.ConditionType == ConditionType.Positive)
                {
                    current_node = build_or_share_beta_memory_node(current_node);
                    LinkedList<TestAtJoinNode> tests = get_join_tests_from_condition(cond, conds_higher_up);
                    AlphaMemory am = build_or_share_alpha_memory(cond);
                    current_node = build_or_share_join_node(current_node as BetaMemory, am, tests);
                }
                else if (cond.ConditionType == ConditionType.Negative)
                {
                    LinkedList<TestAtJoinNode> tests = get_join_tests_from_condition(cond, conds_higher_up);
                    AlphaMemory am = build_or_share_alpha_memory(cond);
                    current_node = build_or_share_negative_node(current_node, am, tests);
                }
                else if (cond.ConditionType == ConditionType.NCC)
                {
                    current_node = build_or_share_ncc_nodes(current_node as JoinNode, cond, conds_higher_up);
                }
                else if (cond.ConditionType == ConditionType.Function)
                {
                    current_node = build_builtin_node(current_node, cond, conds_higher_up);
                }
                conds_higher_up.Add(cond);
            }
            return current_node;
        }

        /// <summary>
        /// Build_builtin_nodes the specified parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="c">The c.</param>
        /// <param name="earlier_conds">The earlier_conds.</param>
        /// <returns></returns>
        private BuiltinMemory build_builtin_node(ReteNode parent, Condition c, List<LeftHandSideCondition> earlier_conds)
        {
            BuiltinMemory new_node = new BuiltinMemory(c.ToString());
            new_node.Type = ReteNodeType.Builtin;
            new_node.Parent = parent;

            parent.Children.AddFirst(new_node);

            new_node.Builtin = ((FuncTerm) c.Attribute).Builtin;

            int cntOfEarlierConditions = earlier_conds.Count - 1;

            if (c.Id.TermType == TermType.Variable)
            {
                for (int i = cntOfEarlierConditions; i >= 0; i--)
                {
                    Condition earlier_cond = earlier_conds[i];
                    if (earlier_cond.ConditionType == ConditionType.Positive)
                    {
                        for (int f2 = 0; f2 < 3; f2++)
                        {
                            Variable o = earlier_cond.Fields[f2] as Variable;
                            if (o != null && o.Equals(c.Id))
                            {
                                VariableSubstituter vs = new VariableSubstituter();
                                vs.FieldNumber = f2;
                                vs.NumberOfLevelsUp = (cntOfEarlierConditions - i);
                                vs.BindingPair.Variable = o;
                                new_node.LeftArgument = vs;
                                f2 = 3;
                                i = -1; //escape loop of cntOfEarlierConditions
                            }
                        }
                    }
                }
            }
            else
            {
                new_node.LeftArgument = new ConstantSubstitutor(c.Id);
            }

            if (c.Value.TermType == TermType.Variable)
            {
                for (int i = cntOfEarlierConditions; i >= 0; i--)
                {
                    Condition earlier_cond = earlier_conds[i];
                    if (earlier_cond.ConditionType == ConditionType.Positive)
                    {
                        for (int f2 = 0; f2 < 3; f2++)
                        {
                            Variable o = earlier_cond.Fields[f2] as Variable;
                            if (o != null && o.Equals(c.Value))
                            {
                                VariableSubstituter vs = new VariableSubstituter();
                                vs.FieldNumber = f2;
                                vs.NumberOfLevelsUp = (cntOfEarlierConditions - i);
                                vs.BindingPair.Variable = o;
                                new_node.RightArgument = vs;
                                f2 = 3;
                                i = -1; //escape loop of cntOfEarlierConditions
                            }
                        }
                    }
                }
            }
            else
            {
                new_node.RightArgument = new ConstantSubstitutor(c.Value);
            }

            update_new_node_with_matches_from_above(new_node);
            return new_node;
        }

        /// <summary>
        /// When adding a production that uses an NCC, we use the build-or-share-ncc-nodes function.
        /// Most of the work in this function is done by the helper function build-or-share-network-for-
        /// conditions, which builds or shares the whole subnetwork for the subconditions of the NCC. The
        /// rest of the build-or-share-ncc-nodes function then builds or shares the NCC and NCC partner
        /// nodes.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="c">The c.</param>
        /// <param name="earlier_conds">The earlier_conds.</param>
        /// <returns></returns>
        private ReteNode build_or_share_ncc_nodes(JoinNode parent, Condition c, List<LeftHandSideCondition> earlier_conds)
        {
            ReteNode bottom_of_subnetwork = build_or_share_network_for_conditions(parent, c.SubConditions, earlier_conds);
            foreach (ReteNode child in parent.Children)
            {
                if (child.Type == ReteNodeType.NCC && ((NCCNode) child).Partner.Parent == bottom_of_subnetwork)
                {
                    return child;
                }
            }
            NCCNode new_node = new NCCNode();
            NCCPartnerNode new_partner = new NCCPartnerNode();
            new_node.Type = ReteNodeType.NCC; // "NCC";
            new_partner.Type = ReteNodeType.NCCPartner; // "NCC-partner";
            parent.IsHeadOfSubNetwork = true;
            new_node.Parent = parent;
            parent.Children.AddLast(new_node);
            new_partner.Parent = bottom_of_subnetwork;
            bottom_of_subnetwork.Children.AddFirst(new_partner);
            new_node.Partner = new_partner;
            new_partner.NCCNode = new_node;
            new_partner.NumberOfConjuncts = c.SubConditions.Count;
            update_new_node_with_matches_from_above(new_node);
            update_new_node_with_matches_from_above(new_partner);
            return new_node;
        }

        /// <summary>
        /// The function for creating new negative nodes is similar to the ones for creating beta memories
        /// and join nodes. However, one additional consideration is important with negative conditions,
        /// and also with conjunctive negations. Any time there is a variable &lt;v&gt; which is tested in a negative
        /// condition and bound in one or more other (positive) conditions, at least one of these positive
        /// conditions must come before the negative condition. Recall that when we add a production to
        /// the network, the network-construction routines are given a list of its conditions in some order. If
        /// all conditions are positive, any order will work. Negative conditions require the aforementioned
        /// constraint on the order, though, because negative nodes need to be able to access the appropriate
        /// variable bindings in tokens, and the tokens "seen" by negative nodes indicate only variable
        /// bindings from earlier conditions, i.e., conditions higher up in the network.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="am">The am.</param>
        /// <param name="tests">The tests.</param>
        /// <returns></returns>
        private ReteNode build_or_share_negative_node(ReteNode parent, AlphaMemory am, LinkedList<TestAtJoinNode> tests)
        {
            foreach (ReteNode child in parent.Children)
            {
                if (child.Type == ReteNodeType.Negative)
                {
                    NegativeNode negativenodeChild = (NegativeNode) child;
                    if (negativenodeChild.AlphaMemory == am)
                    {
                        if (negativenodeChild.Tests.Count == 0 && tests.Count == 0)
                        {
                            return child;
                        }
                        else
                        {
                            //Need to compare tests...
                            throw new ApplicationException("Unhandled...");
                        }
                    }
                }
            }

            NegativeNode new_node = new NegativeNode();
            new_node.Type = ReteNodeType.Negative; // "negative";
            new_node.Parent = parent;

            parent.Children.AddFirst(new_node);

            foreach (TestAtJoinNode test in tests)
            {
                new_node.Tests.AddLast(test);
            }
            new_node.AlphaMemory = am;

            am.Successors.AddFirst(new_node);
            ++am.ReferenceCount;
            new_node.NearestAncestorWithSameAmem = find_nearest_ancestor_with_same_amem(parent, am);
            update_new_node_with_matches_from_above(new_node);

            // *** Right Unlinking ***    
            if (new_node.Items.Count == 0)
            {
                am.Successors.Remove(new_node);
                new_node.IsRightUnlinked = true;
            }
            // *** End Right Unlinking ***

            return new_node;
        }

        /// <summary>
        /// The next helper function is similar, except it handles join nodes rather than beta memory
        /// nodes. The two additional arguments specify the alpha memory to which the join node must
        /// be attached and the variable binding consistency checks it must perform. Note that there is no
        /// need to call update-new-node-with-matches-from-above in this case, because a join node does not
        /// store any tokens, and a newly created join node has no children onto which join results should
        /// be passed.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="am">The am.</param>
        /// <param name="tests">The tests.</param>
        /// <returns></returns>
        private ReteNode build_or_share_join_node(BetaMemory parent, AlphaMemory am, LinkedList<TestAtJoinNode> tests)
        {
            foreach (ReteNode child in parent.AllChildren)
            {
                if (child.Type == ReteNodeType.Join)
                {
                    JoinNode childJoinNode = (JoinNode) child;
                    if (childJoinNode.AlphaMemory == am)
                    {
                        if (childJoinNode.Tests.Count == 0 && tests.Count == 0)
                        {
                            return child;
                        }
                        else
                        {
                            if (childJoinNode.Tests.Count == tests.Count)
                            {
                                bool testsMatch = true;
                                foreach (TestAtJoinNode test in tests)
                                {
                                    if (childJoinNode.Tests.Contains(test) == false)
                                    {
                                        testsMatch = false;
                                        continue;
                                    }
                                }
                                if (testsMatch)
                                {
                                    return child;
                                }
//Need to compare tests...
                                throw new ApplicationException("Unhandled...");
                            }
                        }
                    }
                }
            }

            JoinNode new_node = new JoinNode();
            new_node.Type = ReteNodeType.Join; // "join";
            new_node.Parent = parent;

            parent.Children.AddFirst(new_node);

            parent.AllChildren.AddFirst(new_node);

            foreach (TestAtJoinNode test in tests)
            {
                new_node.Tests.AddLast(test);
            }
            new_node.AlphaMemory = am;

            am.Successors.AddFirst(new_node);
            ++am.ReferenceCount;
            new_node.NearestAncestorWithSameAmem = find_nearest_ancestor_with_same_amem(parent, am);


            // *** Right Unlinking *** 
            if (parent.Items.Count == 0)
            {
                am.Successors.Remove(new_node);
                new_node.IsRightUnlinked = true;
            }
            else if (am.Items.Count == 0)
            {
                if ((parent is DummyTopNode) == false)
                {
                    parent.Children.Remove(new_node);
                    new_node.IsRightUnlinked = true;
                }
            }
            // *** End Right Unlinking *** 

            return new_node;
        }

        /// <summary>
        /// For right unlinking, we need to initialize the nearest-ancestor-with-same-amem fields on
        /// newly-created join and negative nodes. The find-nearest-ancestor-with-same-amem procedure
        /// finds the appropriate value. It starts at a given node and walks up the beta network, returning
        /// the first node it finds that uses a given alpha memory. Note that this node may be in the
        /// subnetwork for a conjunctive negation.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="am">The am.</param>
        /// <returns></returns>
        private ReteNode find_nearest_ancestor_with_same_amem(ReteNode node, AlphaMemory am)
        {
            if (node is DummyTopNode)
            {
                return null;
            }
            if (node.Type == ReteNodeType.Dummy)
            {
                return null;
            }
            if (node.Type == ReteNodeType.Join)
            {
                if (((JoinNode) node).AlphaMemory == am)
                {
                    return node;
                }
            }
            if (node.Type == ReteNodeType.Negative)
            {
                if (((NegativeNode) node).AlphaMemory == am)
                {
                    return node;
                }
            }
            if (node.Type == ReteNodeType.NCC)
            {
                return find_nearest_ancestor_with_same_amem(((NCCNode) node).Partner.Parent, am);
            }
            return find_nearest_ancestor_with_same_amem(node.Parent, am);
        }

        /// <summary>
        /// Finally, we have a helper function for creating a new alpha memory for a given condition,
        /// or finding an existing one to share. The implementation of this function depends on what type
        /// of alpha net implementation is used. If we use a traditional data        /// ow network, as described in
        /// Section 2.2.1, then we simply start at the top of the alpha network and work our way down,
        /// sharing or building new constant test nodes:
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private AlphaMemory build_or_share_alpha_memory(Condition c)
        {
            AlphaMemory am;

            int attributeHash = c.Attribute.GetHashCode();
            int valueHash = c.Value.GetHashCode();
            int idHash = c.Id.GetHashCode();

            if (_alpha_network.ContainsKey(attributeHash))
            {
                Dictionary<int, Dictionary<int, AlphaMemory>> valueDict = _alpha_network[attributeHash];
                if (valueDict.ContainsKey(valueHash))
                {
                    Dictionary<int, AlphaMemory> idDict = valueDict[valueHash];
                    if (idDict.ContainsKey(idHash))
                    {
                        am = idDict[idHash];
                        am.Conditions.Add(c.ToString());
                        return am;
                    }
                    else //no idHash
                    {
                        am = new AlphaMemory();
                        am.ReferenceCount = 0;
                        am.Label = "A" + (++_next_alpha_node);
                        am.Conditions.Add(c.ToString());
                        idDict.Add(idHash, am);
                    }
                }
                else //no valueHash
                {
                    am = new AlphaMemory();
                    am.ReferenceCount = 0;
                    am.Label = "A" + (++_next_alpha_node);
                    am.Conditions.Add(c.ToString());
                    Dictionary<int, AlphaMemory> idDict = new Dictionary<int, AlphaMemory>();
                    idDict.Add(idHash, am);
                    valueDict.Add(valueHash, idDict);
                }
            }
            else //no attribHash
            {
                am = new AlphaMemory();
                am.ReferenceCount = 0;
                am.Label = "A" + (++_next_alpha_node);
                am.Conditions.Add(c.ToString());
                Dictionary<int, AlphaMemory> idDict = new Dictionary<int, AlphaMemory>();
                idDict.Add(idHash, am);

                Dictionary<int, Dictionary<int, AlphaMemory>> valueDict = new Dictionary<int, Dictionary<int, AlphaMemory>>();
                valueDict.Add(valueHash, idDict);

                _alpha_network.Add(attributeHash, valueDict);
            }

            foreach (WME w in _working_memory)
            {
                if (check_constant_tests(w, c))
                {
                    alpha_memory_activation(am, w);
                }
            }

            return am;
        }

        /// <summary>
        /// Check_constant_testses the specified w.
        /// </summary>
        /// <param name="w">The w.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private bool check_constant_tests(WME w, Condition c)
        {
            foreach (Term o in c.Fields)
            {
                if ((o is Variable) == false)
                {
                    if (w.Contains(o) == false)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <para>
        /// Our next helper function, get-join-tests-from-condition, takes a condition and builds a list of
        /// all the variable binding consistency tests that need to be performed by its join node. To do this,
        /// it needs to know what all the earlier conditions are, so it can determine whether a given variable
        /// appeared in them - in which case its occurrence in the current condition means a consistency
        /// test is needed - or whether it is simply a new (not previously seen) variable - in which case
        /// no test is needed. If a variable v has more than one previous occurrence, we still only need
        /// one consistency test for it - join nodes for earlier conditions will ensure that all the previous
        /// occurrences are equal, so the current join node just has to make sure the current WME has the
        /// same value for it as any one of the previous occurrences. The pseudocode below always chooses
        /// the nearest (i.e., most recent) occurrence for the test, because with list-form tokens, the nearest
        /// occurrence is the cheapest to access. With array-form tokens, this choice does not matter.
        /// </para>
        /// <para>
        /// We make one minor change to get-join-tests-from-condition. We must now allow earlier-
        /// conds to contain negative conditions. Occurrences of variables inside negative conditions do not
        /// represent bindings of those variables -- a negative condition tests for the absence of something
        /// in working memory, so there will be no binding. Thus, we modify the function so that it ignores
        /// negative conditions in earlier-conds.
        /// </para>
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="earlier_conds">The earlier_conds.</param>
        /// <returns></returns>
        private LinkedList<TestAtJoinNode> get_join_tests_from_condition(Condition c, List<LeftHandSideCondition> earlier_conds)
        {
            LinkedList<TestAtJoinNode> result = new LinkedList<TestAtJoinNode>();
            int cntOfEarlierConditions = earlier_conds.Count - 1;
            for (int f = 0; f < 3; f++)
            {
                Variable v = c.Fields[f] as Variable;
                if (v != null)
                {
                    for (int i = cntOfEarlierConditions; i >= 0; i--)
                    {
                        Condition earlier_cond = earlier_conds[i];
                        if (earlier_cond.ConditionType == ConditionType.Positive)
                        {
                            for (int f2 = 0; f2 < 3; f2++)
                            {
                                Variable o = earlier_cond.Fields[f2] as Variable;
                                if (o != null && o.Equals(v))
                                {
                                    TestAtJoinNode this_test = new TestAtJoinNode();
                                    this_test.FieldOfArg1 = f;
                                    this_test.ConditionNumberOfArg2 = i;
                                    this_test.FieldOfArg2 = f2;
                                    this_test.NumberOfLevelsUp = (cntOfEarlierConditions - i);
                                    this_test.Evaluator = c.Evaluator;
                                    result.AddLast(this_test);
                                    i = -1;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// The update-new-node-with-matches-from-above procedure initializes the memory node to store
        /// tokens for any existing matches for the earlier conditions.
        /// </summary>
        /// <remarks>
        /// Finally, we give the update-new-node-with-matches-from-above procedure. This is needed
        /// to ensure that newly added productions are immediately matched against the current working
        /// memory. The procedure's job is to ensure that the given new-node's left-activation procedure is
        /// called with all the existing matches for the previous conditions, so that the new-node can take
        /// any appropriate actions (e.g., a beta memory stores the matches as new tokens, and a p-node
        /// signals new complete matches for the production). How update-new-node-with-matches-from-
        /// above achieves this depends on what kind of node the new-node's parent is. If the parent is a
        /// beta memory (or a node for a negated condition, as we will discuss later), this is straightforward,
        /// since the parent has a list (items) of exactly the matches we want. But if the parent node is a
        /// join node, we want to find these matches, we iterate over the WMEs and tokens in the join node's alpha
        /// and beta memories and perform the join tests on each pair. The pseudocode below uses a trick to
        /// do this: while temporarily pretending the new-node is the only child of the join node, it runs the
        /// join node's right-activation procedure for all the WMEs in its alpha memory; any new matches
        /// will automatically be propagated to the new-node. For a variation of this implementation, see
        /// (Tambe et al., 1988); for a general discussion, see (Lee and Schor, 1992).
        /// </remarks>
        /// <param name="newNode">The new node.</param>
        private void update_new_node_with_matches_from_above(ReteNode newNode)
        {
            ReteNode parent = newNode.Parent;

            switch (parent.Type)
            {
                case ReteNodeType.BetaMemory:
                    BetaMemory tmpBetaMem = (BetaMemory) parent;

                    foreach (Token tok in tmpBetaMem.Items)
                    {
                        left_activation(newNode, tok, new WME());
                    }
                    break;
                case ReteNodeType.Builtin:
                    BuiltinMemory tmpBiMem = (BuiltinMemory) parent;

                    foreach (Token tok in tmpBiMem.Items)
                    {
                        left_activation(newNode, tok, new WME());
                    }
                    break;
                case ReteNodeType.Join:

                    ReteNode[] saved_list_of_children = new ReteNode[parent.Children.Count];
                    parent.Children.CopyTo(saved_list_of_children, 0); //.ToArray();
                    parent.Children.Clear();
                    parent.Children.AddFirst(newNode);
                    JoinNode tmpJNode = (JoinNode) parent;
                    if (tmpJNode.AlphaMemory != null)
                    {
                        foreach (ItemInAlphaMemory item in tmpJNode.AlphaMemory.Items)
                        {
                            right_activation(parent, item.WME);
                        }
                    }
                    parent.Children.Clear();

                    foreach (ReteNode retenode in saved_list_of_children)
                    {
                        parent.Children.AddLast(retenode);
                    }
                    break;
                case ReteNodeType.Negative:
                    NegativeNode tmpNNode = (NegativeNode) parent;
                    foreach (Token tok in tmpNNode.Items)
                    {
                        if (tok.JoinResults.Count == 0)
                        {
                            left_activation(newNode, tok, null);
                        }
                    }
                    break;
                case ReteNodeType.NCC:
                    NCCNode tmpNCCNode = (NCCNode) parent;
                    foreach (Token tok in tmpNCCNode.Items)
                    {
                        if (tok.NCCResults.Count == 0)
                        {
                            left_activation(newNode, tok, null);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Controls the activation flow based on node type
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="w">The w.</param>
        private void right_activation(ReteNode child, WME w)
        {
            switch (child.Type)
            {
                case ReteNodeType.Join:
                    join_node_right_activation(child as JoinNode, w);
                    break;
                case ReteNodeType.Negative:
                    negative_node_right_activation(child as NegativeNode, w);
                    break;
                default:
                    throw new ApplicationException("???");
            }
        }

        /// <summary>
        /// To remove an existing production from the network, we start down at the bottom of the
        /// beta network, at the p-node for that production. The basic idea is to start walking from there
        /// up to the top of the net. At each node, we clean up any tokens it contains, and then get rid of
        /// the node | i.e., remove it from the children or successors lists on its predecessors (its parent
        /// and, for some nodes, its alpha memory as well), and deallocate it. We then move up to the
        /// predecessors. If the alpha memory is not being shared by another production, we deallocate it
        /// too. If the parent is not being shared by another production, then we apply the same procedure
        /// to it | clean up its tokens, etc. | and repeat this until we reach either a node being shared by
        /// some other production, or the top of the beta network.
        /// </summary>
        /// <param name="node">The node.</param>
        private void delete_node_and_any_unused_ancestors(ReteNode node)
        {
            if (node.Type == ReteNodeType.NCC)
            {
                delete_node_and_any_unused_ancestors(node);
            }
            if (node.Type == ReteNodeType.BetaMemory)
            {
                while (((BetaMemory) node).Items.Count > 0)
                {
                    delete_token_and_descendents(((BetaMemory) node).Items.First.Value);
                }
            }
            if (node.Type == ReteNodeType.Negative)
            {
                while (((NegativeNode) node).Items.Count > 0)
                {
                    delete_token_and_descendents(((NegativeNode) node).Items.First.Value);
                }
            }
            if (node.Type == ReteNodeType.NCC)
            {
                while (((NCCNode) node).Items.Count > 0)
                {
                    delete_token_and_descendents(((NCCNode) node).Items.First.Value);
                }
            }
            if (node.Type == ReteNodeType.NCCPartner)
            {
                while (((NCCPartnerNode) node).NewResultBuffer.Count > 0)
                {
                    delete_token_and_descendents(((NCCPartnerNode) node).NewResultBuffer.First.Value);
                }
            }

            if (node.Type == ReteNodeType.Production)
            {
                ProductionNode pNode = (ProductionNode) node;
                while (pNode.Items.Count > 0)
                {
                    delete_token_and_descendents(pNode.Items.First.Value);
                    //pNode.items.RemoveFirst();
                }

                //_rules_that_fired.Remove(pNode);
            }

            if (node.Type == ReteNodeType.Join)
            {
                if (((JoinNode) node).IsRightUnlinked == false)
                {
                    ((JoinNode) node).AlphaMemory.Successors.Remove(node);
                }

                if (((JoinNode) node).IsLeftUnlinked == false)
                {
                    node.Parent.Children.Remove(node);
                }

                --((JoinNode) node).AlphaMemory.ReferenceCount;
                if (((JoinNode) node).AlphaMemory.ReferenceCount == 0)
                {
                    delete_alpha_memory(((JoinNode) node).AlphaMemory);
                }
            }

            if (node.Type == ReteNodeType.Negative)
            {
                if (((NegativeNode) node).IsRightUnlinked == false)
                {
                    ((NegativeNode) node).AlphaMemory.Successors.Remove(node);
                }
                --((NegativeNode) node).AlphaMemory.ReferenceCount;
                if (((NegativeNode) node).AlphaMemory.ReferenceCount == 0)
                {
                    delete_alpha_memory(((NegativeNode) node).AlphaMemory);
                }
            }

            if (node.Type == ReteNodeType.Join)
            {
                JoinNode tmpNode = (JoinNode) node;
                ((BetaMemory) tmpNode.Parent).AllChildren.Remove(node);
                if (((BetaMemory) tmpNode.Parent).AllChildren.Count == 0)
                {
                    delete_node_and_any_unused_ancestors(((JoinNode) node).Parent);
                }
            }
            else if (node.Parent.Children.Count == 0)
            {
                delete_node_and_any_unused_ancestors(node.Parent);
            }
        }

        /// <summary>
        /// Delete_alpha_memories the specified amem.
        /// </summary>
        /// <param name="amem">The amem.</param>
        private void delete_alpha_memory(AlphaMemory amem)
        {
            Console.WriteLine(amem);
            throw new Exception();
        }

        /// <summary>
        /// The helper routine delete-token-and-descendents removes a token together with its entire tree
        /// of descendents. For simplicity, the pseudocode here is recursive; an actual implementation may
        /// be made slightly faster by using a nonrecursive tree traversal method.
        /// </summary>
        /// <param name="tok">The tok.</param>
        private void delete_token_and_descendents(Token tok)
        {
            while (tok.Children.Count > 0)
            {
                delete_token_and_descendents(tok.Children.First.Value as Token);
            }

            if (tok.Node.Type == ReteNodeType.NCCPartner)
            {
            }
            else
            {
                if (tok.Node.Type == ReteNodeType.BetaMemory)
                {
                    ((BetaMemory) tok.Node).Items.Remove(tok);
                }
                if (tok.Node.Type == ReteNodeType.Production)
                {
                    ((ProductionNode) tok.Node).Items.Remove(tok);
                }
                if (tok.Node.Type == ReteNodeType.Negative)
                {
                    ((NegativeNode) tok.Node).Items.Remove(tok);
                }
            }


            if (tok.WME != null)
            {
                tok.WME.Tokens.Remove(tok);
            }

            tok.Parent.Children.Remove(tok);

            // *** Right Unlinking *** 
            if (tok.Node.Type == ReteNodeType.BetaMemory)
            {
                if (((BetaMemory) tok.Node).Items.Count == 0)
                {
                    foreach (JoinNode child in tok.Node.Children)
                    {
                        child.AlphaMemory.Successors.Remove(child);
                        child.IsRightUnlinked = true;
                    }
                }
            }
            // *** End Right Unlinking *** 

            if (tok.Node.Type == ReteNodeType.Production)
            {
                ProductionNode prodNode = (ProductionNode) tok.Node;
                if (prodNode.Items.Count == 0)
                {
                    foreach (JoinNode child in tok.Node.Children)
                    {
                        child.AlphaMemory.Successors.Remove(child);
                        child.IsRightUnlinked = true;
                    }
                    prodNode.Production.InferredFacts.Clear();
                }
            }

            if (tok.Node.Type == ReteNodeType.Negative)
            {
                // *** Right Unlinking *** 
                if (((NegativeNode) tok.Node).Items.Count == 0)
                {
                    ((NegativeNode) tok.Node).AlphaMemory.Successors.Remove(tok.Node);
                    ((NegativeNode) tok.Node).IsRightUnlinked = true;
                }
                // *** End Right Unlinking *** 

                foreach (NegativeJoinResult jr in tok.JoinResults)
                {
                    jr.WME.NegativeJoinResults.Remove(jr);
                }
            }
            if (tok.Node.Type == ReteNodeType.NCC)
            {
                foreach (Token result_token in tok.NCCResults)
                {
                    result_token.WME.Tokens.Remove(result_token);
                    result_token.Parent.Children.Remove(result_token);
                }
            }
            if (tok.Node.Type == ReteNodeType.NCCPartner)
            {
                tok.Owner.NCCResults.Remove(tok);
                if (tok.Owner.NCCResults.Count == 0)
                {
                    foreach (ReteNode child in ((NCCPartnerNode) tok.Node).NCCNode.Children)
                    {
                        left_activation(child, tok.Owner, null);
                    }
                }
            }
        }

        /// <summary>
        /// Delete_descendents_of_tokens the specified t.
        /// </summary>
        /// <param name="t">The t.</param>
        private void delete_descendents_of_token(Token t)
        {
            while (t.Children.Count > 0)
            {
                delete_token_and_descendents(t.Children.First.Value as Token);
            }
        }

        /// <summary>
        /// Similarly, whenever a new token tok = ht;wi is added to a beta memory, we add tok to
        /// t.children and to w.tokens. We also fill in the new node field on the token. To simplify our
        /// pseudocode, it is convenient to define a "helper" function make-token which builds a new token
        /// and initializes its various fields as necessary for tree-based removal. Although we write this as
        /// a separate function, it would normally be coded "inline" for efficiency.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="w">The w.</param>
        /// <returns></returns>
        private Token make_token(ReteNode node, Token parent, WME w)
        {
            Token tok = new Token();
            tok.Parent = parent;
            tok.WME = w;
            tok.Node = node;
            parent.Children.AddFirst(tok);
            if (w != null)
            {
                w.Tokens.AddFirst(tok);
            }
            return tok;
        }

        /// <summary>
        /// Upon a right activation (when a new WME w is added to the alpha memory), we look
        /// through the beta memory and find any token(s) t for which all these t-versus-w tests succeed.
        /// Any successful (t;w) combinations are passed on to the join node's children. 
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="w">The w.</param>
        private void join_node_right_activation(JoinNode node, WME w)
        {
            // *** Left Unlinking ***
            if (node.IsLeftUnlinked)
            {
                relink_to_beta_memory(node);
                if (((BetaMemory) node.Parent).Items.Count == 0)
                {
                    node.AlphaMemory.Successors.Remove(node);
                    node.IsRightUnlinked = true;
                }
            }
            // *** End Left Unlinking ***

            foreach (Token t in ((BetaMemory) node.Parent).Items)
            {
                if (perform_join_tests(node.Tests, t, w))
                {
                    foreach (ReteNode child in node.Children)
                    {
                        left_activation(child, t, w);
                    }
                }
            }
        }

        /// <summary>
        /// Relink_to_alpha_memories the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void relink_to_alpha_memory(NegativeNode node)
        {
            NegativeNode ancestor = (NegativeNode) node.NearestAncestorWithSameAmem;
            while (ancestor != null && ancestor.IsRightUnlinked)
            {
                ancestor = (NegativeNode) ancestor.NearestAncestorWithSameAmem;
            }
            if (ancestor != null)
            {
                LinkedListNode<ReteNode> isx = node.AlphaMemory.Successors.Find(ancestor);
                node.AlphaMemory.Successors.AddBefore(isx, node);
            }
            else
            {
                node.AlphaMemory.Successors.AddLast(node);
            }
            node.IsRightUnlinked = false;
        }

        /// <summary>
        /// Relink_to_alpha_memories the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void relink_to_alpha_memory(JoinNode node)
        {
            JoinNode ancestor = (JoinNode) node.NearestAncestorWithSameAmem;
            while (ancestor != null && ancestor.IsRightUnlinked)
            {
                ancestor = (JoinNode) ancestor.NearestAncestorWithSameAmem;
            }
            if (ancestor != null)
            {
                LinkedListNode<ReteNode> isx = node.AlphaMemory.Successors.Find(ancestor);
                node.AlphaMemory.Successors.AddBefore(isx, node);
            }
            else
            {
                node.AlphaMemory.Successors.AddLast(node);
            }
            node.IsRightUnlinked = false;
        }

        /// <summary>
        /// On a right activation of a negative node (when a WME is added to its alpha memory),
        /// we look for any tokens in its memory consistent with the WME; for each such token, we add
        /// this WME to its local result memory. Also, if the number of results changes from zero to
        /// one - indicating that the negated condition was previously true but is now false - then we
        /// call the delete-descendents-of-token helper function to delete any tokens lower in the network
        /// that depend on this token.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="w">The w.</param>
        private void negative_node_right_activation(NegativeNode node, WME w)
        {
            foreach (Token t in node.Items)
            {
                if (perform_join_tests(node.Tests, t, w))
                {
                    if (t.JoinResults.Count == 0)
                    {
                        delete_descendents_of_token(t);
                    }
                    NegativeJoinResult jr = new NegativeJoinResult();
                    jr.Owner = t;
                    jr.WME = w;
                    t.JoinResults.AddFirst(jr);
                    w.NegativeJoinResults.AddFirst(jr);
                }
            }
        }

        /// <summary>
        /// Relink_to_beta_memories the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void relink_to_beta_memory(JoinNode node)
        {
            node.Parent.Children.AddFirst(node);
            node.IsLeftUnlinked = false;
        }

        /// <summary>
        /// Whenever a beta memory is informed of a new match (consisting of an existing token and some
        /// WME), we build a token, add it to the list in the beta memory, and inform each of the beta
        /// memory's children:
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="t">The token.</param>
        /// <param name="w">The wme.</param>
        private void beta_memory_left_activation(BetaMemory node, Token t, WME w)
        {
            Token new_token = make_token(node, t, w);
            node.Items.AddFirst(new_token);


            if (node.Children.Count > 0)
            {
                LinkedListNode<ReteNode> llNode = node.Children.First;
                while (llNode != null)
                {
                    LinkedListNode<ReteNode> tmpllNode = llNode.Next;
                    left_activation(llNode.Value, new_token, null);
                    if (llNode.Next != null)
                        llNode = llNode.Next;
                    else
                        llNode = tmpllNode;
                }
            }
        }

        /// <summary>
        /// On a left activation
        /// (when there is a new match for all the earlier conditions), we build and store a new token, perform
        /// a join for the token, store the join results in the token structure, and pass the token onto any
        /// successor nodes if there were no join results.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="t">The t.</param>
        /// <param name="w">The w.</param>
        private void negative_node_left_activation(NegativeNode node, Token t, WME w)
        {
            // *** Right Unlinking *** 
            if (node.Items.Count == 0)
                relink_to_alpha_memory(node);
            // *** End Right Unlinking *** 

            Token new_token = make_token(node, t, w);
            node.Items.AddFirst(new_token);
            foreach (ItemInAlphaMemory item in node.AlphaMemory.Items)
            {
                if (perform_join_tests(node.Tests, new_token, item.WME))
                {
                    NegativeJoinResult jr = new NegativeJoinResult();
                    jr.Owner = new_token;
                    jr.WME = w;
                    new_token.JoinResults.AddFirst(jr);
                    w.NegativeJoinResults.AddFirst(jr);
                }
            }
            if (new_token.JoinResults.Count == 0)
            {
                foreach (ReteNode child in node.Children)
                {
                    left_activation(child, new_token, null);
                }
            }
        }

        /// <summary>
        /// Ncc_partner_node_left_activations the specified partner.
        /// </summary>
        /// <remarks>
        /// To handle an NCC partner node (left) activation, we take the new match from the subnetwork
        /// and build a "result" token to store it. (The pseudocode for this is shown in Figure 2.8.) Next we
        /// try to find the appropriate owner token in the NCC node's memory. (There might be one there,
        /// if this is a new subconditions match for an old preceding-conditions match, or there might not
        /// be one there, if this is an initial subconditions match for a new preceding-conditions match.)
        /// If we find an appropriate owner token, then we add the new result token to its local memory;
        /// if the number of results in the local memory changes from zero to one | indicating that the
        /// NCC was previously true but is now false | then we call the delete-descendents-of-token helper
        /// function to delete any tokens lower in the network that depend on this owner token. (This is
        /// similar to the negative-node-right-activation procedure on page 43.) On the other hand, if there
        /// isn't an appropriate owner token already in the NCC node's memory, then this new result token
        /// is placed in the new-result-buffer. (The NCC node will soon be activated and collect any new
        /// results from the buffer.)
        /// </remarks>
        /// <param name="partner">The partner.</param>
        /// <param name="t">The t.</param>
        /// <param name="w">The w.</param>
        private void ncc_partner_node_left_activation(NCCPartnerNode partner, Token t, WME w)
        {
            NCCNode nccNode = partner.NCCNode;
            Token new_result = make_token(partner, t, w);
            Token owners_t = t;
            WME owners_w = w;
            for (int i = 0; i < partner.NumberOfConjuncts; i++)
            {
                owners_w = owners_t.WME;
                owners_t = owners_t.Parent;
            }

            foreach (Token owner in nccNode.Items)
            {
                if (owner.Parent == owners_t && owner.WME == owners_w)
                {
                    owner.NCCResults.AddLast(new_result);
                    new_result.Owner = owner;
                    delete_descendents_of_token(owner);
                }
                else
                {
                    partner.NewResultBuffer.AddFirst(new_result);
                }
            }
        }

        /// <summary>
        /// Ncc_node_left_activations the specified node.
        /// </summary>
        /// <remarks>
        /// Our ncc-node-left-activation procedure is similar to the negative-node-left-activation procedure
        /// (page 42). In both cases, we need to find the join results for a new token. For negative
        /// nodes, we compute these join results by scanning the WMEs in an alpha memory and performing
        /// the join tests on them. For NCC nodes, the join results have already been computed by the
        /// subnetwork, so we simply look at the new-result-buffer in the NCC partner node to find them.
        /// </remarks>
        /// <param name="node">The node.</param>
        /// <param name="t">The t.</param>
        /// <param name="w">The w.</param>
        private void ncc_node_left_activation(NCCNode node, Token t, WME w)
        {
            Token new_token = make_token(node, t, w);
            node.Items.AddFirst(new_token);
            foreach (Token result in node.Partner.NewResultBuffer)
            {
                node.Partner.NewResultBuffer.Remove(result);
                new_token.NCCResults.AddFirst(result);
                result.Owner = new_token;
            }
            if (new_token.NCCResults.Count == 0)
            {
                foreach (ReteNode child in node.Children)
                {
                    left_activation(child, new_token, null);
                }
            }
        }

        /// <summary>
        /// Whenever a new WME is filtered through the alpha network and reaches an alpha memory, we
        /// simply add it to the list of other WMEs in that memory, and inform each of the attached join
        /// nodes:
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="w">The w.</param>
        private void alpha_memory_activation(AlphaMemory node, WME w)
        {
            ItemInAlphaMemory new_item = new ItemInAlphaMemory();
            new_item.WME = w;
            new_item.AlphaMemory = node;
            node.Items.AddFirst(new_item);
            w.AlphaMemoryItems.AddFirst(new_item);

            // *** Neo Integration *** 

            if (w.Value.TermType == TermType.EntityObject)
            {
                IFactProvider eo = (IFactProvider)w.Value.Value;
                if (eo.MyFactsHaveBeenAsserted == false)
                {
                    foreach (WME wme in eo.GenerateFactsForRelatedObject(w.Attribute.Value.ToString(), w.Identifier.Value as IFactProvider))
                    {
                        AddWME(wme);
                    }
                }

                //EntityObjectTerm eot = (EntityObjectTerm) w.Value;
                //if (eot.Value.MyFactsHaveBeenAsserted == false)
                //{
                //    foreach (WME wme in eot.Value.GenerateFactsForRelatedObject(w.Attribute.Value.ToString(), w.Identifier.Value as IFactProvider))
                //    {
                //        AddWME(wme);
                //    }
                //}
            }

            if (w.Value.TermType == TermType.ObjectRelation)
            {
                ObjectRelationBase orb = (ObjectRelationBase)w.Value.Value;
                foreach (RulesEnabledEntityObject eo in orb)
                {
                    if (eo.MyFactsHaveBeenAsserted == false)
                    {
                        foreach (WME wme in eo.GenerateFactsForObjectInCollection(w.Attribute.Value.ToString(), orb))
                        {
                            AddWME(wme);
                        }
                    }
                }

                //ObjectRelationTerm eot = (ObjectRelationTerm) w.Value;
                //foreach (RulesEnabledEntityObject eo in eot.Value)
                //{
                //    if (eo.MyFactsHaveBeenAsserted == false)
                //    {
                //        foreach (WME wme in eo.GenerateFactsForObjectInCollection(w.Attribute.Value.ToString(), eot.Value))
                //        {
                //            AddWME(wme);
                //        }
                //    }
                //}
            }

            // *** End Neo Integration *** 

            LinkedListNode<ReteNode> llNode = node.Successors.First;
            while (llNode != null)
            {
                LinkedListNode<ReteNode> tmpllNode = llNode.Next;
                right_activation(llNode.Value, w);
                if (llNode.Next != null)
                    llNode = llNode.Next;
                else
                    llNode = tmpllNode;
            }
        }

        /// <summary>
        /// Upon a left activation (when a new token t is added to the beta memory), we look through the
        /// alpha memory and find any WME(s) w for which all these t-versus-w tests succeed. Again, any
        /// successful (t;w) combinations are passed on to the node's children:
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="t">The t.</param>
        private void join_node_left_activation(JoinNode node, Token t)
        {
            // *** Right Unlinking *** 
            if (node.IsRightUnlinked)
            {
                relink_to_alpha_memory(node);

                // *** Left Unlinking ***
                if (node.AlphaMemory.Items.Count == 0)
                {
                    node.Parent.Children.Remove(node);
                    node.IsLeftUnlinked = true;
                }
                // *** End Left Unlinking ***
            }
            // *** End Right Unlinking *** 

            foreach (ItemInAlphaMemory item in node.AlphaMemory.Items)
            {
                if (perform_join_tests(node.Tests, t, item.WME))
                {
                    foreach (ReteNode child in node.Children)
                    {
                        left_activation(child, t, item.WME);
                    }
                }
            }
        }

        /// <summary>
        /// Builtin_node_left_activations the specified memory.
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <param name="tok">The tok.</param>
        /// <param name="w">The w.</param>
        private void builtin_node_left_activation(BuiltinMemory memory, Token tok, WME w)
        {
            Token new_token = make_token(memory, tok, w);
            memory.Items.AddFirst(new_token);


            if (memory.PerformEvaluation(new_token))
            {
                foreach (ReteNode child in memory.Children)
                {
                    left_activation(child, new_token, w);
                }
            }
        }

        /// <summary>
        /// Perform_join_testses the specified tests.
        /// </summary>
        /// <param name="tests">The tests.</param>
        /// <param name="t">The t.</param>
        /// <param name="w">The w.</param>
        /// <returns></returns>
        private bool perform_join_tests(LinkedList<TestAtJoinNode> tests, Token t, WME w)
        {
            foreach (TestAtJoinNode this_test in tests)
            {
                Term arg1 = w[this_test.FieldOfArg1];
                WME wme2 = t[this_test.NumberOfLevelsUp];
                Term arg2 = wme2[this_test.FieldOfArg2];
                if (this_test.Evaluator.Evaluate(arg1, arg2) == false)
                    return false;
            }
            return true;
        }

        #endregion
    }
}