using System;
using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Evaluates a Average of numeric values
    /// </summary>
    public class Average : BaseAggregator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        public Average(Term matchPredicate) : base(matchPredicate, "Average")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public Average(Term matchPredicate, StringTerm alias) : base(matchPredicate, alias)
        {
        }

        #endregion

        #region BaseAggregator Implimentation

        /// <summary>
        /// Evaluates the specified list of wmes.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The resulting term.</returns>
        public override Term Evaluate(List<WME> values)
        {
            int i = 0;
            double sum = 0;
            foreach (WME item in values)
            {
                if (item.Attribute.Equals(MatchPredicate) && (item.Value.TermType == TermType.Double || item.Value.TermType == TermType.Integer))
                {
                    sum += Convert.ToDouble(item.Value.Value);
                    i++;
                }
            }
            if (i == 0)
            {
                return new DoubleTerm(0.0);
            }
            return new DoubleTerm(sum/i);
        }

        #endregion
    }
}