namespace BooRulesMacro.Tester

import System
import Expergent
import Expergent.Terms
import Expergent.Interfaces
import Expergent.Authoring
import System.Collections.Generic
import BooRulesMacro


RULESET Label="First", EffectiveDate="12/12/2006", TerminationDate="12/12/2007":
	RULE Label="Rule7", Salience=11:
		IF:
			AND(?Customer, $Customer.FirstName, ?FName)
			AND(?Customer, "has", "car")
		THEN:
			ASSERT(?Customer, "firstname", ?FName)
