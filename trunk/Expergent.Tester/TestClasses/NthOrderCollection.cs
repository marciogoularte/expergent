using System.Collections;
using Expergent.Runtime;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for NthOrderCollection.
	/// </summary>
	public class NthOrderCollection
	{
		private ArrayList _lists = new ArrayList();

		private string _id;

		public NthOrderCollection() : base()
		{
			_lists.Add(new CollectionofLists());
			_id = "{D07E1768-3AC2-44d1-8183-02BF7AE70B67}";
			_lists.Add(new CollectionofLists());
		}

		public ArrayList CollOfLists
		{
			get { return _lists; }
		}

		[ExpergentParentID]
		public string ID
		{
			get { return this._id; }
		}
	}
}