using System.Collections;
using Expergent.Runtime;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for ChildHash2.
	/// </summary>
	public class ChildHash2 : Hashtable
	{
		private string _id = "{FE479F05-D758-4fa5-A705-971C74521876}";

		[ExpergentParentID]
		public string ID
		{
			get { return _id; }
		}

		public ChildHash2()
		{
			LocationTestClass theLoc = new LocationTestClass("1", "location 1");
			this.Add(theLoc.ID, theLoc);

			theLoc = new LocationTestClass("2", "location 2");
			this.Add(theLoc.ID, theLoc);

			theLoc = new LocationTestClass("3", "location 3");
			this.Add(theLoc.ID, theLoc);
		}


	}
}