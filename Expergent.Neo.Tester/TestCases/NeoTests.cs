using System;
using System.Collections.Generic;
using CRMSample.Models;
using Expergent;
using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.ConflictResolvers;
using Expergent.Terms;
using Neo.Core;
using Neo.Framework;
using NUnit.Framework;

namespace BooRulesTests.BooRulesTests
{
    [Serializable, TestFixture]
    public class NeoTests
    {
        private const string DataStore = "Neo.SqlClient.SqlDataStore, Neo";
        private const string ConnectionString = @"data source=INDEPENDENCE;initial catalog=SampleCRM;user id=sampleuser;password=sampleuser1996;persist security info=true";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void BasicConnectivityTest()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            Customer customer = factory.FindFirst("Status.Name = {0}", "Active");
            Assert.IsNotNull(customer, "Should not be null.");
        }

        [Test]
        public void FirstTest()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            Customer customer = factory.FindFirst("Status.Name = {0}", "Active");

            Assert.IsNotNull(customer, "Should not be null.");

            IList<WME> factlist = customer.GenerateFactsForRootObject();
            Assert.IsTrue(factlist.Count == 11, "Wrong count.");

            Agenda agenda = new Agenda();
            agenda.LoadRulesFromAssemblies = false;

            Variable customer_var = new Variable("Customer");
            Variable customer_name = new Variable("Customer.Name");

            Production mostSimple = new Production("Most Simple");
            mostSimple.AddConditionToLHS(new PositiveCondition(customer_var, "$Customer.Name", customer_name));
            mostSimple.AddConditionToRHS(new InvokeCondition("shout", customer_var, "Shout", customer_name));
            agenda.AddProduction(mostSimple);

            agenda.AddFacts(factlist);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\NeoFirstTest.log", false);

            Assert.IsTrue(agenda.TotalFacts == 11, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 1, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
        }

        [Test]
        public void CollectionTest()
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
            Variable orderItems = new Variable("OrderItems");
            Variable orderItem = new Variable("OrderItem");
            Variable product = new Variable("Product");
            //Variable comment = new Variable("Comment");
            Variable description = new Variable("Description");

            Production mostSimple = new Production("CollectionTest");
            mostSimple.AddConditionToLHS(new PositiveCondition(customer_var, "$Customer.Orders", orders));
            mostSimple.AddConditionToLHS(new PositiveCondition(orders, "$Customer.Orders.Order", order));
            mostSimple.AddConditionToLHS(new PositiveCondition(order, "$Customer.Orders.Order.OrderItems", orderItems));
            mostSimple.AddConditionToLHS(new PositiveCondition(orderItems, "$Customer.Orders.Order.OrderItems.OrderItem", orderItem));
            mostSimple.AddConditionToLHS(new PositiveCondition(orderItem, "$Customer.Orders.Order.OrderItems.OrderItem.Product", product));
            mostSimple.AddConditionToLHS(new PositiveCondition(product, "$Customer.Orders.Order.OrderItems.OrderItem.Product.Description", description));
            mostSimple.AddConditionToLHS(new FunctionCondition(description, new FuncTerm("funcEquals", new funcEquals()), "Troll Food"));
            mostSimple.AddConditionToRHS(new InvokeCondition("shout", customer_var, "Shout", "Yipee"));
            mostSimple.AddConditionToRHS(new AssertCondition("Wow", customer_var, "eats", description));
            agenda.AddProduction(mostSimple);

            agenda.AddFacts(factlist);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\CollectionTest.log", false);

            Assert.IsTrue(agenda.TotalFacts == 11, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 1, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Bad");
            Assert.IsTrue(customer.Result != null && customer.Result.StartsWith("Shout"), "Did not invoke method.");
        }

        [Test]
        public void Test2()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            ObjectList<Customer> customers = factory.Find("Status.Name = {0}", "Active");

            Assert.IsNotNull(customers.Count >= 2, "Should not be at least 2.");

            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new SalienceResolver();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObjects(customers);

            Production WhatsMyName = new Production("Whats My Name");
            WhatsMyName.Salience = 1;
            Variable customer_var = new Variable("Customer");
            Variable customer_name = new Variable("Name");
            WhatsMyName.AddConditionToLHS(new PositiveCondition("C2", customer_var, "$Customer.Name", customer_name));
            WhatsMyName.AddConditionToLHS(new FunctionCondition("C3", customer_name, new FuncTerm("funcEquals", new funcEquals()), "Joe Blow"));
            WhatsMyName.AddConditionToRHS(new InvokeCondition("R1", customer_var, "Shout", customer_name));
            WhatsMyName.AddConditionToRHS(new SetCondition("R2", customer_var, "Remarks", "Hello from Expergent."));
            agenda.AddProduction(WhatsMyName);

            Production WhatsMyName1 = new Production();
            WhatsMyName1.Salience = 10;
            WhatsMyName1.AddConditionToLHS(new PositiveCondition(customer_var, "$Customer.Name", customer_name));
            WhatsMyName1.AddConditionToRHS(new InvokeCondition(customer_var, "Shout", "Squid"));
            WhatsMyName1.AddConditionToRHS(new SetCondition(customer_var, "Remarks", "Squid Text"));
            agenda.AddProduction(WhatsMyName1);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\NeoTest2.log", false);

            Assert.IsTrue(agenda.TotalFacts == 22, "Bad");
            Assert.IsTrue(agenda.ActionsTaken.Count == 6, "Bad");
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0, "Bad");
            Assert.IsTrue(agenda.ActivatedRuleCount == 2, "Bad");
            Assert.IsTrue(customers.FindFirst("Name = {0}", "Joe Blow").Result != null && customers.FindFirst("Name = {0}", "Joe Blow").Result.Contains("Squid"), "Did not invoke method.");
            Assert.IsTrue(customers.FindFirst("Name = {0}", "Joe Blow").Remarks != null && customers.FindFirst("Name = {0}", "Joe Blow").Remarks.Contains("Squid Text"), "Did not invoke method.");

        }

        [Test]
        public void DigDeep()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            Customer customer = factory.FindFirst("Name = {0}", "Joe Blow");

            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new TimeStampConflictResolver();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObject(customer);

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
            agenda.AddProduction(WhatsMyName);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\DigDeep.log", false);

            Assert.IsTrue(agenda.TotalFacts == 11);
            Assert.IsTrue(agenda.InferredFacts.Count == 0);
            Assert.IsTrue(agenda.ActivatedRuleCount == 1);
            Assert.IsTrue(agenda.ActionsTaken.Count == 1);
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0);
            Assert.IsTrue(customer.Remarks.Equals("Joe Blow"), "Should be 'Default Text.'.");
        }


        [Test]
        public void TimeStampConflictResolver()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            Customer customer = factory.FindFirst("Name = {0}", "Joe Blow");
            Assert.IsNotNull(customer, "SHould have found customer.");
            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new TimeStampConflictResolver();
            agenda.LoadRulesFromAssemblies = false;
            agenda.AddObject(customer);

            Production WhatsMyName = new Production("Sets 'Default Text'.");
            WhatsMyName.Salience = 1;
            Variable customer_var = new Variable("$Customer");
            Variable customer_name = new Variable("$Customer.Name");
            WhatsMyName.AddConditionToLHS(new PositiveCondition(customer_var, "$Customer.Name", customer_name));
            WhatsMyName.AddConditionToLHS(new FunctionCondition("Joe Blow", new FuncTerm("funcEquals", new funcEquals()), customer_name));
            WhatsMyName.AddConditionToRHS(new InvokeCondition(customer_var, "Shout", customer_name));
            WhatsMyName.AddConditionToRHS(new SetCondition(customer_var, "Remarks", "Default Text."));
            agenda.AddProduction(WhatsMyName);

            Production WhatsMyName1 = new Production("Sets 'Squid Text'.");
            WhatsMyName1.Salience = 1;
            WhatsMyName1.AddConditionToLHS(new PositiveCondition(customer_var, "$Customer.Name", customer_name));
            WhatsMyName1.AddConditionToRHS(new InvokeCondition(customer_var, "Shout", "Squid"));
            WhatsMyName1.AddConditionToRHS(new SetCondition(customer_var, "Remarks", "Squid Text"));
            WhatsMyName1.AddConditionToRHS(new AssertCondition(customer_var, "Poop", "Deck"));
            agenda.AddProduction(WhatsMyName1);

            agenda.Run();

            Assert.IsTrue(agenda.TotalFacts == 11);
            Assert.IsTrue(agenda.InferredFacts.Count == 1);
            Assert.IsTrue(agenda.ActivatedRuleCount == 2);
            Assert.IsTrue(agenda.ActionsTaken.Count == 4);
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0);
            Assert.IsTrue(customer.Remarks.Equals("Squid Text"), "Should be 'Default Text.'.");
        }

        [Test]
        public void TestRuleLoader()
        {
            CustomerFactory factory = new CustomerFactory(new ObjectContext(CreateDataStore()));
            Customer customer = factory.FindFirst("Name = {0}", "Joe Blow");

            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new TimeStampConflictResolver();
            agenda.LoadRulesFromAssemblies = true;
            agenda.AddObject(customer);

            agenda.Run();

            agenda.VisualizeNetworkToFile(@"C:\Temp\TestRuleLoader.log", false);

            Assert.IsTrue(agenda.TotalFacts == 11);
            Assert.IsTrue(agenda.InferredFacts.Count == 0);
            Assert.IsTrue(agenda.ActivatedRuleCount == 1);
            Assert.IsTrue(agenda.ActionsTaken.Count == 1);
            Assert.IsTrue(agenda.ActionsSkipped.Count == 0);
            Assert.IsTrue(customer.Remarks.Equals("Joe Blow"), "Should be 'Default Text.'.");
        }

        private IDataStore CreateDataStore()
        {
            object[] objArray = new object[1] {ConnectionString};
            return ((IDataStore) (Activator.CreateInstance(Type.GetType(DataStore), objArray)));
        }
    }
}