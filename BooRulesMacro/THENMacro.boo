#region license
// Copyright (c) 2007, Michael A. Rose (The Universal Sun, Inc.) (michael@theuniversalsun.com)
// All rights reserved.
//
// This file is part of Expergent.
//
// Expergent is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation; either version 3 of the License, or
// (at your option) any later version.
// 
// Expergent is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this distribution; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion

namespace BooRulesMacro

import System
import Boo.Lang.Compiler
import Boo.Lang.Compiler.Ast
import Boo.Lang.Compiler.Ast.AstUtil
import Boo.Lang.Compiler.TypeSystem
import Boo.Lang.Compiler.Ast.Visitors
import UsefulMacro

class THENMacro(UsefulMacroBase):
	
	_rule as MacroStatement
	_ruleSet as MacroStatement
	_rulename as string
	static final Usage = "Usage: THEN:"
				
	override def Expand(macro as MacroStatement):
		Init(macro)
		_ruleSet = GetParentMacroByName("RULESET", macro)
		assert _ruleSet is not null
		_rule = GetParentMacroByName("RULE", macro)
		assert _rule is not null
		
		if _rule.ContainsAnnotation(Consts.RULENAME):
			_rulename = _rule[Consts.RULENAME] as string
		else:
			_rulename = SafeIdentifierName(opt(_rule, "Label", L("Rule${Helpers.Counter}")).ToString())
			_rule[Consts.RULENAME] = _rulename
		
		//System.Diagnostics.Debugger.Launch()
		
		if not CheckUsage(macro):
			Errors.Add(
				CompilerErrorFactory.CustomError(macro.LexicalInfo, Usage))
			return null
		block = Block()
		
		for stmt in macro.Block.Statements:
			if stmt isa ExpressionStatement:
				AddProductions(block, stmt as ExpressionStatement)		
		
		return block
		
	def CheckUsage(macro as MacroStatement):
		if len(macro.Arguments) != 0: return false
		if len(macro.Block.Statements) == 0: return false
		return true

	def AddProductions(block as Block, st as ExpressionStatement):
		stmt = st.Expression as MethodInvocationExpression
		if stmt:
			for i in range(stmt.Arguments.Count):
				refarg = stmt.Arguments[i] as ReferenceExpression
				if refarg:
					if refarg.ToString().StartsWith("?"):
						objVar as string
						if _ruleSet.ContainsAnnotation(refarg.Name):
							objVar = _ruleSet[refarg.Name] as string
						else:
							objVar = SafeIdentifierName(refarg.Name) + "_var"
							_ruleSet[refarg.Name] = objVar
							block.Add(BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression(objVar), MethodInvocationExpression(ReferenceExpression('Variable'), L(refarg.Name))))
						
						stmt.Arguments.Replace(refarg, ReferenceExpression(objVar))

					elif refarg.ToString().StartsWith("$"):
						if IsValidReference(refarg.ToString()):
							stmt.Arguments.Replace(refarg, L(refarg.ToString()))
						else:
							Errors.Add(CompilerErrorFactory.CustomError(refarg.LexicalInfo, _errorMessage))
			
			mie = MethodInvocationExpression()
			mie.Arguments.Add(stmt)
			mie.Target = CreateReferenceExpression("${_rulename}.AddConditionToRHS")
			
			block.Add(mie)
