namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Child object with a recursive relationship
	/// </summary>
	public class ChildObjectTest
	{
		private ChildObjectTest _theChild = null;
		private string _id = "1";

		public ChildObjectTest()
		{
		}

		public ChildObjectTest NextChild
		{
			get { return _theChild; }
			set { _theChild = value; }
		}

		public string ID
		{
			get { return _id; }
		}

	}
}