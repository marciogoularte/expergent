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

namespace Expergent.Configuration
{
    ///<summary>Wrapper for Expergent Options
    ///</summary>
    public class ExpergentOptions
    {
        private string ruleFolder;
        private bool loadRulesFromAssemblies;

        /// <summary>
        /// Gets or sets the rule folder.
        /// </summary>
        /// <value>The rule folder.</value>
        public string RuleFolder
        {
            get { return ruleFolder; }
            set { ruleFolder = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to load the rules from assemblies.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [load rules from assemblies]; otherwise, <c>false</c>.
        /// </value>
        public bool LoadRulesFromAssemblies
        {
            get { return loadRulesFromAssemblies; }
            set { loadRulesFromAssemblies = value; }
        }
    }
}