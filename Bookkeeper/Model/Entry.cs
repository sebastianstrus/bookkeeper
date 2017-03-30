using System;

using SQLite;


namespace Bookkeeper
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }
		public bool IsIncome { get; set; }
		public String Date { get; set; }
		public String Description { get; set; }
		public int TypeID { get; set; }//Account
		public int AccountID { get; set; }//Account
		public int Amount { get; set; }
		public int TaxRateID { get; set; }



		public override string ToString()
		{
			return string.Format("ID: {0}\nIsIncome: {1}\nDate: {2}\nDescription: {3}\nTypeID: {4}\nAccountID: {5}\nAmount: {6}\nTaxRateID {7}\n\n", 
			                     Id, IsIncome, Date, Description, TypeID, AccountID, Amount, TaxRateID);
		}

	}



}
