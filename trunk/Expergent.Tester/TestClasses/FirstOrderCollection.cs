using System.Collections;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for FirstOrderCollection.
	/// </summary>
	public class FirstOrderCollection
	{
		private ArrayList _theLists = new ArrayList();
		private string _id;

		public FirstOrderCollection()
		{
			_theLists.Add(new NthOrderCollection());
			_theLists.Add(new NthOrderCollection());
			_id = "{BD9A5ECB-EFBB-447d-B6E8-0BF7BA28CF2C}";
		}

		public ArrayList NthOrder
		{
			get { return _theLists; }
		}

		//[Expergent.Tester.Runtime.ExpergentParentID]
		public string ID
		{
			get { return _id; }
		}

	}
}