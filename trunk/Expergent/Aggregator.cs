using System;
using System.Collections.Generic;
using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent
{
    /// <summary>
    /// Performs Aggregate functions.
    /// </summary>
    public class Aggregator : IVisitable
    {
        #region Fields

        private List<LeftHandSideCondition> _lhs;
        private string _label;
        private List<Activation> _inferredFacts;
        private ProductionStatus _status;
        private AggregatorNode _aggregatorNode;
        private IAggregator _aggregator;
        private Variable _groupBy;
        private ConditionType _conditionType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregator"/> class.
        /// </summary>
        public Aggregator()
        {
            _lhs = new List<LeftHandSideCondition>();
            _inferredFacts = new List<Activation>();
            _status = ProductionStatus.Ready;
            _conditionType = ConditionType.Assert;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregator"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Aggregator(string label)
            : this()
        {
            _label = label;
        }

        #endregion

        #region Properties

        #region Public

        /// <summary>
        /// Gets or sets the type of the condition or action that will be performed upon activation.
        /// </summary>
        /// <value>The type of the condition.</value>
        public ConditionType ConditionType
        {
            get { return _conditionType; }
            set { _conditionType = value; }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string label
        {
            get { return _label; }
            set { _label = value; }
        }

        /// <summary>
        /// Gets or sets the inferred facts.
        /// </summary>
        /// <value>The inferred facts.</value>
        public List<Activation> InferredFacts
        {
            get { return _inferredFacts; }
            set { _inferredFacts = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Mutex"/> is activated.
        /// </summary>
        /// <value><c>true</c> if activated; otherwise, <c>false</c>.</value>
        public bool Activated
        {
            get { return _aggregatorNode.Items.Count > 0; }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ProductionStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Gets or sets the mutex node.
        /// </summary>
        /// <value>The mutex node.</value>
        public AggregatorNode AggregatorNode
        {
            get { return _aggregatorNode; }
            set { _aggregatorNode = value; }
        }

        /// <summary>
        /// Gets or sets the mutex evaluator.
        /// </summary>
        /// <value>The mutex evaluator.</value>
        public IAggregator AggregatorFunction
        {
            get { return _aggregator; }
            set { _aggregator = value; }
        }

        /// <summary>
        /// Gets or sets the group by.
        /// </summary>
        /// <value>The group by.</value>
        public Variable GroupBy
        {
            get { return _groupBy; }
            set { _groupBy = value; }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Gets the list of conditions on the LHS.
        /// </summary>
        /// <value>The LHS.</value>
        internal List<LeftHandSideCondition> lhs
        {
            get { return _lhs; }
        }

        #endregion

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Adds the condition to LHS.
        /// </summary>
        /// <param name="cond">The cond.</param>
        public void AddConditionToLHS(LeftHandSideCondition cond)
        {
            _lhs.Add(cond);
        }

        /// <summary>
        /// Evaluates the tokens bound to this AggregatorNode and generates the list of InferredFacts.
        /// </summary>
        internal void Evaluate()
        {
            int cntOfEarlierConditions = lhs.Count - 1;
            VariableSubstituter vs = null;

            for (int i = cntOfEarlierConditions; i >= 0; i--)
            {
                Condition earlier_cond = lhs[i];
                if (earlier_cond.ConditionType == ConditionType.Positive)
                {
                    for (int f2 = 0; f2 < 3; f2++)
                    {
                        Variable o = earlier_cond.Fields[f2] as Variable;
                        if (o != null && o.Equals(_groupBy))
                        {
                            vs = new VariableSubstituter();
                            vs.FieldNumber = f2;
                            vs.NumberOfLevelsUp = (cntOfEarlierConditions - i);
                            vs.BindingPair.Variable = o;
                            f2 = 3;
                            i = -1; //escape loop of cntOfEarlierConditions
                        }
                    }
                }
            }
            if (vs == null)
            {
                throw new ApplicationException("Bad rule");
            }

            Dictionary<Term, List<WME>> sorter = new Dictionary<Term, List<WME>>();

            foreach (Token token in _aggregatorNode.Items)
            {
                Token ptoken = token.GetTokenUp(vs.NumberOfLevelsUp);
                Term key = ptoken.WME.Fields[vs.FieldNumber];

                if (sorter.ContainsKey(key))
                {
                    List<WME> list = sorter[key];
                    if (list.Contains(token.WME) == false)
                        list.Add(token.WME);
                }
                else
                {
                    List<WME> list = new List<WME>();
                    list.Add(token.WME);
                    sorter.Add(key, list);
                }
            }

            foreach (KeyValuePair<Term, List<WME>> pair in sorter)
            {
                Term val = _aggregator.Evaluate(pair.Value);
                _inferredFacts.Add(new Activation(new WME(pair.Key, _aggregator.Alias, val), _conditionType));
            }
        }

        #endregion

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OnAggregator(this);
        }

        #endregion
    }
}