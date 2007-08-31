using System;
using System.Collections.Generic;
using Expergent.Terms;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for Effector_SetAttribute.
	/// </summary>
	public class Effector_SetAttribute
	{
		/* Must include a default Constructor */

		public Effector_SetAttribute()
		{
		}

		public void setAttribute(List<Term> termList)
		{
			ObjectTerm objTerm = (ObjectTerm) termList[0];
			PurchaseOrder po = (PurchaseOrder) objTerm.Value;
			String item = ((StringTerm) termList[1]).Value;
			String discount = termList[2].ToString();

			String[] items = po.PurchaseItems;
//			int index;

			for (int i = 0; i < items.Length; ++i)
			{
				if (items[i].Equals(item))
				{
					po.Discounts[i] = discount;
				}
			}


		}
	}
}