using System;
using System.Collections;
using System.Threading;
using Expergent.Runtime;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for ChildHash.
	/// </summary>
	public class ChildHash : Hashtable
	{
		private string _id = "{0958C0A6-6B46-42dd-B2D7-BC6914ED11E6}";

		[ExpergentParentID]
		public string ID
		{
			get { return _id; }
		}

		public ChildHash()
		{
			base.Add("test1", DateTime.Now.Millisecond.ToString());
			Thread.Sleep(87);
			base.Add("test2", DateTime.Now.Millisecond.ToString());
			Thread.Sleep(5);
			base.Add("test3", DateTime.Now.Millisecond.ToString());
		}
	}
}