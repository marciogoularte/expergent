using System.Collections;
using Expergent.Runtime;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for CollectionofLists.
	/// </summary>
	/// 
	[ExpergentCollection("CollectionOfLists", typeof (LocationList), false, true, false)]
	public class CollectionofLists
	{
		private ArrayList _theLocs = new ArrayList();

		public CollectionofLists() : base()
		{
			_theLocs.Add(new LocationList());
			_theLocs.Add(new LocationList());
		}

		public ArrayList TheLocs
		{
			get { return _theLocs; }
		}

		public string TestString
		{
			get { return "teststring"; }
		}

	}
}