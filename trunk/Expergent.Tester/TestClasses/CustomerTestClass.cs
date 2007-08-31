using System.Collections;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for CustomerTestClass.
	/// </summary>
	public class CustomerTestClass
	{
		public enum TestEnum
		{
			type1,
			type
		}

		private LocationList _theLocs = new LocationList();
		private ChildObjectTest _child = new ChildObjectTest();
		private ArrayList _stringList = new ArrayList();
		private CollectionofLists _coll = new CollectionofLists();
		private ChildHash _theHash = new ChildHash();
		private ChildNOCollection _theNOColl = new ChildNOCollection();
		private ChildNOCollection2 _theNOColl2 = new ChildNOCollection2();
		private ChildHash2 _theHash2 = new ChildHash2();
		private NthOrderCollection _theNested = new NthOrderCollection();
		private TestEnum _testEnum = TestEnum.type;
		private LocationTestClass _myLoc; // = new LocationTestClass();

		private string _id = "1";
		private int _intVal = 11;
		private double _dblVal = 1.0;
		private float _fltVal = 1.0F;

		public CustomerTestClass()
		{
			_child.NextChild = new ChildObjectTest();
/*
			_theLocs.Add(new LocationTestClass("", "location 1"));
			_theLocs.Add(new LocationTestClass("2", "location 2"));
			_theLocs.Add(new LocationTestClass("3", "location 3"));
			_theLocs.Add(new LocationTestClass("4", "location 4"));
*/
			_stringList.Add("first val");
			_stringList.Add("second val");
			_stringList.Add("third val");
			_theFirstNested.Add(new FirstOrderCollection());
			_theFirstNested.Add(new FirstOrderCollection());

			_myLoc = new LocationTestClass("2", "location1");

		}


		public LocationTestClass TheLocation
		{
			get { return _myLoc; }
		}

		/// <summary>
		/// return a collection object
		/// </summary>
		public LocationList Locations
		{
			get { return _theLocs; }
		}

		public ChildObjectTest Child
		{
			get { return _child; }
		}

		public string ID
		{
			get { return _id; }
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

		public ArrayList StringValues
		{
			get { return _stringList; }
		}

		public CollectionofLists TheColl
		{
			get { return this._coll; }
		}

		public ChildHash TheHash
		{
			get { return this._theHash; }
		}

		public ChildNOCollection TheChildNOColl
		{
			get { return this._theNOColl; }
		}

		public ChildNOCollection2 TheChildNOColl2
		{
			get { return this._theNOColl2; }
		}

		public ChildHash2 TheHash2
		{
			get { return this._theHash2; }
		}

		public NthOrderCollection TheNestedOne
		{
			get { return this._theNested; }
		}


		public TestEnum TheEnum
		{
			get { return _testEnum; }
		}

		private FirstOrderCollectionList _theFirstNested = new FirstOrderCollectionList();

		public FirstOrderCollectionList FirstOrder
		{
			get { return this._theFirstNested; }
		}


	}
}