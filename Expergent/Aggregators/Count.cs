using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Aggregators
{
    /// <summary>
    /// Counts the elements in a list of wmes
    /// </summary>
    public class Count : BaseAggregator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Count"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        public Count(Term matchPredicate) : base(matchPredicate, "Count")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Count"/> class.
        /// </summary>
        /// <param name="matchPredicate">The match predicate.</param>
        /// <param name="alias">The alias.</param>
        public Count(Term matchPredicate, StringTerm alias) : base(matchPredicate, alias)
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
            foreach (WME item in values)
            {
                if (item.Attribute.Equals(MatchPredicate))
                {
                    i++;
                }
            }
            return new IntegerTerm(i);
        }

        #endregion
    }
}