using System.Collections;
using Expergent.Runtime;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for FirstOrderCollectionList.
	/// </summary>
	public class FirstOrderCollectionList : ArrayList
	{
		private string _id = "{8BB723E7-9B7E-44b3-A98A-096F01770493}";

		public FirstOrderCollectionList() : base()
		{
		}

		[ExpergentParentID]
		public string ID
		{
			get { return _id; }
		}
	}
}