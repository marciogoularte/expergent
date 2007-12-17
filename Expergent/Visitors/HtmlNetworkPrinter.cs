using System;
using System.Text;
using Expergent.Base;
using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.Terms;

namespace Expergent.Visitors
{
    internal class HtmlNetworkPrinter : AbstractVisitor
    {
        internal class HtmlStringBuilder
        {
            private StringBuilder _sb = new StringBuilder();

            public HtmlStringBuilder Append(string s)
            {
                _sb.Append(s);
                return this;
            }

            public HtmlStringBuilder AppendLine(string s)
            {
                _sb.Append(s).AppendLine("<br />");
                return this;
            }

            public HtmlStringBuilder AppendFormat(string s, params string[] parameters)
            {
                _sb.AppendFormat(s, parameters);
                return this;
            }

            public HtmlStringBuilder AppendLine()
            {
                _sb.AppendLine("<br />");
                return this;
            }

            public override string ToString()
            {
                return _sb.ToString();
            }
        }

        private HtmlStringBuilder _sb;
        private string _indentString = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkPrinter"/> class.
        /// </summary>
        public HtmlNetworkPrinter()
        {
            _sb = new HtmlStringBuilder();
        }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <value>The output.</value>
        public String Output
        {
            get { return _sb.ToString(); }
        }

        #region AbstractVisitor Member Overrides

        /// <summary>
        /// Called when [alpha memory] is visited.
        /// </summary>
        /// <param name="am">The am.</param>
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

        /// <summary>
        /// Called when [beta memory] is visited.
        /// </summary>
        /// <param name="beta">The beta.</param>
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

        /// <summary>
        /// Called when [condition] is visited.
        /// </summary>
        /// <param name="c">The c.</param>
        public override void OnCondition(Condition c)
        {
            _sb.AppendLine(c.ToString());
        }

        /// <summary>
        /// Called when [item in alpha memory] is visited.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void OnItemInAlphaMemory(ItemInAlphaMemory item)
        {
            _sb.AppendLine(item.ToString());
        }

        /// <summary>
        /// Called when [join node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
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

        /// <summary>
        /// Called when [builtin memory] is visited.
        /// </summary>
        /// <param name="memory">The memory.</param>
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

        /// <summary>
        /// Called when [NCC node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
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

        /// <summary>
        /// Called when [NCC partner node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public override void OnNCCPartnerNode(NCCPartnerNode node)
        {
            _indentString = string.Empty;
            _sb.Append(_indentString).AppendLine("  |<-------------/");
            _sb.Append(_indentString).AppendLine("  V");

            node.NCCNode.Accept(this);
        }

        /// <summary>
        /// Called when [negative join result] is visited.
        /// </summary>
        /// <param name="result">The result.</param>
        public override void OnNegativeJoinResult(NegativeJoinResult result)
        {
            _sb.AppendLine(result.ToString());
        }

        /// <summary>
        /// Called when [negative node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
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

        /// <summary>
        /// Called when [P node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
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

        /// <summary>
        /// Called when [aggregator] is visited.
        /// </summary>
        /// <param name="aggregator">The aggregator.</param>
        public override void OnAggregator(Aggregator aggregator)
        {
            _sb.AppendLine(aggregator.ToString());
        }

        /// <summary>
        /// Called when [aggregator node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public override void OnAggregatorNode(AggregatorNode node)
        {
            _sb.Append(_indentString).AppendLine(string.Format("Aggregator Node: {0}", node));
            _sb.Append(_indentString).AppendLine("");
            _sb.Append(_indentString).AppendLine("    ====== Inferred Facts ======");
            foreach (Activation activation in node.Aggregator.InferredFacts)
            {
                _sb.Append(_indentString).AppendLine(string.Format("    {0}", activation.InferredFact));
            }
        }

        /// <summary>
        /// Called when [mutex node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
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

        /// <summary>
        /// Called when [production] is visited.
        /// </summary>
        /// <param name="prod">The prod.</param>
        public override void OnProduction(Production prod)
        {
            _sb.AppendLine(prod.ToString());
        }

        /// <summary>
        /// Called when [mutex] is visited.
        /// </summary>
        /// <param name="mutex">The mutex.</param>
        public override void OnMutex(Mutex mutex)
        {
            _sb.AppendLine(mutex.ToString());
        }

        /// <summary>
        /// Called when [at join node] is visited.
        /// </summary>
        /// <param name="test">The test.</param>
        public override void OntestAtJoinNode(TestAtJoinNode test)
        {
            _sb.AppendLine(test.ToString());
        }

        /// <summary>
        /// Called when [token] is visited.
        /// </summary>
        /// <param name="t">The t.</param>
        public override void OnToken(Token t)
        {
            _sb.Append(_indentString).AppendLine(string.Format("  |   {0}", t.WME));
        }

        /// <summary>
        /// Called when [variable] is visited.
        /// </summary>
        /// <param name="v">The v.</param>
        public override void OnVariable(Variable v)
        {
            _sb.AppendLine(v.ToString());
        }

        /// <summary>
        /// Called when [WME] is visited.
        /// </summary>
        /// <param name="wme">The wme.</param>
        public override void OnWME(WME wme)
        {
            _sb.AppendLine(wme.ToString());
        }

        /// <summary>
        /// Called when [top node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
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

        /// <summary>
        /// Called when [top token] is visited.
        /// </summary>
        /// <param name="t">The t.</param>
        public override void OnTopToken(DummyTopToken t)
        {
            _sb.AppendLine(t.ToString());
        }

        /// <summary>
        /// Called when [term] is visited.
        /// </summary>
        /// <param name="term">The term.</param>
        public override void OnTerm(Term term)
        {
            _sb.AppendLine(term.ToString());
        }

        /// <summary>
        /// Called when [rete] is visited.
        /// </summary>
        /// <param name="rete">The rete.</param>
        public override void OnRete(Rete rete)
        {
            _sb.AppendLine(rete.ToString());
        }

        /// <summary>
        /// Called when [rete node] is visited.
        /// </summary>
        /// <param name="node">The node.</param>
        public override void OnReteNode(ReteNode node)
        {
            _sb.AppendLine(node.ToString());
        }

        /// <summary>
        /// Called when [binding pair] is visited.
        /// </summary>
        /// <param name="bindingPair">The binding pair.</param>
        public override void OnBindingPair(BindingPair bindingPair)
        {
            _sb.AppendLine(bindingPair.ToString());
        }

        /// <summary>
        /// Called when [variable substituter] is visited.
        /// </summary>
        /// <param name="substituter">The substituter.</param>
        public override void OnVariableSubstituter(VariableSubstituter substituter)
        {
            _sb.AppendLine(substituter.ToString());
        }

        /// <summary>
        /// Called when [constant substitutor] is visited.
        /// </summary>
        /// <param name="substitutor">The substitutor.</param>
        public override void OnConstantSubstitutor(ConstantSubstitutor substitutor)
        {
            _sb.AppendLine(substitutor.ToString());
        }

        #endregion
    }
}