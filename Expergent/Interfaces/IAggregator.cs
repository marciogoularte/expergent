using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Interfaces
{
    public interface IAggregator
    {
        /// <summary>
        /// Gets or sets the term upon which the evaluation will be performed.
        /// </summary>
        /// <value>The match predicate.</value>
        Term MatchPredicate { get; set; }

        /// <summary>
        /// Gets or sets the alias the will be used when generating inferred facts.
        /// </summary>
        /// <remarks>Will default to the name of the aggregate function. For example 'Count'.</remarks>
        /// <value>The Alias.</value>
        StringTerm Alias { get; set; }

        /// <summary>
        /// Performs the evaluation on the list of wme's
        /// </summary>
        /// <param name="values">The list of wmes.</param>
        /// <returns>The resulting term.</returns>
        Term Evaluate(List<WME> values);
    }
}