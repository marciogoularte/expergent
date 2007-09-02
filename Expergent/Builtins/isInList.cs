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

using System.Collections;
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent.Builtins
{
    ///<summary>Is In List
    ///</summary>
    public class isInList : IBuiltIn
    {
        #region IBuiltIn Members

        /// <summary>
        /// Evaluates the specified Terms.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>bool</returns>
        public bool Evaluate(Term subject, Term obj)
        {
            IEnumerable iterator = obj.Value as IEnumerable;
            if (iterator != null)
            {
                foreach (object o in iterator)
                {
                    if (subject.Value.Equals(o))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return "isInList";
        }
    }
}