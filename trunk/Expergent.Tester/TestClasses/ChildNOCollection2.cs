using System;
using System.Collections;
using System.Collections.Specialized;
using Expergent.Runtime;

namespace Expergent.Tester.TestClasses
{
	/// <summary>
	/// Summary description for ChildNOCollection.
	/// </summary>
	public class ChildNOCollection2 : NameObjectCollectionBase
	{
		private string _id = "{256C8972-E7C2-42a0-8154-9C7EC7D5D5EB}";

		public ChildNOCollection2()
		{
			LocationTestClass theLoc = new LocationTestClass("1", "location 1");
			this.BaseAdd(theLoc.ID, theLoc);

			theLoc = new LocationTestClass("2", "location 2");
			this.BaseAdd(theLoc.ID, theLoc);

			theLoc = new LocationTestClass("3", "location 3");
			this.BaseAdd(theLoc.ID, theLoc);
		}

		[ExpergentParentID]
		public string ID
		{
			get { return _id; }
		}

		new public IEnumerator GetEnumerator()
		{
			return new ListEnumerator(this);
		}

		#region IList Implentation

		public object this[int index]
		{
			get { return BaseGet(index); }
			set
			{
			}
		}

		/// <summary>
		/// *****Not Implemented, Primarily for IList support******
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(object value)
		{
			return true;
		}

		/// <summary>
		/// *****Not Implemented, Primarily for IList support******
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(object value)
		{
			return 0;
		}

		/// <summary>
		/// *****Not Implemented, Primarily for IList support******
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public void Insert(int index, object value)
		{
		}

		/// <summary>
		/// *****Not Implemented, Primarily for IList support******
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public void Remove(object value)
		{
		}

		/// <summary>
		/// Removes object at specified index
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public void RemoveAt(int value)
		{
			base.BaseRemoveAt(value);
		}

		new public bool IsReadOnly
		{
			get { return base.IsReadOnly; }
		}

		/// <summary>
		/// *****Not Implemented, Primarily for IList support******
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool IsFixedSize
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the objects from the collection and clears the errors.
		/// </summary>
		public void Clear()
		{
			BaseClear();
		}

		#endregion

		#region classes

		/// <summary>
		/// Our Private implementation of GetEnumerator().
		/// Using this to add collection support to NameObjectCollectionBase.
		/// </summary>
		/// 
		protected class ListEnumerator : IEnumerator
		{
			private ChildNOCollection2 _list;
			private int _current = -1;


			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="list">a WCCodeList object</param>
			public ListEnumerator(ChildNOCollection2 list)
			{
				_list = list;
			}


			/// <summary>
			/// Moves us to the next object in the collection
			/// </summary>
			/// <returns>boolean indication when we have reached the end</returns>
			public bool MoveNext()
			{
				//Hack for now. This allows remoting to work even though there is an error
				try
				{
					++_current;
					return (_current >= _list.Count) ? false : true;
				}
				catch (Exception ex)
				{
					Console.WriteLine("ERROR ACC.Shared.SharedNameObjCollBase.MoveNext");
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
				return false;
			}


			/// <summary>
			/// Property which returns current object in the collection
			/// </summary>
			public object Current
			{
				get { return _list[_current]; }
			}


			/// <summary>
			/// Returns us to the start of the collection.
			/// </summary>
			public void Reset()
			{
				_current = -1;
			}
		}

		#endregion
	}
}