using System;

using SQLite;


namespace Bookkeeper
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }//id++
		public bool IsIncome { get; set; }
		public String Date { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public Account Account { get; set; }
		public int Amount { get; set; }
		public TaxRate TaxRate { get; set; }
		//public String Path { get; set; } //Kameran funkar inte...

	}
}
