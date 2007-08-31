using Expergent.Conditions;
using NUnit.Framework;
using Expergent.Builtins;
using Expergent.ConflictResolvers;
using Expergent.Terms;

namespace Expergent.Tester
{
    public class BuiltinsTests
    {
        [Test]
        public void ListTest1()
        {
            Production prod = new Production();
            prod.Label = "ListTest";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            Variable l = new Variable("l");

            GenericListTerm<string> virtues = new GenericListTerm<string>(new string[] {"Calm", "Cool", "Collected"});

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            prod.AddConditionToLHS(new PositiveCondition("C4", x, "has virtue", l));
            prod.AddConditionToLHS(new FunctionCondition("C5", l, new FuncTerm("isInList", new isInList()), virtues));

            AssertCondition rhs = new AssertCondition("R1", x, "is", "good");
            rhs.ConditionType = ConditionType.Assert;

            prod.AddConditionToRHS(rhs);

            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new SalienceResolver();
            agenda.AddProduction(prod);


            WME v1 = new WME("V1", "B1", "has virtue", "Calm");
            agenda.AddFact(v1);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            agenda.AddFact(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            agenda.AddFact(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            agenda.AddFact(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            agenda.AddFact(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            agenda.AddFact(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            agenda.AddFact(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            agenda.AddFact(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            agenda.AddFact(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            agenda.AddFact(wme9);

            agenda.Run();

            agenda.PrintNetwork(@"C:\Temp\ListTest.log", false);

            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Rule did not fire.");
            Assert.IsTrue(agenda.InferredFacts.Count == 1, "Bad");
        }

        [Test]
        public void ListTest2()
        {
            Production prod = new Production();
            prod.Label = "ListTest";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            Variable l = new Variable("l");
            Variable v = new Variable("v");

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            prod.AddConditionToLHS(new PositiveCondition("C4", x, "has virtue", l));
            prod.AddConditionToLHS(new PositiveCondition("C5", "virtues", "virtueList", v));
            prod.AddConditionToLHS(new FunctionCondition("C6", l, new FuncTerm("isInList", new isInList()), v));

            AssertCondition rhs = new AssertCondition("R1", x, "is", "good");
            rhs.ConditionType = ConditionType.Assert;

            prod.AddConditionToRHS(rhs);

            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new SalienceResolver();
            agenda.AddProduction(prod);


            WME v1 = new WME("V1", "B1", "has virtue", "Calm");
            agenda.AddFact(v1);

            WME v2 = new WME("V2", "virtues", "virtueList", new string[] {"Calm", "Cool", "Collected"});
            agenda.AddFact(v2);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "B1";
            wme1.Fields[1] = "on";
            wme1.Fields[2] = "B2";
            agenda.AddFact(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "B1";
            wme2.Fields[1] = "on";
            wme2.Fields[2] = "B3";
            agenda.AddFact(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "B1";
            wme3.Fields[1] = "color";
            wme3.Fields[2] = "red";
            agenda.AddFact(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "B2";
            wme4.Fields[1] = "on";
            wme4.Fields[2] = "table";
            agenda.AddFact(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "B2";
            wme5.Fields[1] = "left of";
            wme5.Fields[2] = "B3";
            agenda.AddFact(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "B2";
            wme6.Fields[1] = "color";
            wme6.Fields[2] = "blue";
            agenda.AddFact(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "B3";
            wme7.Fields[1] = "left of";
            wme7.Fields[2] = "B4";
            agenda.AddFact(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "B3";
            wme8.Fields[1] = "on";
            wme8.Fields[2] = "table";
            agenda.AddFact(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "B3";
            wme9.Fields[1] = "color";
            wme9.Fields[2] = "red";
            agenda.AddFact(wme9);

            agenda.Run();

            agenda.PrintNetwork(@"C:\Temp\ListTest2.log", false);

            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Rule did not fire.");
            Assert.IsTrue(agenda.InferredFacts.Count == 1, "Bad");
        }
    }
}