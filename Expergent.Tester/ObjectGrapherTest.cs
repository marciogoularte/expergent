using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Expergent.Reflection;
using Expergent.Tester.TestClasses;

namespace Expergent.Tester
{
    internal class ObjectGrapherTest
    {
        [Test]
        public void ObjectMapTest()
        {
            //String[] items = {"camera", "film"};
            //String[] discounts = {"nodiscount", "nodiscount"};

            //PurchaseOrder po = new PurchaseOrder(items, discounts);
            //PurchaseOrder po1 = new PurchaseOrder(items, discounts);
            CustomerTestClass cust = new CustomerTestClass();
            CustomerTestClass cust2 = new CustomerTestClass();
            LocationTestClass theLoc = new LocationTestClass("1", "root location");

            cust.IntVal = 4;
            cust.TheLocation.Description = "first location";
            cust2.TheLocation.Description = "second location";
            cust2.TheLocation.ID = "3";

            Stream mapStream = this.GetType().Assembly.GetManifestResourceStream("Expergent.Tester.ObjectMap.xml");
            StreamReader mapReader = new StreamReader(mapStream);

            ObjectMapTable theTable = new ObjectMapTable(mapReader);

            Agenda agenda = new Agenda();
            List<WME> facts = new List<WME>();
            facts.AddRange(agenda.CreateFactSetFromObjectInstance(new ObjectInstance("CustomerTestClass", cust), theTable));
            facts.AddRange(agenda.CreateFactSetFromObjectInstance(new ObjectInstance("CustomerTestClass", cust2), theTable));
            facts.AddRange(agenda.CreateFactSetFromObjectInstance(new ObjectInstance("LocationTestClass", theLoc), theTable));
            facts.AddRange(agenda.CreateFactSetFromObjectInstance(new ObjectInstance("MyClass", this), theTable));

            Assert.IsTrue(facts.Count == 31, "Wrong # of facts.");
        }

        [Test]
        public void ObjectGraphTest()
        {
            String[] items = { "camera", "film" };
            String[] discounts = { "nodiscount", "nodiscount" };

            PurchaseOrder po = new PurchaseOrder(items, discounts);
            PurchaseOrder po1 = new PurchaseOrder(items, discounts);
            CustomerTestClass cust = new CustomerTestClass();
            CustomerTestClass cust2 = new CustomerTestClass();
            LocationTestClass theLoc = new LocationTestClass("1", "root location");

            cust.IntVal = 4;
            cust.TheLocation.Description = "first location";
            cust2.TheLocation.Description = "second location";
            cust2.TheLocation.ID = "3";

            Agenda agenda = new Agenda();
            List<WME> facts = new List<WME>();
            facts.AddRange(agenda.CreateFactSetFromObjectInstanceWithObjectGrapher(new ObjectInstance("PurchaseOrder", po)));
            facts.AddRange(agenda.CreateFactSetFromObjectInstanceWithObjectGrapher(new ObjectInstance("PurchaseOrder", po1)));
            facts.AddRange(agenda.CreateFactSetFromObjectInstanceWithObjectGrapher(new ObjectInstance("CustomerTestClass", cust)));
            facts.AddRange(agenda.CreateFactSetFromObjectInstanceWithObjectGrapher(new ObjectInstance("CustomerTestClass", cust2)));
            facts.AddRange(agenda.CreateFactSetFromObjectInstanceWithObjectGrapher(new ObjectInstance("LocationTestClass", theLoc)));
            facts.AddRange(agenda.CreateFactSetFromObjectInstanceWithObjectGrapher(new ObjectInstance("MyClass", this)));

            Assert.IsTrue(facts.Count == 1770, "Wrong # of facts.");
        }

        public void MyMethod(string theString)
        {
            Console.WriteLine(theString);
        }
    }
}