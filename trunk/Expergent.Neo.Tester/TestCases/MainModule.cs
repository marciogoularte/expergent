using System;
using System.Runtime.CompilerServices;
using BooRulesTests.BooRulesTests;
using Expergent.Neo.Tester.TestCases;
using Expergent.Tester;

namespace BooRulesTests
{
    [CompilerGlobalScope]
    public sealed class MainModule
    {
        private MainModule()
        {
        }

        private static void Main(string[] argv)
        {
            Console.WriteLine(argv);
            Console.WriteLine("Hello from Test World!");

            RunNeoTests();
            RunScenarioTests();
            RunAggregatorTests();

            Program program = new Program();
            program.Main();
            
            Console.WriteLine("So Long from Test World");
        }

        private static void RunNeoTests()
        {
            Console.WriteLine("Running Neo Tests...");
            NeoTests nt = new NeoTests();
            nt.BasicConnectivityTest();
            nt.FirstTest();
            nt.Test2();
            for (int i = 0; i < 1; i++)
            {
                nt.TimeStampConflictResolver();
            }
            nt.DigDeep();
            nt.CollectionTest();
            nt.TestRuleLoader();
            Console.WriteLine("Completed Neo Tests...");
        }

        private static void RunScenarioTests()
        {
            ScenarioTests st = new ScenarioTests();
            st.RetractRule();
        }

        private static void RunAggregatorTests()
        {
            AggregatorTests tests = new AggregatorTests();
            tests.FirstAggTest();
            tests.SecondAggTest();
            tests.TestOfSum();
            tests.TestOfAverage();
            tests.TestOfMin();
            tests.TestOfMax();
        }
    }
}

