using System;
using System.Collections.Generic;
using System.Text;
using Expergent.Interfaces;

namespace Expergent
{
    public class AggregatorNode: ReteNode
    {
        #region Private Fields

        private LinkedList<Token> _items;
        private List<WME> _inferredFacts;
        private Aggregator _aggregator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MutexNode"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public AggregatorNode(string label)
        {
            _label = label;
            _type = ReteNodeType.Aggregator;
            _items = new LinkedList<Token>();
            _inferredFacts = new List<WME>();
        }

        #endregion

        #region Public Methods

        #region Overrides

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _label;
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public virtual LinkedList<Token> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the inferred facts.
        /// </summary>
        /// <value>The inferred facts.</value>
        public List<WME> InferredFacts
        {
            get { return _inferredFacts; }
        }

        /// <summary>
        /// Gets or sets the mutex.
        /// </summary>
        /// <value>The mutex.</value>
        public Aggregator Aggregator
        {
            get { return _aggregator; }
            set { _aggregator = value; }
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IVisitor visitor)
        {
            visitor.OnAggregatorNode(this);
        }

        #endregion
    }
}