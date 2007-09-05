using System;
using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Evaluates a sum of numeric values
    /// </summary>
    public class Sum : BaseAggregator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        public Sum(Term matchPredicate) : base(matchPredicate, "Sum")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public Sum(Term matchPredicate, StringTerm alias) : base(matchPredicate, alias)
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
            double d = 0;
            foreach (WME item in values)
            {
                if (item.Attribute.Equals(MatchPredicate) && (item.Value.TermType == TermType.Double || item.Value.TermType == TermType.Integer))
                {
                    d += Convert.ToDouble(item.Value.Value);
                }
            }
            return new DoubleTerm(d);
        }

        #endregion
    }
}