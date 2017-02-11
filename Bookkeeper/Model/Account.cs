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


/*
 * Jag tror att för att kunna få ut t.ex. ett Account-objekt från en spinner så måste du se till 
 * att Account-klassen ärver från Java.Lang.Object, och när du hämtar objektet från en spinner 
 * t.ex med spinner.SelectedItem så behöver du typecasta det. Typ såhär: Account a = (Account) spinner.SelectedItem;
 * */