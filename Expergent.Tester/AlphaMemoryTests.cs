using System.Collections.Generic;
using System.IO;
using Expergent.Builtins;
using Expergent.Conditions;
using Expergent.Evaluators;
using Expergent.Terms;
using Expergent.Visitors;
using NUnit.Framework;

namespace Expergent.Tester
{
    public class AlphaMemoryTests
    {
        [Test]
        public void TestAddWME()
        {
            WME wme = new WME();
            wme.Fields[0] = "x";
            wme.Fields[1] = "y";
            wme.Fields[2] = "z";

            Rete am = new Rete();
            am.AddWME(wme);
        }


        /// <summary>
        /// This production matches if there is a stack of (at least) two blocks (designated by <x> and
        /// <y>) to the left of some block (designated by <z>) which is not both red and on some other
        /// block. Note that the occurrences of <z> inside the NCC refer to the binding of <z> from outside
        /// the NCC (in C2), while <w> is simply a new variable inside the NCC, since it does not occur
        /// anywhere in the (positive) conditions outside the NCC.
        /// </summary>
        [Test]
        public void NCCConditionTest()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            Variable w = new Variable("w");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));

            NCCCondition neg = new NCCCondition("C3", z, "color", "red");
            neg.ConditionType = ConditionType.NCC;
            //neg.IsPositive = false;
            //neg.IsNegative = false;
            neg.SubConditions.Add(new PositiveCondition("C4", z, "on", w));
            prod.AddConditionToLHS(neg);

            AssertCondition rhs = new AssertCondition("C4", x, "is", "on top");
            rhs.ConditionType = ConditionType.Assert;

            prod.AddConditionToRHS(rhs);

            Rete rete = new Rete();
            rete.AddProduction(prod);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            Assert.IsTrue(prod.InferredFacts.Count == 1, "Wrong number of conclusions");
            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\NCCTest.log", false))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }
            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");
        }

        /// <summary>
        /// This production matches if there is a stack of (at least) two blocks (designated by <x> and <y>)
        /// to the left of some block (designated by <z>) which is not known to be red.
        /// </summary>
        [Test]
        public void NegativeConditionTest()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            //Variable c = new Variable("c");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            //prod.AddConditionToLHS(new condition("C3", z, "color", c));

            NegativeCondition neg = new NegativeCondition("C3", z, "color", "blue");
            neg.ConditionType = ConditionType.Negative;
            //neg.IsPositive = false;
            //neg.IsNegative = true;
            prod.AddConditionToLHS(neg);


            AssertCondition rhs = new AssertCondition("C4", x, "is", "on top");
            rhs.ConditionType = ConditionType.Assert;

            prod.AddConditionToRHS(rhs);

            Rete rete = new Rete();
            rete.AddProduction(prod);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            //WME wme10 = new WME("W10");
            //wme10.fields[0] = "B4";
            //wme10.fields[1] = "color";
            //wme10.fields[2] = "green";
            //rete.add_wme(wme10);

            //rete_node dummy = rete.DummyTopNode;

            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\NegativeConditionTest.log", false))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }

            //Assert.IsTrue(rete.RulesThatFired.Count == 1, "Rule did not fire.");
            Assert.IsTrue(prod.InferredFacts.Count == 1, "Wrong number of conclusions");
            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");
        }

        [Test]
        public void GrandParents()
        {
            Rete rete = new Rete();

            Production prod = new Production();
            prod.Label = "Whose your daddy";

            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            prod.AddConditionToLHS(new PositiveCondition(x, "parent", y));
            prod.AddConditionToRHS(new AssertCondition(y, "child", x));

            rete.AddProduction(prod);

            Production prod1 = new Production();
            prod1.Label = "Whose your grand daddy";

            prod1.AddConditionToLHS(new PositiveCondition(x, "parent", y));
            prod1.AddConditionToLHS(new PositiveCondition(y, "parent", z));
            prod1.AddConditionToRHS(new AssertCondition(x, "gparent", z));

            rete.AddProduction(prod1);

            Production prod2 = new Production();
            prod2.Label = "Whose your grand child";

            prod2.AddConditionToLHS(new PositiveCondition(x, "gparent", y));
            prod2.AddConditionToRHS(new AssertCondition(y, "gchild", x));

            rete.AddProduction(prod2);

            Production prod3 = new Production();
            prod3.Label = "Whose your great grand daddy";

            prod3.AddConditionToLHS(new PositiveCondition(x, "parent", y));
            prod3.AddConditionToLHS(new PositiveCondition(y, "gparent", z));
            prod3.AddConditionToRHS(new AssertCondition(x, "ggparent", z));

            rete.AddProduction(prod3);

            List<WME> wmes = new List<WME>();

            wmes.Add(new WME("george", "gchild", "kendall"));
            wmes.Add(new WME("kendall", "parent", "immanuel"));
            wmes.Add(new WME("louis", "gparent", "richard"));
            wmes.Add(new WME("jacques", "parent", "jean-francois"));
            wmes.Add(new WME("friedrich", "parent", "louis"));
            wmes.Add(new WME("louis", "gparent", "willard"));
            wmes.Add(new WME("willard", "gchild", "ludwig"));
            wmes.Add(new WME("miles", "parent", "ludwig"));
            wmes.Add(new WME("ludwig", "gchild", "noam"));
            wmes.Add(new WME("miles", "gparent", "wilhelm"));
            wmes.Add(new WME("friedrich", "parent", "jean-francois"));
            wmes.Add(new WME("bijan", "gchild", "george"));
            wmes.Add(new WME("wilhelm", "gparent", "bertrand"));
            wmes.Add(new WME("alan", "parent", "bijan"));
            wmes.Add(new WME("kendall", "parent", "gottlob"));
            wmes.Add(new WME("david", "gchild", "bertrand"));
            wmes.Add(new WME("alan", "gchild", "henry"));
            wmes.Add(new WME("kendall", "gparent", "louis"));
            wmes.Add(new WME("willard", "gchild", "willard"));
            wmes.Add(new WME("richard", "gchild", "pat"));
            wmes.Add(new WME("miles", "parent", "friedrich"));
            wmes.Add(new WME("noam", "gparent", "ludwig"));
            wmes.Add(new WME("jean-francois", "gchild", "bertrand"));
            wmes.Add(new WME("louis", "parent", "bijan"));
            wmes.Add(new WME("willard", "parent", "kendall"));
            wmes.Add(new WME("gottlob", "gparent", "kendall"));
            wmes.Add(new WME("immanuel", "gchild", "wilhelm"));
            wmes.Add(new WME("george", "parent", "drew"));
            wmes.Add(new WME("friedrich", "gchild", "david"));
            wmes.Add(new WME("gottlob", "gchild", "bertrand"));
            wmes.Add(new WME("wilhelm", "parent", "ludwig"));
            wmes.Add(new WME("henry", "gchild", "willard"));
            wmes.Add(new WME("alan", "gchild", "richard"));
            wmes.Add(new WME("george", "gchild", "miles"));
            wmes.Add(new WME("george", "gparent", "willard"));
            wmes.Add(new WME("alasdair", "gparent", "willard"));
            wmes.Add(new WME("willard", "gparent", "immanuel"));
            wmes.Add(new WME("jacques", "gparent", "george"));
            wmes.Add(new WME("henry", "gchild", "rudolf"));
            wmes.Add(new WME("wilhelm", "gparent", "miles"));
            wmes.Add(new WME("noam", "gparent", "jean-francois"));
            wmes.Add(new WME("pat", "gchild", "friedrich"));
            wmes.Add(new WME("rudolf", "gchild", "david"));
            wmes.Add(new WME("john", "gchild", "stanley"));
            wmes.Add(new WME("jacques", "gchild", "bijan"));
            wmes.Add(new WME("george", "parent", "miles"));
            wmes.Add(new WME("louis", "parent", "drew"));
            wmes.Add(new WME("rudolf", "parent", "jacques"));
            wmes.Add(new WME("bertrand", "gparent", "ludwig"));
            wmes.Add(new WME("gottlob", "parent", "john"));
            wmes.Add(new WME("miles", "gchild", "miles"));
            wmes.Add(new WME("bijan", "gchild", "noam"));
            wmes.Add(new WME("jean-francois", "gparent", "george"));
            wmes.Add(new WME("miles", "gparent", "henry"));
            wmes.Add(new WME("kendall", "gchild", "ludwig"));
            wmes.Add(new WME("louis", "gparent", "david"));
            wmes.Add(new WME("noam", "parent", "george"));
            wmes.Add(new WME("pat", "parent", "richard"));
            wmes.Add(new WME("george", "parent", "ludwig"));
            wmes.Add(new WME("george", "gparent", "alan"));
            wmes.Add(new WME("pat", "gparent", "louis"));
            wmes.Add(new WME("david", "parent", "friedrich"));
            wmes.Add(new WME("wilhelm", "gchild", "alasdair"));
            wmes.Add(new WME("pat", "gchild", "bertrand"));
            wmes.Add(new WME("noam", "parent", "stanley"));
            wmes.Add(new WME("friedrich", "gparent", "bijan"));
            wmes.Add(new WME("willard", "gchild", "bertrand"));
            wmes.Add(new WME("kendall", "gparent", "noam"));
            wmes.Add(new WME("kendall", "gparent", "george"));
            wmes.Add(new WME("drew", "parent", "gottlob"));
            wmes.Add(new WME("david", "parent", "kendall"));
            wmes.Add(new WME("alan", "gparent", "gottlob"));
            wmes.Add(new WME("richard", "parent", "jacques"));
            wmes.Add(new WME("alasdair", "gchild", "david"));
            wmes.Add(new WME("richard", "parent", "bijan"));
            wmes.Add(new WME("jean-francois", "gparent", "alan"));
            wmes.Add(new WME("jacques", "gchild", "louis"));
            wmes.Add(new WME("miles", "gchild", "jean-francois"));
            wmes.Add(new WME("pat", "parent", "miles"));
            wmes.Add(new WME("kendall", "parent", "gottlob"));
            wmes.Add(new WME("gottlob", "parent", "willard"));
            wmes.Add(new WME("jean-francois", "parent", "willard"));
            wmes.Add(new WME("stanley", "gchild", "ludwig"));
            wmes.Add(new WME("john", "gchild", "miles"));
            wmes.Add(new WME("john", "parent", "jean-francois"));
            wmes.Add(new WME("alan", "gchild", "louis"));
            wmes.Add(new WME("stanley", "gchild", "wilhelm"));
            wmes.Add(new WME("drew", "gparent", "miles"));
            wmes.Add(new WME("jacques", "parent", "immanuel"));
            wmes.Add(new WME("david", "gparent", "willard"));
            wmes.Add(new WME("miles", "parent", "alasdair"));
            wmes.Add(new WME("immanuel", "gparent", "ludwig"));
            wmes.Add(new WME("kendall", "gparent", "henry"));
            wmes.Add(new WME("immanuel", "gparent", "ludwig"));
            wmes.Add(new WME("miles", "gchild", "john"));
            wmes.Add(new WME("louis", "gchild", "alan"));
            wmes.Add(new WME("willard", "parent", "alan"));
            wmes.Add(new WME("kendall", "parent", "ludwig"));
            wmes.Add(new WME("george", "gchild", "alasdair"));
            wmes.Add(new WME("richard", "gparent", "bertrand"));

            int i = 1;
            foreach (WME wme in wmes)
            {
                wme.Label = "W" + i++;
                rete.AddWME(wme);
            }

            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\GrandParentsTest.log", false))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }

            Assert.IsTrue(rete.WorkingMemory.Count == 100, "Bad");
            Assert.IsTrue(prod.InferredFacts.Count == 33, "Wrong number of conclusions");
            Assert.IsTrue(prod1.InferredFacts.Count == 42, "Wrong number of conclusions");
            Assert.IsTrue(prod2.InferredFacts.Count == 29, "Wrong number of conclusions");
            Assert.IsTrue(prod3.InferredFacts.Count == 38, "Wrong number of conclusions");
        }

        [Test]
        public void TestAddProduction()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            prod.AddConditionToRHS(new AssertCondition("C4", x, "is", "on top"));

            Rete rete = new Rete();
            rete.AddProduction(prod);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\TestAddProduction - 1.log", false))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }

            //Assert.IsTrue(rete.RulesThatFired.Count == 1, "Rule did not fire.");
            Assert.IsTrue(prod.InferredFacts.Count == 1, "Wrong number of conclusions");
            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");
        }

        /*
        C1: (<x> ^on <y>)
        C2: (<y> ^left-of <z>)
        C3: (<z> ^color red)
        C4: (<a> ^color maize)
        C5: (<b> ^color blue)
        C6: (<c> ^color green)
        C7: (<d> ^color white)
        C8: (<s> ^on table)
        C9: (<y> ^<a> <b>)
        C10: (<a> ^left-of <d>)
        */

        [Test]
        public void ReteWithHash()
        {
            Production prod = new Production();
            prod.Label = "ReteWithHash";
            Variable a = new Variable("a");
            Variable b = new Variable("b");
            Variable c = new Variable("c");
            Variable d = new Variable("d");
            Variable s = new Variable("s");

            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            prod.AddConditionToLHS(new PositiveCondition("C4", a, "color", "maize"));
            prod.AddConditionToLHS(new PositiveCondition("C5", b, "color", "blue"));
            prod.AddConditionToLHS(new PositiveCondition("C6", c, "color", "green"));
            prod.AddConditionToLHS(new PositiveCondition("C7", d, "color", "white"));
            prod.AddConditionToLHS(new PositiveCondition("C8", s, "on", "table"));
            prod.AddConditionToLHS(new PositiveCondition("C9", z, a, b));
            prod.AddConditionToLHS(new PositiveCondition("C10", a, "left of", d));

            prod.AddConditionToRHS(new AssertCondition("C4", x, "is", "on top"));

            Rete rete = new Rete();
            rete.AddProduction(prod);

            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\ReteWithHash-Pre.log", false))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\ReteWithHash.log", false))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }

            Assert.IsTrue(prod.InferredFacts.Count == 0, "Wrong number of conclusions");
        }

        [Test]
        public void EvaluatorTest()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            //Variable c = new Variable("c");
            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            //prod.AddConditionToLHS(new condition("C3", z, "color", "red"));

            PositiveCondition noteq = new PositiveCondition("NOT BLUE", x, "color", "blue");
            noteq.Evaluator = EvaluatorManager.Instance.Evaluators["NotEquals"];
            prod.AddConditionToLHS(noteq);

            prod.AddConditionToRHS(new AssertCondition("C5", x, "calls", z));

            Rete rete = new Rete();
            rete.AddProduction(prod);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\EvaluatorTest.log", false))
            {
                writer.Write(printer.Output);
                writer.Flush();
            }

            Assert.IsTrue(prod.InferredFacts.Count == 2, "Wrong number of conclusions");
            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");
        }

        [Test]
        public void RemoveWME()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            AssertCondition rhs = new AssertCondition("C4", x, "is", "on top");
            rhs.ConditionType = ConditionType.Assert;

            prod.AddConditionToRHS(rhs);
            Rete rete = new Rete();

            rete.AddProduction(prod);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            Assert.IsTrue(prod.InferredFacts.Count == 1, "Wrong number of conclusions");

            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");

            rete.RemoveWME(wme9);

            Assert.IsTrue(prod.InferredFacts.Count == 0, "Wrong number of conclusions");

            Assert.IsTrue(rete.WorkingMemory.Count == 8, "Bad");
        }

        [Test]
        public void RemoveProduction()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            AssertCondition rhs = new AssertCondition("C4", x, "is", "on top");
            rhs.ConditionType = ConditionType.Assert;

            prod.AddConditionToRHS(rhs);
            Rete rete = new Rete();

            rete.AddProduction(prod);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            Assert.IsTrue(prod.InferredFacts.Count == 1, "Wrong number of conclusions");

            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");

            rete.RemoveProduction(prod);

            Assert.IsTrue(prod.InferredFacts.Count == 0, "Wrong number of conclusions");

            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");
        }

        [Test]
        public void Test4pointsTo1()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            prod.AddConditionToLHS(new PositiveCondition("C4", x, "has", "car"));
            prod.AddConditionToLHS(new PositiveCondition("C5", x, "wants", z));
            prod.AddConditionToRHS(new AssertCondition("C5", x, "gets", z));
            Rete rete = new Rete();

            rete.AddProduction(prod);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            WME wme10 = new WME("W10");
            wme10.Fields[0] = "B2";
            wme10.Fields[1] = "has";
            wme10.Fields[2] = "bike";
            rete.AddWME(wme10);

            WME wme11 = new WME("W11");
            wme11.Fields[0] = "B1";
            wme11.Fields[1] = "has";
            wme11.Fields[2] = "car";
            rete.AddWME(wme11);

            WME wme12 = new WME("W12");
            wme12.Fields[0] = "B1";
            wme12.Fields[1] = "wants";
            wme12.Fields[2] = "B3";
            rete.AddWME(wme12);

            //Assert.IsTrue(rete.RulesThatFired.Count == 1, "Rule did not fire.");
            Assert.IsTrue(prod.InferredFacts.Count == 1, "Wrong number of conclusions");
            Assert.IsTrue(rete.WorkingMemory.Count == 12, "Bad");
        }

        [Test]
        public void MultiBuiltinTest()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            //Variable z = new Variable("z");
            Variable c = new Variable("c");
            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            //prod.AddConditionToLHS(new condition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", x, "color", c));

            FunctionCondition funCond = new FunctionCondition("F4", c, new FuncTerm("funcNotEquals", new funcNotEquals()), new StringTerm("blue"));
            funCond.ConditionType = ConditionType.Function;
            prod.AddConditionToLHS(funCond);

            prod.AddConditionToRHS(new AssertCondition("C4", x, "is", c));

            Rete rete = new Rete();
            rete.AddProduction(prod);

            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\MultiBuiltinTest.log", false))
            {
                writer.Write(printer.Output);
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("-----------------------------------------------");
                writer.Flush();
            }

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\MultiBuiltinTest.log", true))
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.Write(printer.Output);
                writer.Flush();
            }

            //Assert.IsTrue(rete.RulesThatFired.Count == 1, "Rule did not fire.");
            Assert.IsTrue(prod.InferredFacts.Count == 2, "Wrong number of InferredFacts");
            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");
        }

        [Test]
        public void BuiltinTest()
        {
            Production prod = new Production();
            prod.Label = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));

            FunctionCondition funCond = new FunctionCondition("F4", x, new FuncTerm("funcEquals", new funcEquals()), new StringTerm("B1"));
            funCond.ConditionType = ConditionType.Function;
            prod.AddConditionToLHS(funCond);

            prod.AddConditionToRHS(new AssertCondition("C4", x, "is", "on top"));

            Rete rete = new Rete();
            rete.AddProduction(prod);

            NetworkPrinter printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\TestBuiltin.log", false))
            {
                writer.Write(printer.Output);
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("-----------------------------------------------");
                writer.Flush();
            }

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            rete.AddWME(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            rete.AddWME(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            rete.AddWME(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            rete.AddWME(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            rete.AddWME(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            rete.AddWME(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            rete.AddWME(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            rete.AddWME(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            rete.AddWME(wme9);

            printer = new NetworkPrinter();
            rete.DummyTopNode.Accept(printer);

            using (StreamWriter writer = new StreamWriter(@"C:\Temp\TestBuiltin.log", true))
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.Write(printer.Output);
                writer.Flush();
            }

            Assert.IsTrue(prod.InferredFacts.Count == 1, "Wrong number of conclusions");
            Assert.IsTrue(rete.WorkingMemory.Count == 9, "Bad");
        }
    }
}