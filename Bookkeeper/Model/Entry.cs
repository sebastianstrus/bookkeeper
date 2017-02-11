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
		public int TypeID { get; set; }//Account
		public int AccountID { get; set; }//Account
		public int Amount { get; set; }
		public int TaxRateID { get; set; }//TaxRate
		//public String Path { get; set; } //Kameran funkar inte...

	}
}
