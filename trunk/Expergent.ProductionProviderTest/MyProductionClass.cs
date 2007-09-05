using System;
using System.Collections.Generic;
using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.Interfaces;
using Expergent.Terms;

namespace Expergent.ProductionProviderTest
{
    public class MyProductionClass : IProductionProvider
    {
        private readonly DateTime _effectiveDate;
        private readonly DateTime _terminationDate;
        private readonly string _label;

        public MyProductionClass()
        {
            _effectiveDate = Convert.ToDateTime("12/12/1997");
            _terminationDate = Convert.ToDateTime("12/12/2020");
            _label = "My Production";
        }

        #region IProductionProvider Members

        public DateTime EffectiveDate
        {
            get { return _effectiveDate; }
        }

        public DateTime TerminationDate
        {
            get { return _terminationDate; }
        }

        public string Label
        {
            get { return _label; }
        }

        #endregion

        #region IProductionProvider Members

        public List<Production> GetProductions()
        {
            List<Production> list = new List<Production>();

            Production WhatsMyName = new Production("WhatsMyName");
            WhatsMyName.Salience = 1;
            Variable customer_var = new Variable("Customer");
            Variable customer_name = new Variable("Customer.Name");
            Variable site_status = new Variable("Customer.Status");
            Variable site_status_name = new Variable("Customer.Status.Name");

            WhatsMyName.AddConditionToLHS(new PositiveCondition("C1", customer_var, "$Customer.Name", customer_name));
            WhatsMyName.AddConditionToLHS(new FunctionCondition("F1", customer_name, new FuncTerm("funcEquals", new funcEquals()), "Joe Blow"));
            WhatsMyName.AddConditionToLHS(new PositiveCondition("C2", customer_var, "$Customer.Status", site_status));
            WhatsMyName.AddConditionToLHS(new PositiveCondition("C3", site_status, "$Customer.Status.Name", site_status_name));
            WhatsMyName.AddConditionToLHS(new FunctionCondition("F2", site_status_name, new FuncTerm("funcEquals", new funcEquals()), "Active"));
            WhatsMyName.AddConditionToRHS(new SetCondition(customer_var, "Remarks", customer_name));

            list.Add(WhatsMyName);

            return list;
        }

        public List<Override> GetOverrides()
        {
            List<Override> list = new List<Override>();

            return list;
        }

        public List<Mutex> GetMutexes()
        {
            List<Mutex> list = new List<Mutex>();

            return list;
        }

        public List<Aggregator> GetAggregators()
        {
            List<Aggregator> list = new List<Aggregator>();

            return list;
        }

        #endregion
    }
}