namespace BooRulesMacro.Tester

import System
import System.Collections.Generic

class Customer:
"""Description of Customer"""
	
	[property(FirstName)]
	_fname as string
	
	[property(LastName)]
	_lname as string
	
	[property(Foo)]
	_foo as Foo
	
	[property(AnotherFoo)]
	_foo2 as Foo
	
	[getter(Orders)]
	_orders as List of Order
	
	def constructor():
		_foo = Foo()
		_orders = List of Order()
		
	def Shout(x):
		print x

class Foo:
"""Description of Foo"""
	[property(Name)]
	_name as string
	
	[property(Bar)]
	_bar as Bar
	
	def constructor():
		_bar = Bar()

class Bar:
"""Description of Bar"""
	
	[property(Name)]
	_name as string
	
	def constructor():
		pass
		
	def Say(x):
		print x

class Order:

	[property(Item)]
	_item as string
	
	def constructor(item as string):
		_item = item

