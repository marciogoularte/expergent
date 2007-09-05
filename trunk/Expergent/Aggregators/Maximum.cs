using System;
using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Evaluates a list of values and returns the Largest value.
    /// </summary>
    public class Maximum : BaseAggregator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        public Maximum(Term matchPredicate)
            : base(matchPredicate, "Maximum")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sum"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public Maximum(Term matchPredicate, StringTerm alias)
            : base(matchPredicate, alias)
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
            double? d = null;
            foreach (WME item in values)
            {
                if (item.Attribute.Equals(MatchPredicate) && (item.Value.TermType == TermType.Double || item.Value.TermType == TermType.Integer))
                {
                    double tmpDbl = Convert.ToDouble(item.Value.Value);
                    if (d.HasValue)
                    {
                        if (d < tmpDbl)
                        {
                            d = tmpDbl;
                        }
                    }
                    else
                    {
                        d = tmpDbl;
                    }
                }
            }
            return new DoubleTerm(d);
        }

        #endregion
    }
}