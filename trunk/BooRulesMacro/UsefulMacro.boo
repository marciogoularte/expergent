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

#region attribution
// Portions of this code were originally written by Steve Donovan and its use here does not imply otherwise
#endregion

namespace UsefulMacro

import Boo.Lang.Compiler
import Boo.Lang.Compiler.Ast
import Boo.Lang.Compiler.Ast.AstUtil
import Boo.Lang.Compiler.Ast.Visitors
import Boo.Lang.Compiler.TypeSystem

callable ReferenceFilter(s as string) as Expression

# useful binary operators
def Plus(e1 as Expression, e2 as Expression):
	return BinaryExpression(BinaryOperatorType.Addition,e1,e2)
	
def Minus(e1 as Expression, e2 as Expression):
	return BinaryExpression(BinaryOperatorType.Subtraction,e1,e2)
	
def Divide(e1 as Expression, e2 as Expression):
	return BinaryExpression(BinaryOperatorType.Division,e1,e2)

def Multiply(e1 as Expression, e2 as Expression):
	return BinaryExpression(BinaryOperatorType.Multiply,e1,e2)
	
def LessThan(e1 as Expression, e2 as Expression):
	return BinaryExpression(BinaryOperatorType.LessThan,e1,e2)
	
def Assign(e1 as Expression, e2 as Expression):
	return BinaryExpression(BinaryOperatorType.Assign,e1,e2)

def Assign(name as string, e as Expression):
	return Assign(ReferenceExpression(name),e)

# only one unary operator so far!
def Increment(e as Expression):
	return UnaryExpression(UnaryOperatorType.Increment,e)
	
# useful shortcuts for reference expressions and literals
def RE(name as string):
	return ReferenceExpression(name)
	
def CRE(nm as string):
	return CreateReferenceExpression(nm)
	
def L(x as double):
	return DoubleLiteralExpression(x)
	
def L(i as int):
	return IntegerLiteralExpression(i)
	
def L(x as string):
	return StringLiteralExpression(x)
	
def ArrayLiteral(*items as (Expression)):
	arr = ArrayLiteralExpression()
	for item in items:
		arr.Items.Add(item)
	return arr
	
def Call(fun as string, *args as (Expression)):
	mie = MethodInvocationExpression(Target: CRE(fun))
	for arg in args:
		mie.Arguments.Add(arg)
	return mie
	
def CapInit(s as string):
	if not char.IsUpper(s[0]):
		s = s[0:1].ToUpper() + s[1:]
	return s
	
def Invoke(e1 as Expression, e2 as Expression):
	return CreateMethodInvocationExpression(e1,e2)	
	
def ConstructArray(type as string, e as Expression):
	return Call("Boo.Lang.Builtins.array",RE(type),e)
	
def IndexArray(target as Expression, index as Expression):
	se = SlicingExpression(Target: target)
	se.Indices.Add(Slice(index))
	return se
	
def PropGet(e as Expression, prop as string):
	mre = MemberReferenceExpression(Name: "get_"+prop, Target: e)	
	mie = MethodInvocationExpression(Target: mre)
	return mie
	
def LengthOf(e as Expression):
	return PropGet(e,"Length")


class NameChanger(DepthFirstTransformer):
	[property(Filter)]
	_filter as ReferenceFilter
	
	override def OnReferenceExpression(node as ReferenceExpression):
		transformedExpr = _filter(node.Name)
		if transformedExpr != null:
			ReplaceCurrentNode(transformedExpr)
			
def TransformReference(n as Node, filter as ReferenceFilter):
	nc = NameChanger(Filter: filter)
	nc.Visit(n)

class UsefulMacroBase(AbstractAstMacro):
	_block as Block
	_blockStack as List
	_args as ExpressionCollection
	static _public = TypeMemberModifiers.Public
	_class as ClassDefinition
	_method as Method
	_errorMessage as string
	
	# nothing useful, just to switch off the warning message
	override def Expand(macro as MacroStatement):
		pass	
	
	static write = CreateReferenceExpression("System.Console.Write")
	static writeline = CreateReferenceExpression("System.Console.WriteLine")
	
	def Assignments(node as MacroStatement):
		for a in node.Arguments:
			if a isa BinaryExpression:
				expr = a as BinaryExpression
				if expr.Operator == BinaryOperatorType.Assign:
					yield expr
					
	def GetAssignment(node as MacroStatement, name as string):
		for e in Assignments(node):
			if e.Left.ToString() == name:
				return e
		return null
		
	def opt(node as MacroStatement, name as string, default as Expression):
		e = GetAssignment(node, name)
		return e.Right if e
		return default
		
	def Assignments():
		for a in _args:
			if a isa BinaryExpression:
				expr = a as BinaryExpression
				if expr.Operator == BinaryOperatorType.Assign:
					yield expr
					
	def GetAssignment(name as string):
		for e in Assignments():
			if e.Left.ToString() == name:
				return e
		return null
		
	def opt(name as string, default as Expression):
		e = GetAssignment(name)
		return e.Right if e
		return default
				
	def AssignStmtDefault(name as string, default as Expression):
		e = GetAssignment(name)
		if e:
			_block.Add(e)
		else:
			AssignStmt(name,default)
	
	def PushBlock():
		_blockStack.Push(_block)
		_block = Block()
		
	def PopBlock():
		ret = _block
		_block = _blockStack.Pop()
		return ret
		
	def AssignStmt(name as string, e as Expression):
		_block.Add(Assign(name,e))
		
	def AssignStmt(e1 as Expression, e2 as Expression):
		_block.Add(Assign(e1,e2))
		
	def Add(e as Expression):
		_block.Add(e)
		
	def Add(s as Statement):
		_block.Add(s)
		
	def DoMethod(e1 as Expression, e2 as Expression):
		Add(Invoke(e1,e2))
		
	def Display(e as Expression):
		DoMethod(write,e)
		
	def DisplayLine():
		DoMethod(writeline,L(""))
		
		
	def AddField(nm as string, t as string):
		f = Field(Modifiers:TypeMemberModifiers.Private,Name:nm)
		f.Type = SimpleTypeReference(t) 
		_class.Members.Add(f)
		
	def AddClass(nm as string, base as string):
		_class = ClassDefinition(Modifiers:_public,Name:nm)
		_class.BaseTypes.Add(SimpleTypeReference(Name: base))
		Context.CompileUnit.Modules[0].Members.Add(_class)

	def AddClass(nm as string, modifier as TypeMemberModifiers, base as string):
		_class = ClassDefinition(Modifiers:modifier,Name:nm)
		_class.BaseTypes.Add(SimpleTypeReference(Name: base))
		Context.CompileUnit.Modules[0].Members.Add(_class)
	
	def IsValidReference(name as string) as bool:
		if string.IsNullOrEmpty(name):
			raise "name parameter is null or empty."
		if name.StartsWith("$"):
			name = name[1:]
		names = name.Split(*(".".ToCharArray()))
		if names.Length == 1:
			result = Context.NameResolutionService.Resolve(names[0]) is not null
			if not result:
				_errorMessage = "'${names[0]}' is not a valid class."
			return result
		className = names[0]
		props = List(names[1:])
		while props.Count > 0:
			e = Context.NameResolutionService.Resolve(className)
			if e is null:
				_errorMessage = "'${className}' is not a valid class."
				return false
			p = Context.NameResolutionService.ResolveProperty(e, props[0].ToString())
			if p is null:
				m = Context.NameResolutionService.ResolveMethod(e, props[0].ToString())
				if m is null:
					_errorMessage = "The member '${props[0].ToString()}' is not valid for the class '${className}'."
					return false
				else:
					className = m.ReturnType.Name
					props = props[1:]
			else:
				if p isa InternalProperty:
					node = (p as InternalProperty).Node as Property
					className = node.Type.ToString()
					props = props[1:]
				elif p isa ExternalProperty:
					className = (p as ExternalProperty).PropertyInfo.PropertyType.Name
					props = props[1:]
				else:
					raise p.ToString()
		
		return true
	
	def AddMethod(nm as string, *parms as ((string))):
		if nm == "constructor":
			_method = Constructor()
		else:
			_method = Method()
		_method.Name = nm
		_method.Modifiers = _public		
		if len(parms) > 0:
			for p  in parms:
				_method.Parameters.Add(ParameterDeclaration(p[0],SimpleTypeReference(p[1])))
		_class.Members.Add(_method)
	
	def AddProperty(nm as string, type as string, field as string):
        table = Property(nm)
        table.Type = SimpleTypeReference(type)
        table.Modifiers = TypeMemberModifiers.Public
        table.Getter = Method("get")
        table.Getter.Body.Add(ReturnStatement(RE(field)))
        table.Setter = Method("set")
        table.Setter.Body.Add(Assign(field, RE("value")))
        _class.Members.Add(table)
        
    def AddFieldAndProperty(property as string, type as string, field as string):
        AddField(field, type)
        AddProperty(property, type, field)
	
	def Override():
		_method.Modifiers = _method.Modifiers | TypeMemberModifiers.Override
		
	def AddStatement(stmt as Statement):
		_method.Body.Statements.Add(stmt)
		
	def AddStatement(expr as Expression):
		AddStatement(ExpressionStatement(expr))
		
	def AddReturnStatement(e as Expression):
		AddStatement(ReturnStatement(e))
		
	def GetParentMacroByName(name as string, node as Node) as MacroStatement:
		parentNode = node as MacroStatement
		while parentNode is not null:
			if parentNode.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase):
				return parentNode as MacroStatement
			parentNode = GetParentMacro(parentNode)
		return null
	
	def GetParentMacro(node as Node):
		if node.ParentNode is not null:
			return node.ParentNode.ParentNode as MacroStatement
		else:
			return null

	def Init(macro as MacroStatement):
		_block = Block()
		_blockStack = []
		_args = macro.Arguments
		
# a macro which constructs macros! The body will be inserted between a standard 'Init(macro)' and 'return _block'.
# arguments are also available as '_args[0]', etc.
class MacroMacro(UsefulMacroBase):
	override def Expand(macro as MacroStatement):
		Init(macro)
		name = CapInit(macro.Arguments[0].ToString())+"Macro"
		AddClass(name,"UsefulMacroBase")
		AddMethod("Expand",('stmt','Boo.Lang.Compiler.Ast.MacroStatement')); Override()
		AddStatement(Call("Init",RE("stmt")))
		AddStatement(macro.Block)
		AddStatement(ReturnStatement(RE("_block")))
		return _block
		
