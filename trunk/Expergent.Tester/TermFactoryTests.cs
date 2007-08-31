using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Expergent.Terms;

namespace Expergent.Tester
{
    public class TermFactoryTests
    {
        [Test]
        public void BasicTest()
        {
            List<Type> types = new List<Type>();
            ArrayList elements = new ArrayList();

            ArrayList testCollection = new ArrayList(new string[]{"One", "Two"});
            Customer cust = new Customer();
            cust.Label = "Test";

            elements.Add("EL1"); //string
            elements.Add(Char.Parse("A"));
            elements.Add(true);
            elements.Add(Convert.ToDouble(10));
            elements.Add(Convert.ToSingle(10));
            elements.Add(Convert.ToDecimal(10));
            elements.Add(Convert.ToInt16(10));
            elements.Add(Convert.ToInt32(10));
            elements.Add(Convert.ToInt64(10));
            elements.Add(DateTime.Now);
            elements.Add(testCollection);
            elements.Add(new GenericListTerm<string>(new string[] { "One", "Two" }));
            elements.Add(cust);

            types.Add(typeof(StringTerm));
            types.Add(typeof(StringTerm));
            types.Add(typeof(BooleanTerm));
            types.Add(typeof(DoubleTerm));
            types.Add(typeof(DoubleTerm));
            types.Add(typeof(DoubleTerm));
            types.Add(typeof(IntegerTerm));
            types.Add(typeof(IntegerTerm));
            types.Add(typeof(IntegerTerm));
            types.Add(typeof(DateTimeTerm));
            types.Add(typeof(ListTerm));
            types.Add(typeof(GenericListTerm<string>));
            types.Add(typeof(ObjectTerm));

            Assert.AreEqual(elements.Count, types.Count, "Bad Test Monkey");

            for (int i = 0; i < elements.Count; i++)
            {
                Term term = TermFactory.Instance.Create(elements[i]);
                Assert.IsTrue(term.GetType() == types[i], "Wrong Term Type");
            }

        }
    }
}
