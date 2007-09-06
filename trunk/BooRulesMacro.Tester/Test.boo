namespace BooRulesMacro.Tester

import System
import Expergent
import Expergent.Aggregators
import Expergent.Conditions
import Expergent.Terms
import Expergent.Interfaces
import Expergent.Authoring
import Expergent.MutexEvaluators
import System.Collections.Generic
import BooRulesMacro


RULESET Label="First", EffectiveDate="12/12/2006", TerminationDate="12/12/2007":
	RULE Label="Rule7", Salience=11:
		IF:
			AND(?Customer, $Customer.FirstName, ?FName)
			AND(?Customer, "has", "car")
		THEN:
			ASSERT(?Customer, "firstname", ?FName)
			
			
	MUTEX Label="MUTEX", Evaluator=Max(?Customer, "give discount", ?Discount):
		IF:
			AND(?Customer, $Customer.FirstName, ?FName)
			AND(?Customer, "has", "car")
		THEN:
			ASSERT(?Customer, "firstname", ?FName)
			
	OVERRIDE Label="O1", Winner="win", Loser="loss":
		pass
		
	AGGREGATOR Label='A1', GroupBy='Customer', AggregatorFunction=Count('$Customer.Orders.Order', '$Customer.Orders.Count'), ConditionType=ConditionType.Assert:
		IF:
			AND(?Customer, $Customer.FirstName, ?FName)
			AND(?Customer, "has", "car")
