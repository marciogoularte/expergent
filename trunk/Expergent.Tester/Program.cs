namespace Expergent.Tester
{
    public class Program
    {
        public void Main()
        {
            TesterStub stub = new TesterStub();
            stub.RunCollectionTests();
            stub.RunAlphaMemoryTests();
            stub.RunAgendaTests();
            stub.RunMetaTests();
            stub.RunBuiltinTests();
            stub.RunTermTests();
            stub.ObjectGrapherTests();
        }
    }

    public class TesterStub
    {
        public void RunCollectionTests()
        {
            CollectionsTester ctest = new CollectionsTester();
            ctest.HashMapTest();
        }

        public void RunAlphaMemoryTests()
        {
            AlphaMemoryTests amt = new AlphaMemoryTests();
            amt.TestAddWME();
            amt.TestAddProduction();
            amt.NegativeConditionTest();
            amt.NCCConditionTest();
            amt.GrandParents();
            amt.RemoveWME();
            amt.RemoveProduction();
            amt.Test4pointsTo1();
            amt.EvaluatorTest();
            amt.ReteWithHash();
            amt.BuiltinTest();
            amt.MultiBuiltinTest();
        }

        public void RunAgendaTests()
        {
            AgendaTests amt = new AgendaTests();
            amt.TestAddProduction();
        }

        public void RunMetaTests()
        {
            MetaNodeTester tester = new MetaNodeTester();
            tester.BasicOverrideTest();
            tester.BasicMetaTest();
            tester.GiveBestDiscount();
        }

        public void RunBuiltinTests()
        {
            BuiltinsTests bt = new BuiltinsTests();
            bt.ListTest1();
            bt.ListTest2();
        }
        public void RunTermTests()
        {
            TermFactoryTests tft = new TermFactoryTests();
            tft.BasicTest();
        }
        public void ObjectGrapherTests()
        {
            ObjectGrapherTest ogt = new ObjectGrapherTest();
            ogt.ObjectGraphTest();
            ogt.ObjectMapTest();
        }
    }
}