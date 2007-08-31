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

internal class BlockFinder(DepthFirstVisitor):
	private _annotationName as string
	private _blocks = []
	
	def constructor(annotationName as string):
		_annotationName = annotationName
	
	override def OnBlock(node as Block):
		super.OnBlock(node)
		if node.ContainsAnnotation(_annotationName):
			_blocks.Add(node)
	
	Blocks as (Block):
		get:
			return _blocks.ToArray(typeof(Block))
