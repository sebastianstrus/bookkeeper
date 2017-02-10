using System;
using SQLite;

namespace Bookkeeper
{
	public class TaxRate
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }//id++

		public double Value { get; set; }

		public override string ToString()
		{
			return string.Format(Value * 100 + "%");
		}
	}
}
