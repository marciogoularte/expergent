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
using System.Text;
using Expergent.Base;
using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.Terms;

namespace Expergent.Visitors
{
    public class NetworkPrinter : AbstractVisitor
    {
        private StringBuilder _sb;
        private string _indentString = String.Empty;

        public NetworkPrinter()
        {
            _sb = new StringBuilder();
        }

        public String Output
        {
            get { return _sb.ToString(); }
        }

        #region AbstractVisitor Member Overrides

        public override void OnAlphaMemory(AlphaMemory am)
        {
            _sb.Append("").AppendLine(string.Format("Alpha: {0} - ({1})", am, string.Join(", ", am.Conditions.ToArray())));
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  |                     ====== MATCHES ======");
            foreach (ItemInAlphaMemory item in am.Items)
            {
                _sb.Append(_indentString).AppendLine(string.Format("  |                     {0}", item.WME));
            }
        }

        public override void OnBetaMemory(BetaMemory beta)
        {
            _sb.Append(_indentString).AppendLine(string.Format("Beta: {0}", beta));
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  |   ====== MATCHES ======");
            foreach (Token item in beta.Items)
            {
                item.Accept(this);
            }
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  V");
            foreach (ReteNode child in beta.Children)
            {
                child.Accept(this);
            }
        }

        public override void OnCondition(Condition c)
        {
            _sb.AppendLine(c.ToString());
        }

        public override void OnItemInAlphaMemory(ItemInAlphaMemory item)
        {
            _sb.AppendLine(item.ToString());
        }

        public override void OnJoinNode(JoinNode node)
        {
            _sb.Append(_indentString).Append("Join Node <---------- ");
            node.AlphaMemory.Accept(this);

            if (node.IsHeadOfSubNetwork)
            {
                _sb.Append("").AppendLine("  |");
                _indentString = "  |            ";
                _sb.Append("  |--------> ").AppendLine("SUBNETWORK");
                _sb.Append(_indentString).AppendLine("  |");
                _sb.Append(_indentString).AppendLine("  V");
                foreach (ReteNode child in node.Children)
                {
                    if ((child is NCCNode) == false)
                        child.Accept(this);
                }
            }
            else
            {
                _sb.Append(_indentString).AppendLine("  |");
                _sb.Append(_indentString).AppendLine("  V");
                foreach (ReteNode child in node.Children)
                {
                    child.Accept(this);
                }
            }
        }

        public override void OnBuiltinMemory(BuiltinMemory memory)
        {
            _sb.AppendLine("Builtin: " + memory.Label);
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  |   ====== Evaluations ======");
            foreach (string s in memory.Results)
            {
                _sb.Append(_indentString).AppendFormat("  |  {0}", s).AppendLine();
            }
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  V");
            foreach (ReteNode child in memory.Children)
            {
                child.Accept(this);
            }
        }

        public override void OnNCCNode(NCCNode node)
        {
            _sb.Append(_indentString).AppendLine("NCC Node");
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  |   ====== MATCHES ======");
            foreach (Token item in node.Items)
            {
                item.Accept(this);
            }
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  V");
            foreach (ReteNode child in node.Children)
            {
                child.Accept(this);
            }
        }

        public override void OnNCCPartnerNode(NCCPartnerNode node)
        {
            _indentString = string.Empty;
            _sb.Append(_indentString).AppendLine("  |<-------------/");
            _sb.Append(_indentString).AppendLine("  V");

            node.NCCNode.Accept(this);
        }

        public override void OnNegativeJoinResult(NegativeJoinResult result)
        {
            _sb.AppendLine(result.ToString());
        }

        public override void OnNegativeNode(NegativeNode node)
        {
            _sb.Append(_indentString).Append("Negative Node <------ ");
            node.AlphaMemory.Accept(this);
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  |   ====== MATCHES ======");
            foreach (Token item in node.Items)
            {
                item.Accept(this);
            }
            _sb.Append(_indentString).AppendLine("  |");
            _sb.Append(_indentString).AppendLine("  V");
            foreach (ReteNode child in node.Children)
            {
                child.Accept(this);
            }
        }

        public override void OnPNode(ProductionNode node)
        {
            _sb.Append(_indentString).AppendLine(string.Format("P-Node: {0}", node));
            _sb.Append(_indentString).AppendLine("");
            _sb.Append(_indentString).AppendLine("    ====== Inferred Facts ======");
            foreach (Activation activation in node.Production.InferredFacts)
            {
                _sb.Append(_indentString).AppendLine(string.Format("    {0}", activation.InferredFact));
            }
        }

        public override void OnMutexNode(MutexNode node)
        {
            _sb.Append(_indentString).AppendLine(string.Format("Mutex Node: {0}", node));
            _sb.Append(_indentString).AppendLine("");
            _sb.Append(_indentString).AppendLine("    ====== Inferred Facts ======");
            foreach (Activation activation in node.Mutex.InferredFacts)
            {
                _sb.Append(_indentString).AppendLine(string.Format("    {0}", activation.InferredFact));
            }
        }

        public override void OnProduction(Production prod)
        {
            _sb.AppendLine(prod.ToString());
        }

        public override void OnMutex(Mutex mutex)
        {
            _sb.AppendLine(mutex.ToString());
        }

        public override void OntestAtJoinNode(TestAtJoinNode test)
        {
            _sb.AppendLine(test.ToString());
        }

        public override void OnToken(Token t)
        {
            _sb.Append(_indentString).AppendLine(string.Format("  |   {0}", t.WME));
        }

        public override void OnVariable(Variable v)
        {
            _sb.AppendLine(v.ToString());
        }

        public override void OnWME(WME wme)
        {
            _sb.AppendLine(wme.ToString());
        }

        public override void OnTopNode(DummyTopNode node)
        {
            foreach (ReteNode child in node.Children)
            {
                _sb.Append(_indentString).AppendLine("TopNode");
                _sb.Append(_indentString).AppendLine("  |");
                _sb.Append(_indentString).AppendLine("  V");
                child.Accept(this);
                _sb.Append(_indentString).AppendLine();
                _sb.Append(_indentString).AppendLine();
                _sb.Append(_indentString).AppendLine();
            }
        }

        public override void OnTopToken(DummyTopToken t)
        {
            _sb.AppendLine(t.ToString());
        }

        public override void OnTerm(Term term)
        {
            _sb.AppendLine(term.ToString());
        }

        public override void OnRete(Rete rete)
        {
            _sb.AppendLine(rete.ToString());
        }

        public override void OnReteNode(ReteNode node)
        {
            _sb.AppendLine(node.ToString());
        }

        public override void OnBindingPair(BindingPair bindingPair)
        {
            _sb.AppendLine(bindingPair.ToString());
        }

        public override void OnVariableSubstituter(VariableSubstituter substituter)
        {
            _sb.AppendLine(substituter.ToString());
        }

        public override void OnConstantSubstitutor(ConstantSubstitutor substitutor)
        {
            _sb.AppendLine(substitutor.ToString());
        }

        #endregion
    }
}