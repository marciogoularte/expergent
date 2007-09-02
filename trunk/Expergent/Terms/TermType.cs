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

namespace Expergent.Terms
{
    ///<summary>
    /// Indicates the type of term.
    ///</summary>
    public enum TermType
    {
        ///<summary>Function
        ///</summary>
        Function = 1,
        ///<summary>Variable
        ///</summary>
        Variable = 2,
        ///<summary>String
        ///</summary>
        String = 3,
        ///<summary>Integer
        ///</summary>
        Integer = 4,
        ///<summary>Double
        ///</summary>
        Double = 5,
        ///<summary>ObjectReference
        ///</summary>
        ObjectReference = 6,
        ///<summary>Boolean
        ///</summary>
        Boolean = 7,
        ///<summary>DateTime
        ///</summary>
        DateTime = 8,
        ///<summary>
        ///</summary>List
        List,
        ///<summary>Null
        ///</summary>
        Null,
        ///<summary>Guid
        ///</summary>
        Guid,
        ///<summary>Collection
        ///</summary>
        Collection,
        ///<summary>ObjectRelation
        ///</summary>
        ObjectRelation,
        ///<summary>EntityObject
        ///</summary>
        EntityObject
    }
}