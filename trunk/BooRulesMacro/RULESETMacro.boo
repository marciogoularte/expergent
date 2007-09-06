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

class RULESETMacro(UsefulMacroBase):

    internal class MacroExpander(DepthFirstTransformer):
        _int as Method
        
        def constructor(prop as Method):
            _int = prop
        
        override def OnBlock(node as Block):
            for stmt as Statement in node.Statements:
                _int.Body.Add(stmt)

    override def Expand(macro as MacroStatement) as Statement:
        Init(macro)
        
        assert macro.Block.Statements.Count > 0
        //System.Diagnostics.Debugger.Launch()
        label = SafeIdentifierName(opt("Label", L("RuleSet${Helpers.Counter}")).ToString())
        eDate = opt("EffectiveDate", L(DateTime.Now.AddYears(-100).ToShortDateString())).ToString().Replace('\'', '')
        assert IsDate(eDate)
        tDate = opt("TerminationDate", L(DateTime.Now.AddYears(100).ToShortDateString())).ToString().Replace('\'', '')
        assert IsDate(tDate)
        AddClass(label,TypeMemberModifiers.Final, "IProductionProvider")
        
        AddMethod("constructor")
        AddStatement(Assign("label", L(label)))
        AddStatement(Assign("effectiveDate", Call("System.DateTime.Parse", (L(eDate)))  ))
        AddStatement(Assign("terminationDate", Call("System.DateTime.Parse", (L(tDate)))  ))
        
        AddFieldAndProperty("Label", "string", "label")
        AddFieldAndProperty("EffectiveDate", "System.DateTime", "effectiveDate")
        AddFieldAndProperty("TerminationDate", "System.DateTime", "terminationDate")
        
        AddGetProductionsMethod(macro.Block)
        AddGetAggregatorsMethod(macro.Block)
        AddGetMutexesMethod(macro.Block)
        AddGetOverridesMethod(macro.Block)
        
        return _block
        
    private def ProcessRuleBlock(block as Block, property as Property):
        assert block.Statements.Count == 0
        
    private def AddGetProductionsMethod(block as Block):
        listRef = GenericTypeReference()
        listRef.Name = "List"
        listRef.GenericArguments.Add(SimpleTypeReference("Production"))
        
        method = Method("GetProductions")
        method.ReturnType = listRef
        
        xc = GenericReferenceExpression()
        xc.GenericArguments.Add(SimpleTypeReference("Production"))
        xc.Target = ReferenceExpression('List')
        method.Body.Add(BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression('list'), MethodInvocationExpression(xc)))
        _class.Members.Add(method)
        
        bf = BlockFinder("RULE")
        bf.Visit(block)
        
        for childBlock as Block in bf.Blocks:
            mr = MacroExpander(method)
            mr.Visit(childBlock)
        
        method.Body.Add(ReturnStatement(ReferenceExpression('list')))
        
    private def AddGetAggregatorsMethod(block as Block):
        listRef = GenericTypeReference()
        listRef.Name = "List"
        listRef.GenericArguments.Add(SimpleTypeReference("Aggregator"))
        
        method = Method("GetAggregators")
        method.ReturnType = listRef
        
        xc = GenericReferenceExpression()
        xc.GenericArguments.Add(SimpleTypeReference("Aggregator"))
        xc.Target = ReferenceExpression('List')
        method.Body.Add(BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression('list'), MethodInvocationExpression(xc)))
        _class.Members.Add(method)
        
        bf = BlockFinder("AGGREGATOR")
        bf.Visit(block)
        
        for childBlock as Block in bf.Blocks:
            mr = MacroExpander(method)
            mr.Visit(childBlock)
        
        method.Body.Add(ReturnStatement(ReferenceExpression('list')))
        
    private def AddGetMutexesMethod(block as Block):
        listRef = GenericTypeReference()
        listRef.Name = "List"
        listRef.GenericArguments.Add(SimpleTypeReference("Mutex"))
        
        method = Method("GetMutexes")
        method.ReturnType = listRef
        
        xc = GenericReferenceExpression()
        xc.GenericArguments.Add(SimpleTypeReference("Mutex"))
        xc.Target = ReferenceExpression('List')
        method.Body.Add(BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression('list'), MethodInvocationExpression(xc)))
        _class.Members.Add(method)
        
        bf = BlockFinder("MUTEX")
        bf.Visit(block)
        
        for childBlock as Block in bf.Blocks:
            mr = MacroExpander(method)
            mr.Visit(childBlock)
        
        method.Body.Add(ReturnStatement(ReferenceExpression('list')))
        
    private def AddGetOverridesMethod(block as Block):
        listRef = GenericTypeReference()
        listRef.Name = "List"
        listRef.GenericArguments.Add(SimpleTypeReference("Override"))
        
        method = Method("GetOverrides")
        method.ReturnType = listRef
        
        xc = GenericReferenceExpression()
        xc.GenericArguments.Add(SimpleTypeReference("Override"))
        xc.Target = ReferenceExpression('List')
        method.Body.Add(BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression('list'), MethodInvocationExpression(xc)))
        _class.Members.Add(method)
        
        bf = BlockFinder("OVERRIDE")
        bf.Visit(block)
        
        for childBlock as Block in bf.Blocks:
            mr = MacroExpander(method)
            mr.Visit(childBlock)
        
        method.Body.Add(ReturnStatement(ReferenceExpression('list')))
