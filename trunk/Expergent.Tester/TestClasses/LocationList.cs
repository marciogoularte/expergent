using System;
using System.Collections;
using System.Threading;
using Expergent.Runtime;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for LocationList.
	/// </summary>
	/// 
	[ExpergentCollection("LocationList", typeof (LocationTestClass), false, true, false)]
	public class LocationList : ArrayList
	{
		private string _id = "{BC4A21D0-85FF-45b8-82CB-11DEC1CEC070}";

		[ExpergentParentID]
		public string ID
		{
			get { return _id; }
		}

		public LocationList() : base()
		{
			//_id = System.Guid.NewGuid().ToString();
			this.Add(new LocationTestClass(DateTime.Now.Millisecond.ToString(), "location 1"));
			Thread.Sleep(10);
			this.Add(new LocationTestClass(DateTime.Now.Millisecond.ToString(), "location 2"));
			Thread.Sleep(10);
			this.Add(new LocationTestClass(DateTime.Now.Millisecond.ToString(), "location 3"));
			Thread.Sleep(10);
			this.Add(new LocationTestClass(DateTime.Now.Millisecond.ToString(), "location 4"));
		}
	}
}