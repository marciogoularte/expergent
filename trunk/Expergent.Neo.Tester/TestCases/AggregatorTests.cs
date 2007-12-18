using System;
using System.Collections.Generic;
using CRMSample.Models;
using Expergent.Aggregators;
using Expergent.Authoring;
using Expergent.Terms;
using Neo.Core;
using Neo.Framework;
using NUnit.Framework;

namespace Expergent.Neo.Tester.TestCases
{
    [TestFixture]
    public class AggregatorTests
    {
        private const string DataStore = "Neo.SqlClient.SqlDataStore, Neo";
        private const string ConnectionString = @"data source=INDEPENDENCE;initial catalog=SampleCRM;user id=sampleuser;password=sampleuser1996;persist security info=true";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void FirstAggTest()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            Customer customer = factory.FindFirst("Name = {0}", "Joe Blow");

            IList<WME> factlist = customer.GenerateFactsForRootObject();
            Assert.IsTrue(factlist.Count == 11, "Wrong count.");

            Agenda agenda = new Agenda();
            agenda.LoadRulesFromAssemblies = false;

            Variable customer_var = new Variable("Customer");
            Variable orders = new Variable("Orders");
            Variable order = new Variable("Order");
            Variable val = new Variable("?Value");

            Aggregator ag = new Aggregator("Count of customer orders.");
            ag.GroupBy = customer_var;
            ag.AggregatorFunction = new Count("$Customer.Orders.Order", "$Customer.Orders.Count");
            ag.AddConditionToLHS(new AND(customer_var, "$Customer.Orders", orders));
            ag.AddConditionToLHS(new AND(orders, "$Customer.Orders.Order", order));
            agenda.AddAggregator(ag);

            Production mostSimple = new Production("If Big Spender");
            mostSimple.AddConditionToLHS(new AND(customer_var, "$Customer.Orders.Count", val));
            mostSimple.AddConditionToRHS(new INVOKE("shout", customer_var, "Shout", val));
            agenda.AddProduction(mostSimple);

            agenda.AddFacts(factlist);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\FirstAggTest.log", false);

            Assert.IsTrue(agenda.TotalFacts == 11, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 1, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
        }

        [Test]
        public void SecondAggTest()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            ObjectList<Customer> customers = factory.FindAllObjects();

            Agenda agenda = new Agenda();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObjects(customers);

            Variable customer_var = new Variable("Customer");
            Variable orders = new Variable("Orders");
            Variable order = new Variable("Order");
            Variable count = new Variable("OrderCount");

            Aggregator ag = new Aggregator("Count of customer orders.");
            ag.GroupBy = customer_var;
            ag.AggregatorFunction = new Count("$Customer.Orders.Order", "Customer.Orders.Count");
            ag.AddConditionToLHS(new AND(customer_var, "$Customer.Orders", orders));
            ag.AddConditionToLHS(new AND(orders, "$Customer.Orders.Order", order));
            agenda.AddAggregator(ag);

            Production mostSimple = new Production("If Big Spender");
            mostSimple.AddConditionToLHS(new AND(customer_var, "Customer.Orders.Count", count));
            mostSimple.AddConditionToRHS(new INVOKE("shout", customer_var, "Shout", count));
            agenda.AddProduction(mostSimple);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\SecondAggTest.log", false);

            Assert.IsTrue(agenda.TotalFacts == 22, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 2, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            foreach (Customer customer in customers)
            {
                Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
            }
        }

        [Test]
        public void TestOfSum()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            ObjectList<Customer> customers = factory.FindAllObjects();

            Agenda agenda = new Agenda();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObjects(customers);

            Variable customer_var = new Variable("Customer");
            Variable orders = new Variable("Orders");
            Variable order = new Variable("Order");
            Variable orderamount = new Variable("OrderAmount");
            Variable sum = new Variable("OrderAmountSum");

            Aggregator ag = new Aggregator("Sum of customer order OrderAmount.");
            ag.GroupBy = customer_var;
            ag.AggregatorFunction = new Sum("$Customer.Orders.Order.OrderAmount");
            ag.AddConditionToLHS(new AND(customer_var, "$Customer.Orders", orders));
            ag.AddConditionToLHS(new AND(orders, "$Customer.Orders.Order", order));
            ag.AddConditionToLHS(new AND(order, "$Customer.Orders.Order.OrderAmount", orderamount));
            agenda.AddAggregator(ag);

            Production mostSimple = new Production("If Big Spender");
            mostSimple.AddConditionToLHS(new AND(customer_var, "Sum", sum));
            mostSimple.AddConditionToRHS(new INVOKE("shout", customer_var, "Shout", sum));
            agenda.AddProduction(mostSimple);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\TestOfSum.log", false);

            Assert.IsTrue(agenda.TotalFacts == 22, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 2, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            foreach (Customer customer in customers)
            {
                Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
            }
        }

        [Test]
        public void TestOfAverage()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            ObjectList<Customer> customers = factory.FindAllObjects();

            Agenda agenda = new Agenda();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObjects(customers);

            Variable customer_var = new Variable("Customer");
            Variable orders = new Variable("Orders");
            Variable order = new Variable("Order");
            Variable orderamount = new Variable("OrderAmount");
            Variable avg = new Variable("OrderAmountAvg");

            Aggregator ag = new Aggregator("Average of customer order OrderAmount.");
            ag.GroupBy = customer_var;
            ag.AggregatorFunction = new Average("$Customer.Orders.Order.OrderAmount");
            ag.AddConditionToLHS(new AND(customer_var, "$Customer.Orders", orders));
            ag.AddConditionToLHS(new AND(orders, "$Customer.Orders.Order", order));
            ag.AddConditionToLHS(new AND(order, "$Customer.Orders.Order.OrderAmount", orderamount));
            agenda.AddAggregator(ag);

            Production mostSimple = new Production("If Big Spender");
            mostSimple.AddConditionToLHS(new AND(customer_var, "Average", avg));
            mostSimple.AddConditionToRHS(new INVOKE("shout", customer_var, "Shout", avg));
            agenda.AddProduction(mostSimple);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\TestOfAverage.log", false);

            Assert.IsTrue(agenda.TotalFacts == 22, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 2, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            foreach (Customer customer in customers)
            {
                Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
            }
        }

        [Test]
        public void TestOfMin()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            ObjectList<Customer> customers = factory.FindAllObjects();

            Agenda agenda = new Agenda();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObjects(customers);

            Variable customer_var = new Variable("Customer");
            Variable orders = new Variable("Orders");
            Variable order = new Variable("Order");
            Variable orderamount = new Variable("OrderAmount");
            Variable min = new Variable("OrderAmountMinimum");

            Aggregator ag = new Aggregator("Minimum of customer order OrderAmount.");
            ag.GroupBy = customer_var;
            ag.AggregatorFunction = new Minimum("$Customer.Orders.Order.OrderAmount");
            ag.AddConditionToLHS(new AND(customer_var, "$Customer.Orders", orders));
            ag.AddConditionToLHS(new AND(orders, "$Customer.Orders.Order", order));
            ag.AddConditionToLHS(new AND(order, "$Customer.Orders.Order.OrderAmount", orderamount));
            agenda.AddAggregator(ag);

            Production mostSimple = new Production("If Big Spender");
            mostSimple.AddConditionToLHS(new AND(customer_var, "Minimum", min));
            mostSimple.AddConditionToRHS(new INVOKE("shout", customer_var, "Shout", min));
            agenda.AddProduction(mostSimple);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\TestOfMin.log", false);

            Assert.IsTrue(agenda.TotalFacts == 22, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 2, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            foreach (Customer customer in customers)
            {
                Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
            }
        }

        [Test]
        public void TestOfMax()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            ObjectList<Customer> customers = factory.FindAllObjects();

            Agenda agenda = new Agenda();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObjects(customers);

            Variable customer_var = new Variable("Customer");
            Variable orders = new Variable("Orders");
            Variable order = new Variable("Order");
            Variable orderamount = new Variable("OrderAmount");
            Variable max = new Variable("OrderAmountMaximum");

            Aggregator ag = new Aggregator("Maximum of customer order OrderAmount.");
            ag.GroupBy = customer_var;
            ag.AggregatorFunction = new Maximum("$Customer.Orders.Order.OrderAmount");
            ag.AddConditionToLHS(new AND(customer_var, "$Customer.Orders", orders));
            ag.AddConditionToLHS(new AND(orders, "$Customer.Orders.Order", order));
            ag.AddConditionToLHS(new AND(order, "$Customer.Orders.Order.OrderAmount", orderamount));
            agenda.AddAggregator(ag);

            Production mostSimple = new Production("If Big Spender");
            mostSimple.AddConditionToLHS(new AND(customer_var, "Maximum", max));
            mostSimple.AddConditionToRHS(new INVOKE("shout", customer_var, "Shout", max));
            agenda.AddProduction(mostSimple);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\TestOfMax.log", false);

            Assert.IsTrue(agenda.TotalFacts == 22, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 2, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            foreach (Customer customer in customers)
            {
                Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
            }
        }

        private IDataStore CreateDataStore()
        {
            object[] objArray = new object[1] {ConnectionString};
            return ((IDataStore) (Activator.CreateInstance(Type.GetType(DataStore), objArray)));
        }
    }
}