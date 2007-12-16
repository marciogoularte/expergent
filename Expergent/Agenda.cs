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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Expergent.Conditions;
using Expergent.Configuration;
using Expergent.ConflictResolvers;
using Expergent.Interfaces;
using Expergent.Neo;
using Expergent.Reflection;
using Expergent.Visitors;
using NSort.Generic;

namespace Expergent
{
    /// <summary>
    /// The rules execution environment
    /// </summary>
    public class Agenda
    {
        #region Fields

        private Dictionary<string, Production> _productions;
        private Rete _rete;
        private List<WME> _inferredFacts;
        private IComparer<Production> _conflictResolutionStrategy;
        private List<WME> _initialFacts;
        private int _productionCnt;
        private int _activatedRuleCount = 0;
        private int _notActivatedRuleCount = 0;
        private List<Override> _overrides;
        private List<Mutex> _mutexesNew;
        private string agendaName;
        private List<WME> _actionsTaken;
        private List<WME> _actionsSkipped;
        private bool loadRulesFromAssemblies;
        private static ExpergentOptions options;
        private Production[] _prods;
        private List<Aggregator> _aggregators;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Agenda"/> class.
        /// </summary>
        public Agenda()
        {
            _productionCnt = 1;
            _productions = new Dictionary<string, Production>();
            _inferredFacts = new List<WME>();
            _initialFacts = new List<WME>();
            _rete = new Rete();
            //_rete.OnActivation += new ActivationDelegate(OnActivationHandler);
            _overrides = new List<Override>();
            _mutexesNew = new List<Mutex>();
            _aggregators = new List<Aggregator>();
            _conflictResolutionStrategy = new SalienceResolver();
            _actionsTaken = new List<WME>();
            _actionsSkipped = new List<WME>();
            InitializeConfig();
        }

        #endregion

        private void Instance_RuleChanged(object sender, FileSystemEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #region Public Properties

        /// <summary>
        /// Gets the activated rule count.
        /// </summary>
        /// <value>The activated rule count.</value>
        public int ActivatedRuleCount
        {
            get { return _activatedRuleCount; }
        }

        /// <summary>
        /// Gets the not activated rule count.
        /// </summary>
        /// <value>The not activated rule count.</value>
        public int NotActivatedRuleCount
        {
            get { return _notActivatedRuleCount; }
        }

        //private void OnActivationHandler(object o, ActivationEvent e)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        /// <summary>
        /// Gets or sets the conflict resolution strategy.
        /// </summary>
        /// <value>The conflict resolution strategy.</value>
        public IComparer<Production> ConflictResolutionStrategy
        {
            get { return _conflictResolutionStrategy; }
            set { _conflictResolutionStrategy = value; }
        }

        /// <summary>
        /// Gets the inferred facts.
        /// </summary>
        /// <value>The inferred facts.</value>
        public List<WME> InferredFacts
        {
            get { return _inferredFacts; }
        }

        /// <summary>
        /// Gets or sets the name of the agenda.
        /// </summary>
        /// <value>The name of the agenda.</value>
        public string AgendaName
        {
            get { return agendaName; }
            set { agendaName = value; }
        }

        /// <summary>
        /// Gets the total facts count.
        /// </summary>
        /// <value>The total facts.</value>
        public int TotalFacts
        {
            get { return _initialFacts.Count; }
        }

        /// <summary>
        /// Gets the actions taken.
        /// </summary>
        /// <value>The actions taken.</value>
        public List<WME> ActionsTaken
        {
            get { return _actionsTaken; }
        }

        /// <summary>
        /// Gets the actions skipped.
        /// </summary>
        /// <value>The actions skipped.</value>
        public List<WME> ActionsSkipped
        {
            get { return _actionsSkipped; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [load rules from assemblies].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [load rules from assemblies]; otherwise, <c>false</c>.
        /// </value>
        public bool LoadRulesFromAssemblies
        {
            get { return loadRulesFromAssemblies; }
            set { loadRulesFromAssemblies = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The main method for tules evaluation.
        /// </summary>
        public void Run()
        {
            if (options.LoadRulesFromAssemblies || loadRulesFromAssemblies)
            {
                ProductionLoader.Instance.RulesDirectory = options.RuleFolder;
                ProductionLoader.Instance.RuleChanged += new FileSystemEventHandler(Instance_RuleChanged);

                foreach (IProductionProvider ruleSetProvider in ProductionLoader.Instance.RuleSets)
                {
                    foreach (Aggregator aggregator in ruleSetProvider.GetAggregators())
                    {
                        AddAggregator(aggregator);
                    }
                    foreach (Production production in ruleSetProvider.GetProductions())
                    {
                        AddProduction(production);
                    }
                    foreach (Override @override in ruleSetProvider.GetOverrides())
                    {
                        AddOverride(@override);
                    }
                    foreach (Mutex mutex in ruleSetProvider.GetMutexes())
                    {
                        AddMutex(mutex);
                    }
                }
            }

            foreach (Production prod in _productions.Values)
            {
                _rete.AddProduction(prod);
            }
            foreach (Mutex mut in _mutexesNew)
            {
                _rete.AddMutex(mut);
            }
            foreach (Aggregator ag in _aggregators)
            {
                _rete.AddAggregator(ag);
            }
            foreach (WME wme in _initialFacts)
            {
                _rete.AddWME(wme);
            }
            EvaluateOverrides();
            EvaluateAggregators();
            ResolveConflicts();
            Activate();

            ResolveMutexes();
        }

        /// <summary>
        /// Creates the fact set from an object instance.
        /// </summary>
        /// <param name="objectinstance">The object instance.</param>
        /// <param name="objectMapTable">The object map table.</param>
        /// <returns></returns>
        public List<WME> CreateFactSetFromObjectInstance(ObjectInstance objectinstance, ObjectMapTable objectMapTable)
        {
            ObjectInterface objectinterface = new ObjectInterface(AgendaName);
            Object obj = objectinstance.Object;
            return objectinterface.createFactSetFromObjectInstance(obj, objectMapTable);
        }

        /// <summary>
        /// Creates the fact set from an object instance with object grapher.
        /// </summary>
        /// <param name="objectinstance">The object instance.</param>
        /// <returns></returns>
        public List<WME> CreateFactSetFromObjectInstanceWithObjectGrapher(ObjectInstance objectinstance)
        {
            Object obj = objectinstance.Object;
            ObjectInterface objectinterface = new ObjectInterface(AgendaName);
            return objectinterface.createFactSetObjectInstanceWithObjectGrapher(obj);
        }

        /// <summary>
        /// Adds the object to the internal list of business objects.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void AddObject(IFactProvider obj)
        {
            if (obj != null)
            {
                _initialFacts.AddRange(obj.GenerateFactsForRootObject());
            }
        }

        /// <summary>
        /// Adds a collection of objects to the internal list of business objects.
        /// </summary>
        /// <param name="entityObjects">The entity objects.</param>
        public void AddObjects(IEnumerable entityObjects)
        {
            foreach (RulesEnabledEntityObject entityObject in entityObjects)
            {
                AddObject(entityObject);
            }
        }

        /// <summary>
        /// Adds the production.
        /// </summary>
        /// <param name="prod">The prod.</param>
        public void AddProduction(Production prod)
        {
            if (String.IsNullOrEmpty(prod.Label))
            {
                prod.Label = "Rule" + _productionCnt++;
            }
            _productions.Add(prod.Label, prod);
        }

        /// <summary>
        /// Adds the fact.
        /// </summary>
        /// <param name="fact">The fact to the initial facts list.</param>
        public void AddFact(WME fact)
        {
            _initialFacts.Add(fact);
        }

        /// <summary>
        /// Adds a list of facts to the initial facts list.
        /// </summary>
        /// <param name="list">The list.</param>
        public void AddFacts(IList<WME> list)
        {
            _initialFacts.AddRange(list);
        }

        /// <summary>
        /// Adds the override to the internal list.
        /// </summary>
        /// <param name="override">The override.</param>
        public void AddOverride(Override @override)
        {
            if (_productions.ContainsKey(@override.Winner) && _productions.ContainsKey(@override.Loser))
            {
                _overrides.Add(@override);
            }
            else
            {
                throw new ApplicationException("Productions must be defined prior to Override definition.");
            }
        }

        /// <summary>
        /// Adds the mutex to the internal list of mutexes.
        /// </summary>
        /// <param name="mutex">The mutex.</param>
        public void AddMutex(Mutex mutex)
        {
            _mutexesNew.Add(mutex);
        }

        /// <summary>
        /// Adds the aggregator to the internal list.
        /// </summary>
        /// <param name="aggregator">The aggregator.</param>
        public void AddAggregator(Aggregator aggregator)
        {
            _aggregators.Add(aggregator);
        }

        /// <summary>
        /// Prints the network.
        /// </summary>
        /// <param name="s">The file path.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        public void PrintNetwork(string s, bool append)
        {
            NetworkPrinter printer = new NetworkPrinter();
            _rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(s, append))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Resolves the mutexes.
        /// </summary>
        private void ResolveMutexes()
        {
            foreach (Mutex mutex in _mutexesNew)
            {
                if (mutex.Activated)
                    ApplyMutexResults(mutex);
            }
        }

        private void EvaluateAggregators()
        {
            foreach (Aggregator aggregator in _aggregators)
            {
                if (aggregator.Activated)
                    ApplyAggregatorResults(aggregator);
            }
        }

        private void ApplyAggregatorResults(Aggregator aggregator)
        {
            aggregator.Evaluate();
            DoIt(aggregator.InferredFacts);
        }

        /// <summary>
        /// Applies the mutex results.
        /// </summary>
        /// <param name="mutex">The mutex.</param>
        private void ApplyMutexResults(Mutex mutex)
        {
            mutex.Evaluate();
            DoIt(mutex.InferredFacts);
        }

        /// <summary>
        /// Evaluates the overrides.
        /// </summary>
        private void EvaluateOverrides()
        {
            foreach (Override exclusion in _overrides)
            {
                Production winner = _productions[exclusion.Winner];
                Production loser = _productions[exclusion.Loser];
                if (winner.Activated && loser.Activated)
                {
                    loser.Status = ProductionStatus.LostInOverride;
                    winner.Status = ProductionStatus.WonInOverride;
                }
            }
        }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        private void Activate()
        {
            for (int i = 0; i < _prods.Length; i++)
            {
                Production prod = _prods[i];
                if (prod.Activated)
                {
                    switch (prod.Status)
                    {
                        case ProductionStatus.LostInOverride:
                            _notActivatedRuleCount++;
                            break;
                        case ProductionStatus.WonInOverride:
                            _activatedRuleCount++;
                            DoIt(prod.InferredFacts);
                            break;
                        case ProductionStatus.Ready:
                            _activatedRuleCount++;
                            DoIt(prod.InferredFacts);
                            break;
                    }
                }
                else
                {
                    _notActivatedRuleCount++;
                }
            }
        }

        /// <summary>
        /// Does it.
        /// </summary>
        /// <param name="activations">The activations.</param>
        private void DoIt(List<Activation> activations)
        {
            foreach (Activation cond in activations)
            {
                switch (cond.ConditionType)
                {
                    case ConditionType.Assert:
                        Assert(cond.InferredFact);
                        break;
                    case ConditionType.Invoke:
                        Invoke(cond.InferredFact);
                        break;
                    case ConditionType.Set:
                        Set(cond.InferredFact);
                        break;
                    case ConditionType.Retract:
                        Retract(cond.InferredFact);
                        break;
                    default:
                        Assert(cond.InferredFact);
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the specified fact.
        /// </summary>
        /// <param name="fact">The fact.</param>
        private void Set(WME fact)
        {
            IFactProvider eo = fact.Identifier.Value as IFactProvider;
            if (eo == null)
            {
                return;
            }
            try
            {
                eo.SetProperty(fact.Attribute.Value.ToString(), fact.Value.Value);
                _actionsTaken.Add(fact);
            }
            catch
            {
                _actionsSkipped.Add(fact);
            }
        }

        /// <summary>
        /// Invokes the specified fact.
        /// </summary>
        /// <param name="fact">The fact.</param>
        private void Invoke(WME fact)
        {
            IFactProvider eo = fact.Identifier.Value as IFactProvider;
            if (eo == null)
            {
                return;
            }
            try
            {
                eo.InvokeMethod(fact.Attribute.Value.ToString(), fact.Value.Value);
                _actionsTaken.Add(fact);
            }
            catch
            {
                _actionsSkipped.Add(fact);
            }
        }

        /// <summary>
        /// Asserts the specified fact.
        /// </summary>
        /// <param name="fact">The fact.</param>
        private void Assert(WME fact)
        {
            _inferredFacts.Add(fact);
            _rete.AddWME(fact);
        }

        /// <summary>
        /// Retracts the specified fact.
        /// </summary>
        /// <param name="fact">The fact.</param>
        private void Retract(WME fact)
        {
            _inferredFacts.Remove(fact);
            _rete.RemoveWME(fact);
        }

        /// <summary>
        /// Resolves the conflicts.
        /// </summary>
        private void ResolveConflicts()
        {
            _prods = new Production[_productions.Count];
            _productions.Values.CopyTo(_prods, 0);
            FastQuickSorter<Production> sorter = new FastQuickSorter<Production>();
            sorter.Comparer = _conflictResolutionStrategy;
            sorter.Sort(_prods);
        }

        /// <summary>
        /// Initializes the config.
        /// </summary>
        private static void InitializeConfig()
        {
            InitializeConfig("Expergent");

            if (options == null)
            {
                InitializeConfig("expergent");
            }

            if (options == null)
            {
                options = new ExpergentOptions();
            }
        }

        /// <summary>
        /// Initializes the config.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        private static void InitializeConfig(string sectionName)
        {
            options = ConfigurationManager.GetSection(sectionName) as ExpergentOptions;
        }

        #endregion
    }
}