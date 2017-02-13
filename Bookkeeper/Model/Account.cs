using System;
using SQLite;

namespace Bookkeeper
{
	public class Account
	{
		public string Name { get; set; }
		[PrimaryKey]
		public int Number { get; set; }
		public string Type { get; set; }


		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Number);
		}
	}
}

