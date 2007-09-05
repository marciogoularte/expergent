using System.Collections.Generic;
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Base class for all Aggregators
    /// </summary>
    public abstract class BaseAggregator : IAggregator
    {
        #region Fields

        private StringTerm _alias;
        private Term _matchPredicate;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Count"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public BaseAggregator(Term matchPredicate, StringTerm alias)
        {
            _matchPredicate = matchPredicate;
            _alias = alias;
        }

        #endregion

        #region IAggregator Members

        /// <summary>
        /// Gets or sets the term upon which the evaluation will be performed.
        /// </summary>
        /// <value>The match predicate.</value>
        public Term MatchPredicate
        {
            get { return _matchPredicate; }
            set { _matchPredicate = value; }
        }

        /// <summary>
        /// Gets or sets the alias the will be used when generating inferred facts.
        /// </summary>
        /// <value>The Alias.</value>
        /// <remarks>Will default to the name of the aggregate function. For example 'Count'.</remarks>
        public StringTerm Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        #region Abstract

        /// <summary>
        /// Evaluates the specified list of wmes.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The resulting term.</returns>
        public abstract Term Evaluate(List<WME> values);

        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _alias.Value;
        }

        #endregion
    }
}