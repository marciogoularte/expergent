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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Expergent.Interfaces;

namespace Expergent.Reflection
{
    /// <summary>
    /// Loads compiled production assemblies
    /// </summary>
    internal class ProductionLoader
    {
        #region Fields

        private FileSystemWatcher _ruleFolderWatcher;
        private String _rulesDirectory;
        private List<Assembly> _assemblies;
        private event FileSystemEventHandler _rulesChangedImpl;
        private List<Type> _registeredTypes;
        private static readonly ProductionLoader _instance = new ProductionLoader();
        private List<IProductionProvider> _ruleSets;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ProductionLoader"/> class.
        /// </summary>
        static ProductionLoader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionLoader"/> class.
        /// </summary>
        private ProductionLoader()
        {
            _registeredTypes = new List<Type>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the ProductionLoader Singleton Instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ProductionLoader Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets or sets the Rules Directory, 
        /// obtained from the configuration.
        /// </summary>
        public String RulesDirectory
        {
            get { return _rulesDirectory; }
            set { _rulesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value); }
        }

        /// <summary>
        /// Gets the rule sets.
        /// </summary>
        /// <value>The rule sets.</value>
        public List<IProductionProvider> RuleSets
        {
            get
            {
                if (_ruleSets == null)
                {
                    _ruleSets = LoadRuleSetsFromAssemblies();
                }
                return _ruleSets;
            }
        }

        #endregion

        #region Events
        
        /// <summary>
        /// When any change is made to the rules folder, this event is raised
        /// </summary>
        public event FileSystemEventHandler RuleChanged
        {
            add
            {
                lock (this)
                {
                    if (_ruleFolderWatcher == null)
                    {
                        InitViewFolderWatch();
                    }
                    _rulesChangedImpl += value;
                }
            }
            remove
            {
                lock (this)
                {
                    _rulesChangedImpl -= value;
                    if (_rulesChangedImpl == null)
                    {
                        DisposeViewFolderWatch();
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private List<IProductionProvider> LoadRuleSetsFromAssemblies()
        {
            List<IProductionProvider> lst = new List<IProductionProvider>();
            if (_assemblies == null)
                LoadAssemblies();
            foreach (Assembly c in _assemblies)
            {
                lst.AddRange(LoadRuleSetFromAssembly(c));
            }
            return lst;
        }

        private List<IProductionProvider> LoadRuleSetFromAssembly(Assembly c)
        {
            List<IProductionProvider> lst = new List<IProductionProvider>();
            TypeFilter filter = new TypeFilter(IsIRuleSetProvider);
            foreach (Type t in c.GetTypes())
            {
                if ((t.IsClass) && (t.IsAbstract == false) && (t.FindInterfaces(filter, null).Length > 0) && (_registeredTypes.Contains(t) == false))
                {
                    _registeredTypes.Add(t);
                    lst.Add((IProductionProvider) Activator.CreateInstance(t));
                }
            }
            return lst;
        }

        /// <summary>
        /// Determines whether the supplied type is a concrete subclass of <c>IProductionProvider</c>
        /// </summary>
        /// <param name="typeObj">Class to be checked</param>
        /// <param name="criteriaObj">Not used</param>
        /// <returns><c>true</c> if this is a concrete class and a subclass of <c>IProductionProvider</c></returns>
        private bool IsIRuleSetProvider(Type typeObj, Object criteriaObj)
        {
            return (typeObj.IsInterface) && (typeObj == typeof (IProductionProvider));
        }

        private void LoadAssemblies()
        {
            List<Assembly> lst = new List<Assembly>();
            DirectoryInfo ruleDir = new DirectoryInfo(_rulesDirectory);
            if (ruleDir.Exists)
            {
                foreach (FileInfo c in ruleDir.GetFiles("*.dll"))
                {
                    try
                    {
                        lst.Add(Assembly.LoadFile(c.FullName));
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(string.Format("Skipping {0} in rule loader.\n{1}", c.FullName, e));
                    }
                }
            }
            _assemblies = lst;
        }

        private void DisposeViewFolderWatch()
        {
            _rulesChangedImpl -= new FileSystemEventHandler(ruleFolderWatcher_Changed);
            _ruleFolderWatcher.Dispose();
        }

        private void InitViewFolderWatch()
        {
            _ruleFolderWatcher = new FileSystemWatcher(RulesDirectory);
            _ruleFolderWatcher.IncludeSubdirectories = true;
            _ruleFolderWatcher.Changed += new FileSystemEventHandler(ruleFolderWatcher_Changed);
            _ruleFolderWatcher.Created += new FileSystemEventHandler(ruleFolderWatcher_Changed);
            _ruleFolderWatcher.Deleted += new FileSystemEventHandler(ruleFolderWatcher_Changed);
            _ruleFolderWatcher.EnableRaisingEvents = true;
        }

        private void ruleFolderWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //force the productions to reload
            _ruleSets = null;
            _assemblies = null;

            // tell any other subscribers
            if (_rulesChangedImpl != null)
            {
                _rulesChangedImpl(this, e);
            }
        }

        #endregion
    }
}