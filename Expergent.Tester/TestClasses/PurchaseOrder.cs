using System;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for PurchaseOrder.
	/// </summary>
	public class PurchaseOrder
	{
		private String[] purchaseItems;
		private String[] purchaseItems1;
		private String[] discounts;
		private int intVal = 0;

		#region constructors

		public PurchaseOrder(String[] purchaseItems, String[] discounts)
		{
			this.purchaseItems = purchaseItems;
			this.purchaseItems1 = purchaseItems;
			this.discounts = discounts;
		}

		#endregion

		#region properties

		public String[] PurchaseItems
		{
			get { return purchaseItems; }
			set { purchaseItems = value; }
		}

		public String[] Discounts
		{
			get { return discounts; }
			set { discounts = value; }
		}

		public int IntVal
		{
			get { return this.intVal; }
			set { intVal = value; }
		}

		#endregion

		#region methods

		public override String ToString()
		{
			String str = string.Empty;

			for (int i = 0; i < purchaseItems.Length; ++i)
			{
				str += "purchaseItems: " + purchaseItems[i] + " ";
				str += "discount: " + discounts[i] + " ";
			}
			return str;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override bool Equals(Object obj)
		{
			PurchaseOrder po = (PurchaseOrder) obj;
			String[] purchaseItems = po.PurchaseItems;
			String[] discounts = po.Discounts;

			for (int i = 0; i < this.purchaseItems.Length; ++i)
			{
				if (this.purchaseItems[i] != null && purchaseItems[i] != null)
				{
					if (!this.purchaseItems[i].Equals(purchaseItems[i]))
						return false;
				}
				if (this.discounts[i] != null && discounts[i] != null)
				{
					if (!this.discounts[i].Equals(discounts[i]))
						return false;
				}
			}
			return true;
		}

		#endregion
	}
}