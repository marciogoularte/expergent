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

class MUTEXMacro(UsefulMacroBase):
	
	static final Usage = "Usage: MUTEX Label='somelabel', Evaluator=Max(?Customer, 'give discount', ?Discount):"
	
	override def Expand(macro as MacroStatement) as Statement:
		Init(macro)
		if not CheckUsage(macro):
			Errors.Add(CompilerErrorFactory.CustomError(macro.LexicalInfo, Usage))
			return null
		//System.Diagnostics.Debugger.Launch()
		label as string
		if macro.ContainsAnnotation(Consts.MUTEXNAME):
			label = macro[Consts.MUTEXNAME] as string
		else:
			label = SafeIdentifierName(opt("Label", L("Mutex${Helpers.Counter}")).ToString())
			macro[Consts.MUTEXNAME] = label

		block = Block()
		block.Annotate("MUTEX")
		rule = BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression(label), MethodInvocationExpression(ReferenceExpression('Mutex')))
		block.Add(rule)
		block.Add(BinaryExpression(BinaryOperatorType.Assign, CreateReferenceExpression(label + '.Label'), L(label)      ))
		evaluator = GetAssignment("Evaluator")
		if not evaluator:
			Errors.Add(CompilerErrorFactory.CustomError(macro.LexicalInfo, "The Evaluator argument is required."))
			return null
		ProcessEvaluatorArguments(block, evaluator, label)
		for stmt as Statement in macro.Block.Statements:
			block.Add(stmt)
		mie = MethodInvocationExpression()
		mie.Arguments.Add(ReferenceExpression(label))
		mie.Target = CreateReferenceExpression("list.Add")
		block.Add(mie)
		return block

	def CheckUsage(macro as MacroStatement):
		if len(macro.Arguments) < 1: return false
		if len(macro.Block.Statements) < 1: return false
		return true

	def ProcessEvaluatorArguments(block as Block, st as BinaryExpression, label as string):
		stmt = st.Right as MethodInvocationExpression
		if stmt:
			for i in range(stmt.Arguments.Count):
				refarg = stmt.Arguments[i] as ReferenceExpression
				if refarg:
					if refarg.ToString().StartsWith("?"):
						objVar as string
						if block.ContainsAnnotation(refarg.Name):
							objVar = block[refarg.Name] as string
						else:
							objVar = SafeIdentifierName(refarg.Name) + "_var"
							block[refarg.Name] = objVar
							block.Add(BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression(objVar), MethodInvocationExpression(ReferenceExpression('Variable'), L(refarg.Name))))
						
						stmt.Arguments.Replace(refarg, ReferenceExpression(objVar))

					elif refarg.ToString().StartsWith("$"):
						if IsValidReference(refarg.ToString()):
							stmt.Arguments.Replace(refarg, L(refarg.ToString()))
						else:
							Errors.Add(CompilerErrorFactory.CustomError(refarg.LexicalInfo, _errorMessage))
			
			block.Add(BinaryExpression(BinaryOperatorType.Assign, CreateReferenceExpression(label + '.Evaluator'), stmt ))
		else:
			Errors.Add(CompilerErrorFactory.CustomError(block.LexicalInfo, "The RHS of the Evaluator argument is not correct."))
