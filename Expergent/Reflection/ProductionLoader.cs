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
using System.IO;
using System.Reflection;
using Expergent.Interfaces;

namespace Expergent.Reflection
{
    internal class ProductionLoader
    {
        private FileSystemWatcher ruleFolderWatcher;
        private String viewRootDir;
        private List<Assembly> assemblies;
        private event FileSystemEventHandler ViewChangedImpl;
        private List<Type> registeredTypes;
        private static readonly ProductionLoader instance = new ProductionLoader();
        private List<IProductionProvider> _ruleSets;

        static ProductionLoader()
        {
        }

        private ProductionLoader()
        {
            registeredTypes = new List<Type>();
        }

        public static ProductionLoader Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets/sets the root directory of views, 
        /// obtained from the configuration.
        /// </summary>
        public String ViewRootDir
        {
            get { return viewRootDir; }
            set { viewRootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value); }
        }

        //public List<Assembly> Assemblies
        //{
        //    get
        //    {
        //        if (assemblies != null)
        //            return assemblies;
        //        LoadAssemblies();
        //        return assemblies;
        //    }
        //}

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

        private List<IProductionProvider> LoadRuleSetsFromAssemblies()
        {
            List<IProductionProvider> lst = new List<IProductionProvider>();
            if (assemblies == null)
                LoadAssemblies();
            foreach (Assembly c in assemblies)
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
                if ((t.IsClass) && (t.IsAbstract == false) && (t.FindInterfaces(filter, null).Length > 0) && (registeredTypes.Contains(t) == false))
                {
                    registeredTypes.Add(t);
                    lst.Add((IProductionProvider) Activator.CreateInstance(t));
                }
            }
            return lst;
        }

        /// <summary>
        /// Determines whether the supplied type is a concrete subclass of <c>IEntityMap</c>
        /// </summary>
        /// <param name="typeObj">Class to be checked</param>
        /// <param name="criteriaObj">Not used</param>
        /// <returns><c>true</c> if this is a concrete class and a subclass of <c>IEntityMap</c></returns>
        public bool IsIRuleSetProvider(Type typeObj, Object criteriaObj)
        {
            return (typeObj.IsInterface) && (typeObj == typeof (IProductionProvider));
        }

        private void LoadAssemblies()
        {
            List<Assembly> lst = new List<Assembly>();
            DirectoryInfo ruleDir = new DirectoryInfo(viewRootDir);
            if (ruleDir.Exists)
            {
                foreach (FileInfo c in ruleDir.GetFiles("*.dll"))
                {
                    try
                    {
                        lst.Add(Assembly.LoadFile(c.FullName));
                    }
                    catch //(Exception e)
                    {
                        //Logging.Instance.TheLogger.Info(string.Format("Skipping {0} in rule loader.", c.FullName), e);
                    }
                }
            }
            assemblies = lst;
        }

        public event FileSystemEventHandler RuleChanged
        {
            add
            {
                //avoid concurrency problems with creating/removing the watcher
                //in two threads in parallel. Unlikely, but better to be safe.
                lock (this)
                {
                    //create the watcher if it doesn't exists
                    if (ruleFolderWatcher == null)
                    {
                        InitViewFolderWatch();
                    }
                    ViewChangedImpl += value;
                }
            }
            remove
            {
                //avoid concurrency problems with creating/removing the watcher
                //in two threads in parallel. Unlikely, but better to be safe.
                lock (this)
                {
                    ViewChangedImpl -= value;
                    if (ViewChangedImpl == null) //no more subscribers.
                    {
                        DisposeViewFolderWatch();
                    }
                }
            }
        }

        private void DisposeViewFolderWatch()
        {
            ViewChangedImpl -= new FileSystemEventHandler(ruleFolderWatcher_Changed);
            ruleFolderWatcher.Dispose();
        }

        private void InitViewFolderWatch()
        {
            ruleFolderWatcher = new FileSystemWatcher(ViewRootDir);
            ruleFolderWatcher.IncludeSubdirectories = true;
            ruleFolderWatcher.Changed += new FileSystemEventHandler(ruleFolderWatcher_Changed);
            ruleFolderWatcher.Created += new FileSystemEventHandler(ruleFolderWatcher_Changed);
            ruleFolderWatcher.Deleted += new FileSystemEventHandler(ruleFolderWatcher_Changed);
            ruleFolderWatcher.EnableRaisingEvents = true;
        }

        private void ruleFolderWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (ViewChangedImpl != null)
            {
                ViewChangedImpl(this, e);
            }
        }
    }
}