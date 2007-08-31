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
// Portions of this code were originally written by Andrew Davey as part of the Specter project (http://specter.sourceforge.net/)
// Its use here does not imply otherwise, nor does it invalidate his copyright
#endregion

namespace BooRulesMacro

import System
import System.Text.RegularExpressions

import Boo.Lang.Compiler 
import Boo.Lang.Compiler.Ast 
import Boo.Lang.Compiler.Ast.AstUtil
import Boo.Lang.Compiler.IO

class Helpers:
	static counter as int
	
	static Counter as int:
		get:
			return ++counter

class Consts():
	public static final RULENAME = "boorules.rulename"

static def SafeIdentifierName(input as string):
	input = ReplaceOperatorsWithEnglish(input)
	input = @/ ([a-z])/.Replace(input, 
		{ m as Match |
			return m.Groups[1].Value.Trim().ToUpper()
		}
	)
	input = @/[^_a-zA-Z0-9]/.Replace(input, string.Empty)
	if /^[^_a-zA-Z]/.IsMatch(input):
		input = "_" + input
	
	return input

static def SafeIdentifierName(input as Expression):
	if input isa StringLiteralExpression:
		return SafeIdentifierName((input as StringLiteralExpression).Value)
	return SafeIdentifierName(input.ToString())

static def ReplaceOperatorsWithEnglish(message as string):
	operators = [ 
				("==" , "equal"),
				("!=" , "not equal"),
				(">=" , "greater than or equal"),
				("<=" , "less than or equal"),
				("<"  , "less than"),
				(">"  , "greater than")
				]
	for op as (string) in operators:
		message = message.Replace(op[0], op[1])
		
	return message
	
static def IsDate(dt as string) as bool:
    try:
        DateTime.Parse(dt)
        return true
    except:
        return false
