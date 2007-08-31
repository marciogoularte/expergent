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

using Expergent.Interfaces;

namespace Expergent
{
    /// <summary>
    /// The test-at-join-node structure specifies the locations of the two fields whose values must be
    /// equal in order for some variable to be bound consistently:
    /// </summary>
    public class TestAtJoinNode : IVisitable
    {
        #region Fields

        private int _field_of_arg1;
        private int _condition_number_of_arg2;
        private int _field_of_arg2;
        private int _number_of_levels_up;
        private IEvaluator _evaluator;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAtJoinNode"/> class.
        /// </summary>
        public TestAtJoinNode()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the evaluator.
        /// </summary>
        /// <value>The evaluator.</value>
        public IEvaluator Evaluator
        {
            get { return _evaluator; }
            set { _evaluator = value; }
        }

        /// <summary>
        /// Arg1 is one of the three fields in the WME (in the alpha memory).
        /// </summary>
        /// <value>The field of arg1.</value>
        public int FieldOfArg1
        {
            get { return _field_of_arg1; }
            set { _field_of_arg1 = value; }
        }

        /// <summary>
        /// with list-form tokens, it is convenient to have condition-number-of-arg2 specify the relative condition number, i.e., the number of conditions in between the one containing arg1 and the one containing arg2.
        /// </summary>
        /// <value>The condition number of arg2.</value>
        public int ConditionNumberOfArg2
        {
            get { return _condition_number_of_arg2; }
            set { _condition_number_of_arg2 = value; }
        }

        /// <summary>
        /// arg2 is a field from a WME that matched some earlier condition in the production (i.e., part of the token in the beta memory).
        /// </summary>
        /// <value>The field of arg2.</value>
        public int FieldOfArg2
        {
            get { return _field_of_arg2; }
            set { _field_of_arg2 = value; }
        }

        /// <summary>
        /// Gets or sets the number of levels up.
        /// </summary>
        /// <value>The number of levels up.</value>
        public int NumberOfLevelsUp
        {
            get { return _number_of_levels_up; }
            set { _number_of_levels_up = value; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            TestAtJoinNode test = obj as TestAtJoinNode;
            if (test == null)
            {
                return base.Equals(obj);
            }
            return test.ConditionNumberOfArg2.Equals(ConditionNumberOfArg2) && test.Evaluator.ToString().Equals(Evaluator.ToString()) && test.FieldOfArg1.Equals(FieldOfArg1) && test.FieldOfArg2.Equals(FieldOfArg2) && test.NumberOfLevelsUp.Equals(NumberOfLevelsUp);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return ConditionNumberOfArg2.GetHashCode() + _evaluator.GetHashCode() + FieldOfArg1.GetHashCode() + FieldOfArg2.GetHashCode() + NumberOfLevelsUp.GetHashCode();
        }

        #endregion

        #region IVisitable Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.OntestAtJoinNode(this);
        }

        #endregion
    }
}