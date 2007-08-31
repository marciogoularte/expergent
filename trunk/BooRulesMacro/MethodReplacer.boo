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

internal class MethodReplacer(DepthFirstTransformer):
	
	[getter(Expressions)]
	lst as List
	
	[getter(Assertions)]
	_ast as List
	
	[getter(MethodInvocationExpression)]
	methodInvocationExpression as MethodInvocationExpression
	
	def constructor():
		lst = []
		_ast = []
	
	override def OnBinaryExpression(node as BinaryExpression):
		_ast.Add(node.Clone())
		RemoveCurrentNode()
	
	override def OnMethodInvocationExpression(node as MethodInvocationExpression):
		methodInvocationExpression = node
		walker as ReferenceExpression = node.Target;
		while walker is not null:
			lst.Add(walker)
			if walker isa MemberReferenceExpression:
				target = (walker as MemberReferenceExpression).Target
				if target is not null:
					walker = target
			else:
				walker = null
