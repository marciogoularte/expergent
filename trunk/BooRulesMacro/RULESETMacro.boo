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
        _int as Property
        
        def constructor(prop as Property):
            _int = prop
        
        override def OnBlock(node as Block):
            for stmt as Statement in node.Statements:
                _int.Getter.Body.Add(stmt)

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
        
        ruleListProperty = Property("RuleList")
        listRef = GenericTypeReference()
        listRef.Name = "List"
        listRef.GenericArguments.Add(SimpleTypeReference("Production"))
        ruleListProperty.Type = listRef
        ruleListProperty.Getter = Method("get")
        
        
        xc = GenericReferenceExpression()
        xc.GenericArguments.Add(SimpleTypeReference("Production"))
        xc.Target = ReferenceExpression('List')
        ruleListProperty.Getter.Body.Add(BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression('list'), MethodInvocationExpression(xc)))
        _class.Members.Add(ruleListProperty)
        
        block = macro.Block

        mr = MacroExpander(ruleListProperty)
        mr.Visit(block)
        
        ruleListProperty.Getter.Body.Add(ReturnStatement(ReferenceExpression('list')))

        return _block
        
    private def ProcessRuleBlock(block as Block, property as Property):
        assert block.Statements.Count == 0
