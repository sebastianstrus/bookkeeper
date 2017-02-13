using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;


namespace Bookkeeper
{
	class BookkeeperMenager
	{

		private static BookkeeperMenager instance;

		public static BookkeeperMenager Instance
		{
			get
			{
				if (instance == null)
				{

					instance = new BookkeeperMenager();
				}
				return instance;
			}
		}

		private List<Entry> entries;
		public List<Entry> Entries { get { return entries; } }

		private List<Account> incomeAccountList;
		public List<Account> IncomeAccountList { get { return incomeAccountList; } }
		private List<Account> expenseAccountList;
		public List<Account> ExpenseAccountList { get { return expenseAccountList; } }

		public List<Entry> IncomeEntries { get { return entries.Where(b => b.IsIncome).ToList(); } }//.Property.Value
		public List<Entry> ExpenseEntries { get { return entries.Where(b => !b.IsIncome).ToList(); } }

		private List<Account> moneyAccountList;
		public List<Account> MoneyAccountList { get { return moneyAccountList; } }

		private List<TaxRate> taxRateList;
		public List<TaxRate> TaxRateList { get { return taxRateList; } }

		private List<Account> accountList;
		public List<Account> AccountList { get { return accountList; } }



		public string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"\database.db";

		private BookkeeperMenager()
		{
			Account inAccount1 = new Account { Name = "Försäljning", Number = 3000, Type = "income" };
			Account inAccount2 = new Account { Name = "Försäljning av tjänster", Number = 3040, Type = "income" };
			Account inAccount3 = new Account { Name = "Rådgivning", Number = 5000, Type = "income" };

			Account exAccount1 = new Account { Name = "Övriga egna uttag", Number = 2013, Type = "expense" };
			Account exAccount2 = new Account { Name = "Förbrukningsmaterial", Number = 2222, Type = "expense" };
			Account exAccount3 = new Account { Name = "Reklam och PR", Number = 5900, Type = "expense" };

			Account mAccount1 = new Account { Name = "Kassa", Number = 1910, Type = "money" };
			Account mAccount2 = new Account { Name = "Företagskonto", Number = 1930, Type = "money" };
			Account mAccount3 = new Account { Name = "Egna insättningar", Number = 2018, Type = "money" };

			SQLiteConnection db = new SQLiteConnection(dbPath);
			db.CreateTable<Account>();
			if (db.Table<Account>().ToList().Count() == 0)
			{
				db.Insert(inAccount1);
				db.Insert(inAccount2);
				db.Insert(inAccount3);

				db.Insert(exAccount1);
				db.Insert(exAccount2);
				db.Insert(exAccount3);

				db.Insert(mAccount1);
				db.Insert(mAccount2);
				db.Insert(mAccount3);
			}

			accountList = db.Table<Account>().ToList();

			incomeAccountList = db.Table<Account>().Where(a => a.Type == "income").ToList();
			expenseAccountList = db.Table<Account>().Where(a => a.Type == "expense").ToList();
			moneyAccountList = db.Table<Account>().Where(a => a.Type == "money").ToList();

			db.CreateTable<TaxRate>();
			TaxRate tr1 = new TaxRate() { Value = 0.25, };
			TaxRate tr2 = new TaxRate { Value = 0.12, };
			TaxRate tr3 = new TaxRate { Value = 0.06, };
			TaxRate tr4 = new TaxRate { Value = 0.0, };

			if (db.Table<TaxRate>().ToList().Count() == 0)
			{
				db.Insert(tr1);
				db.Insert(tr2);
				db.Insert(tr3);
				db.Insert(tr4);
			}


			// tax rate list to spinner
			taxRateList = db.Table<TaxRate>().ToList();

			db.CreateTable<Entry>();
			if (db.Table<Entry>().ToList().Count() == 0)
			{
				db.Insert(new Entry()
				{
					IsIncome = true,
					Date = "3/5/2017",
					Description = "Dator till ITHS",
					TypeID = inAccount1.Number,
					AccountID = mAccount1.Number,
					Amount = 10000,
					TaxRateID = tr1.Id
					//Path = "sebastianstrus/projects/..." Kameran funkar inte
				});
				db.Insert(new Entry
				{
					IsIncome = false,
					Date = "4/5/2017",
					Description = "Mat på ICA",
					TypeID = exAccount1.Number,
					AccountID = mAccount2.Number,
					Amount = 200,
					TaxRateID = tr2.Id
					//Path = 
				});
				db.Insert(new Entry
				{
					IsIncome = false,
					Date = "5/5/2017",
					Description = "Ny domän",
					TypeID = exAccount3.Number,
					AccountID = mAccount2.Number,
					Amount = 5000,
					TaxRateID = tr3.Id
					//Path = "sebastianstrus/projects/" 
				});
			}
			entries = db.Table<Entry>().ToList();
		}

		public static void AddEntry(Entry e)
		{
			SQLiteConnection db = new SQLiteConnection(instance.dbPath);
			db.Insert(e);
			instance.entries = db.Table<Entry>().ToList();
			db.Close();
		}

		public static void UpdateEntry(Entry e, int entryId)
		{
			SQLiteConnection db = new SQLiteConnection(instance.dbPath);
			Entry temp = db.Get<Entry>(entryId);

			temp.IsIncome = e.IsIncome;
			temp.Date = e.Date;
			temp.Description = e.Description;
			temp.TypeID = e.TypeID;
			temp.AccountID = e.AccountID;
			temp.Amount = e.Amount;
			temp.TaxRateID = e.TaxRateID;

			db.Update(temp);
			instance.entries = db.Table<Entry>().ToList();
			db.Close();
		}

		public string GetTaxReport()
		{
			double incomeTax = 0.0;
			double expenseTax = 0.0;

			foreach(Entry entry in entries)
			{
				if (entry.IsIncome)
				{
					TaxRate taxRate = taxRateList[entry.TaxRateID - 1];
					double value = taxRate.Value;
					incomeTax += Math.Round(double.Parse(entry.Amount.ToString()) -double.Parse(entry.Amount.ToString()) / (1.0 + value), 2);
				}
				else
				{
					TaxRate taxRate = taxRateList[entry.TaxRateID - 1];
					double value = taxRate.Value;
					expenseTax += Math.Round(double.Parse(entry.Amount.ToString()) - double.Parse(entry.Amount.ToString()) / (1.0 + value), 2);
				}
			}
			string taxReport = "Betald moms för alla inkomster är " + incomeTax + "kr.\nBetald moms för alla utgifter är  " + expenseTax+"kr.";
			return taxReport;
		}

		public string GetAccountReport()
		{
			string accountReport = "";

			string incomeAccountReport = "";
			string expenseAccountReport = "";
			string moneyAccountReport = "";

			foreach (Account account in incomeAccountList)
			{
				double total = 0.0;
				string someEntries = "";

				foreach (Entry entry in entries)
				{
					if (entry.TypeID == account.Number)
					{
						someEntries += "\n" + entry.Date + " - " + entry.Description + ", " + entry.Amount + "kr";
						total += entry.Amount;
					}
					   
				}
				incomeAccountReport += "*** " + account + " (total: " + total + "kr)"+ someEntries +"\n***\n";
				
			}

			foreach (Account account in expenseAccountList)
			{

				double total = 0.0;
				string someEntries = "";
				foreach (Entry entry in entries)
				{
					if (entry.TypeID == account.Number)
					{
						someEntries += "\n" + entry.Date + " - " + entry.Description + ", -" + entry.Amount + "kr";
						total += entry.Amount;
					}
				}

				string minus = total > 0 ? "-" : "";
				expenseAccountReport += "*** " + account + " (total: " + minus + total + "kr)"+ someEntries +"\n***\n";
			}

			foreach (Account account in moneyAccountList)
			{
				double total = 0.0;
				string someEntries = "";
				foreach (Entry entry in entries)
				{
					if (entry.AccountID == account.Number)
					{
						if (entry.IsIncome)
						{
							someEntries += "\n" + entry.Date + " - " + entry.Description + ", " + entry.Amount + "kr";
							total += entry.Amount;
						}
						else
						{
							someEntries += "\n" + entry.Date + " - " + entry.Description + ", -" + entry.Amount + "kr";
							total -= entry.Amount;
						}
					}
				}
				moneyAccountReport += "*** " + account + " (total: " + total + "kr)" + someEntries + "\n***\n";
			}

			accountReport = incomeAccountReport + "\n\n" + expenseAccountReport + "\n\n" + moneyAccountReport;
			return accountReport;
		}
	}
}



