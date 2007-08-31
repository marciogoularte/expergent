using Expergent.Conditions;
using NUnit.Framework;
using Expergent.Builtins;
using Expergent.MutexEvaluators;
using Expergent.ConflictResolvers;
using Expergent.Terms;

namespace Expergent.Tester
{
    public class MetaNodeTester
    {
        [Test]
        public void BasicMetaTest()
        {
            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new SalienceResolver();

            Production prod = new Production();
            prod.Label = "Gold Customer";
            Variable customer = new Variable("customer");
            Variable purchases = new Variable("purchases");
            //Variable y = new Variable("y");
            //Variable x = new Variable("x");

            prod.AddConditionToLHS(new PositiveCondition("C1", customer, "purchases", purchases));
            FunctionCondition gt = new FunctionCondition("C2", purchases, new FuncTerm("funcGreaterThan", new funcGreaterThan()), 10);
            gt.ConditionType = ConditionType.Function;
            prod.AddConditionToLHS(gt);
            AssertCondition rhs = new AssertCondition("R1", customer, "is", "gold");
            rhs.ConditionType = ConditionType.Assert;
            prod.AddConditionToRHS(rhs);
            agenda.AddProduction(prod);

            Production prod1 = new Production();
            prod1.Label = "Platinum Customer";

            prod1.AddConditionToLHS(new PositiveCondition("C1", customer, "purchases", purchases));
            FunctionCondition gt1 = new FunctionCondition("C2", purchases, new FuncTerm("funcGreaterThan", new funcGreaterThan()), 20);
            gt1.ConditionType = ConditionType.Function;
            prod1.AddConditionToLHS(gt1);
            AssertCondition rhs1 = new AssertCondition("R1", customer, "is", "Platinum");
            rhs1.ConditionType = ConditionType.Assert;
            prod1.AddConditionToRHS(rhs1);
            agenda.AddProduction(prod1);

            //production mut = new production();
            //mut.label = "mutex";
            //mut.AddConditionToLHS(new condition("C1", customer, "is", "Platinum"));
            //mut.AddConditionToLHS(new condition("C2", customer, "is", "gold"));

            //condition gtx = new condition("C3", y, "funcNotEquals", x);
            //gt1.ConditionType = ConditionType.Function;
            ////prod1.AddConditionToLHS(gtx);
            //condition rhsx = new condition("R1", customer, "is", "Platinum");
            //rhs1.ConditionType = ConditionType.Assert;
            //prod1.AddConditionToRHS(rhsx);
            //agenda.AddProduction(mut);

            //agenda.AddMutexNode(prod, new condition("R1", customer, "is", "gold"), prod1, new condition("R1", customer, "is", "Platinum"), new NotEquals());

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "joe";
            wme1.Fields[1] = "purchases";
            wme1.Fields[2] = "1";
            agenda.AddFact(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "ted";
            wme2.Fields[1] = "purchases";
            wme2.Fields[2] = "10";
            agenda.AddFact(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "ed";
            wme3.Fields[1] = "purchases";
            wme3.Fields[2] = "11";
            agenda.AddFact(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "phil";
            wme4.Fields[1] = "purchases";
            wme4.Fields[2] = "18";
            agenda.AddFact(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "mary";
            wme5.Fields[1] = "purchases";
            wme5.Fields[2] = "22";
            agenda.AddFact(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "jane";
            wme6.Fields[1] = "purchases";
            wme6.Fields[2] = "25";
            agenda.AddFact(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "fred";
            wme7.Fields[1] = "purchases";
            wme7.Fields[2] = "50";
            agenda.AddFact(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "harry";
            wme8.Fields[1] = "purchases";
            wme8.Fields[2] = "55";
            agenda.AddFact(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "jon";
            wme9.Fields[1] = "purchases";
            wme9.Fields[2] = "99";
            agenda.AddFact(wme9);

            agenda.Run();

            agenda.PrintNetwork(@"C:\Temp\BasicOverrideTest.log", false);

            Assert.IsTrue(agenda.ActivatedRuleCount == 2, "Rule did not fire.");
            Assert.IsTrue(agenda.InferredFacts.Count == 12, "Bad");
        }

        [Test]
        public void GiveBestDiscount()
        {
            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new SalienceResolver();


            Variable customer = new Variable("customer");
            Variable purchases = new Variable("purchases");
            Variable discount = new Variable("discount");

            Production SilverCustomer = new Production("Silver Customer");
            SilverCustomer.AddConditionToLHS(new PositiveCondition("C1", customer, "purchases", purchases));
            SilverCustomer.AddConditionToLHS(new FunctionCondition("C2", purchases, new FuncTerm("funcGreaterThan", new funcGreaterThan()), 10));
            SilverCustomer.AddConditionToRHS(new AssertCondition("R1", customer, "is", "silver"));
            agenda.AddProduction(SilverCustomer);

            Production GoldCustomer = new Production("Gold Customer");
            GoldCustomer.AddConditionToLHS(new PositiveCondition("C1", customer, "purchases", purchases));
            GoldCustomer.AddConditionToLHS(new FunctionCondition("C2", purchases, new FuncTerm("funcGreaterThan", new funcGreaterThan()), 25));
            GoldCustomer.AddConditionToRHS(new AssertCondition("R1", customer, "is", "gold"));
            agenda.AddProduction(GoldCustomer);

            Production PlatinumCustomer = new Production("Platinum Customer");
            PlatinumCustomer.AddConditionToLHS(new PositiveCondition("C1", customer, "purchases", purchases));
            PlatinumCustomer.AddConditionToLHS(new FunctionCondition("C2", purchases, new FuncTerm("funcGreaterThan", new funcGreaterThan()), 50));
            PlatinumCustomer.AddConditionToRHS(new AssertCondition("R1", customer, "is", "platinum"));
            agenda.AddProduction(PlatinumCustomer);

            Production SilverDiscount = new Production("Silver Discount");
            SilverDiscount.AddConditionToLHS(new PositiveCondition("C1", customer, "is", "silver"));
            SilverDiscount.AddConditionToRHS(new AssertCondition("R1", customer, "give discount", 5));
            agenda.AddProduction(SilverDiscount);

            Production GoldDiscount = new Production("Gold Discount");
            GoldDiscount.AddConditionToLHS(new PositiveCondition("C1", customer, "is", "gold"));
            GoldDiscount.AddConditionToRHS(new AssertCondition("R1", customer, "give discount", 10));
            agenda.AddProduction(GoldDiscount);

            Production PlatinumDiscount = new Production("Platinum Discount");
            PlatinumDiscount.AddConditionToLHS(new PositiveCondition("C1", customer, "is", "platinum"));
            PlatinumDiscount.AddConditionToRHS(new AssertCondition("R1", customer, "give discount", 15));
            agenda.AddProduction(PlatinumDiscount);

            Mutex BestDiscount = new Mutex("Best Discount");
            //BestDiscount.AddAggregator(new Max(), customer, new StringTerm("give discount"), discount);
            BestDiscount.AddConditionToLHS(new PositiveCondition("C1", customer, "give discount", discount));
            //BestDiscount.AddConditionToLHS(new condition("C2", ConditionType.Function, customer, "funcMax", discount));
            BestDiscount.AddConditionToRHS(new InvokeCondition("R1", customer, "Customer.GiveDiscount", discount));
            BestDiscount.AddEvaluator(new Max(), customer, new StringTerm("give discount"), discount);
            agenda.AddMutex(BestDiscount);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "joe";
            wme1.Fields[1] = "purchases";
            wme1.Fields[2] = "1";
            agenda.AddFact(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "ted";
            wme2.Fields[1] = "purchases";
            wme2.Fields[2] = "10";
            agenda.AddFact(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "ed";
            wme3.Fields[1] = "purchases";
            wme3.Fields[2] = "11";
            agenda.AddFact(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "phil";
            wme4.Fields[1] = "purchases";
            wme4.Fields[2] = "18";
            agenda.AddFact(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "mary";
            wme5.Fields[1] = "purchases";
            wme5.Fields[2] = "22";
            agenda.AddFact(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "jane";
            wme6.Fields[1] = "purchases";
            wme6.Fields[2] = "25";
            agenda.AddFact(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "fred";
            wme7.Fields[1] = "purchases";
            wme7.Fields[2] = "50";
            agenda.AddFact(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "harry";
            wme8.Fields[1] = "purchases";
            wme8.Fields[2] = "55";
            agenda.AddFact(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "jon";
            wme9.Fields[1] = "purchases";
            wme9.Fields[2] = "99";
            agenda.AddFact(wme9);

            agenda.Run();

            agenda.PrintNetwork(@"C:\Temp\GiveBestDiscount.log", false);

            Assert.IsTrue(agenda.ActivatedRuleCount == 5, "Rule did not fire.");
            Assert.IsTrue(agenda.InferredFacts.Count == 21, "Bad");
            Assert.IsTrue(BestDiscount.InferredFacts.Count == 7, "Bad");
        }

        [Test]
        public void BasicOverrideTest()
        {
            Agenda agenda = new Agenda();
            agenda.ConflictResolutionStrategy = new SalienceResolver();

            Production prod = new Production();
            prod.Label = "Gold Customer";
            Variable customer = new Variable("customer");
            Variable purchases = new Variable("purchases");

            prod.AddConditionToLHS(new PositiveCondition("C1", customer, "purchases", purchases));
            FunctionCondition gt = new FunctionCondition("C2", purchases, new FuncTerm("funcGreaterThan", new funcGreaterThan()), 10);
            gt.ConditionType = ConditionType.Function;
            prod.AddConditionToLHS(gt);
            AssertCondition rhs = new AssertCondition("R1", customer, "is", "gold");
            rhs.ConditionType = ConditionType.Assert;
            prod.AddConditionToRHS(rhs);
            agenda.AddProduction(prod);

            Production prod1 = new Production();
            prod1.Label = "Platinum Customer";

            prod1.AddConditionToLHS(new PositiveCondition("C1", customer, "purchases", purchases));
            FunctionCondition gt1 = new FunctionCondition("C2", purchases, new FuncTerm("funcGreaterThan", new funcGreaterThan()), 20);
            gt1.ConditionType = ConditionType.Function;
            prod1.AddConditionToLHS(gt1);
            AssertCondition rhs1 = new AssertCondition("R1", customer, "is", "Platinum");
            rhs.ConditionType = ConditionType.Assert;
            prod1.AddConditionToRHS(rhs1);
            agenda.AddProduction(prod1);

            Override me = new Override();
            me.Winner = "Platinum Customer";
            me.Loser = "Gold Customer";

            agenda.AddOverride(me);

            WME wme1 = new WME("W1");
            wme1.Fields[0] = "joe";
            wme1.Fields[1] = "purchases";
            wme1.Fields[2] = "1";
            agenda.AddFact(wme1);

            WME wme2 = new WME("W2");
            wme2.Fields[0] = "ted";
            wme2.Fields[1] = "purchases";
            wme2.Fields[2] = "10";
            agenda.AddFact(wme2);

            WME wme3 = new WME("W3");
            wme3.Fields[0] = "ed";
            wme3.Fields[1] = "purchases";
            wme3.Fields[2] = "11";
            agenda.AddFact(wme3);

            WME wme4 = new WME("W4");
            wme4.Fields[0] = "phil";
            wme4.Fields[1] = "purchases";
            wme4.Fields[2] = "18";
            agenda.AddFact(wme4);

            WME wme5 = new WME("W5");
            wme5.Fields[0] = "mary";
            wme5.Fields[1] = "purchases";
            wme5.Fields[2] = "22";
            agenda.AddFact(wme5);

            WME wme6 = new WME("W6");
            wme6.Fields[0] = "jane";
            wme6.Fields[1] = "purchases";
            wme6.Fields[2] = "25";
            agenda.AddFact(wme6);

            WME wme7 = new WME("W7");
            wme7.Fields[0] = "fred";
            wme7.Fields[1] = "purchases";
            wme7.Fields[2] = "50";
            agenda.AddFact(wme7);

            WME wme8 = new WME("W8");
            wme8.Fields[0] = "harry";
            wme8.Fields[1] = "purchases";
            wme8.Fields[2] = "55";
            agenda.AddFact(wme8);

            WME wme9 = new WME("W9");
            wme9.Fields[0] = "jon";
            wme9.Fields[1] = "purchases";
            wme9.Fields[2] = "99";
            agenda.AddFact(wme9);

            agenda.Run();

            agenda.PrintNetwork(@"C:\Temp\BasicOverrideTest.log", false);

            Assert.IsTrue(agenda.ActivatedRuleCount == 1, "Rule did not fire.");
            Assert.IsTrue(agenda.InferredFacts.Count == 5, "Bad");
        }
    }
}