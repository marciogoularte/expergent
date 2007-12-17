using Expergent.Authoring;
using Expergent.Conditions;
using Expergent.ConflictResolvers;
using Expergent.Terms;
using NUnit.Framework;

namespace Expergent.Neo.Tester.TestCases
{
    public class ScenarioTests
    {
        [Test]
        public void RetractRule()
        {
            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new SalienceResolver();

            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");

            Production prod = new Production("find-stack-of-two-blocks-to-the-left-of-a-red-block");
            prod.Salience = 1;

            prod.AddConditionToLHS(new PositiveCondition("C1", x, "on", y));
            prod.AddConditionToLHS(new PositiveCondition("C2", y, "left of", z));
            prod.AddConditionToLHS(new PositiveCondition("C3", z, "color", "red"));
            prod.AddConditionToRHS(new AssertCondition("R1", x, "is", "on top"));
            prod.AddConditionToRHS(new AssertCondition("R2", "mary", "looks", "fine"));
            agenda.AddProduction(prod);

           
            Production dependentProd = new Production("Dependent Production");
            dependentProd.Salience = 55;
            dependentProd.AddConditionToLHS(new AND("mary", "looks", "fine"));
            dependentProd.AddConditionToRHS(new ASSERT("should", "not", "see"));
            agenda.AddProduction(dependentProd);

            Production retractProd = new Production("Retract Production");
            retractProd.Salience = 50;
            retractProd.AddConditionToLHS(new AND(x, "is", "on top"));
            retractProd.AddConditionToRHS(new RETRACT("mary", "looks", "fine"));
            agenda.AddProduction(retractProd);

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

            agenda.VisualizeNetworkToFile(@"C:\Temp\RetractRule.log", false);

            Assert.IsTrue(agenda.ActivatedRuleCount == 2, "Rule did not fire.");
            Assert.IsTrue(agenda.NotActivatedRuleCount == 1, "Rule should not have fired.");
            Assert.IsTrue(agenda.InferredFacts.Count == 1, "Bad");
        }
    }
}