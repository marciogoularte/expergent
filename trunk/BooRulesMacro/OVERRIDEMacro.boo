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

class OVERRIDEMacro(UsefulMacroBase):
    
    static final Usage = "Usage: OVERRIDE Label='somelabel', Winner='win', Loser='loss':"
    
    override def Expand(macro as MacroStatement) as Statement:
        Init(macro)
        if not CheckUsage(macro):
            Errors.Add(CompilerErrorFactory.CustomError(macro.LexicalInfo, Usage))
            return null
        //System.Diagnostics.Debugger.Launch()
        label as string
        if macro.ContainsAnnotation(Consts.OVERRIDENAME):
            label = macro[Consts.OVERRIDENAME] as string
        else:
            label = SafeIdentifierName(opt("Label", L("Override${Helpers.Counter}")).ToString())
            macro[Consts.OVERRIDENAME] = label
        
        block = Block()
        block.Annotate("OVERRIDE")
        rule = BinaryExpression(BinaryOperatorType.Assign, ReferenceExpression(label), MethodInvocationExpression(ReferenceExpression('Override')))
        block.Add(rule)
        block.Add(BinaryExpression(BinaryOperatorType.Assign, CreateReferenceExpression(label + '.Label'), L(label) ))
        block.Add(BinaryExpression(BinaryOperatorType.Assign, CreateReferenceExpression(label + '.Winner'), opt("Winner", L("Winner")) ))
        block.Add(BinaryExpression(BinaryOperatorType.Assign, CreateReferenceExpression(label + '.Loser'), opt("Loser", L("Loser")) ))
        
        for stmt as Statement in macro.Block.Statements:
            block.Add(stmt)
            
        mie = MethodInvocationExpression()
        mie.Arguments.Add(ReferenceExpression(label))
        mie.Target = CreateReferenceExpression("list.Add")
        block.Add(mie)
        return block
        
    def CheckUsage(macro as MacroStatement):
        if len(macro.Arguments) != 3: return false
        if len(macro.Block.Statements) == 1: return false
        return true
