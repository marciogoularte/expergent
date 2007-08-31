using System;
using System.Text;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for LocationTestClass.
	/// </summary>
	/// 
	[Serializable]
	public class LocationTestClass
	{
		private string _id = string.Empty;
		private string _description = string.Empty;
		private int _intVal = 11;
		private double _dblVal = 1.0;
		private float _fltVal = 1.0F;

		public LocationTestClass(string id, string description)
		{
			_id = id;
			_description = description;
		}

		public string ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public int IntVal
		{
			get { return _intVal; }
			set { _intVal = value; }
		}

		public double DblVal
		{
			get { return _dblVal; }
		}

		public float FltVal
		{
			get { return _fltVal; }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(this.ID + ", ");
			sb.Append(this.Description + ", ");
			sb.Append(this.IntVal.ToString());

			return sb.ToString();

		}


//		public override bool Equals(object theObj)
//		{
//			//if(theObj is LocationTestClass && ((LocationTestClass)theObj)._id.Equals(this._id))
//				return true;
//
//			//return false;
//		}
	}
}