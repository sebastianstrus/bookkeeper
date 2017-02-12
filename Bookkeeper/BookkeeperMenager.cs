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
		public List<Account> IncomeTypeArray { get { return incomeAccountList; } }
		private List<Account> expenseAccountList;
		public List<Account> ExpenseTypeArray { get { return expenseAccountList; } }

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


			//SQLiteConnection db = new SQLiteConnection(dbPath + @"\database.db");
			db.CreateTable<TaxRate>();
			//db.DeleteAll<TaxRate>();
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
					//Path = "sebastianstrus/projects/" Kameran funkar inte
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
					//Path = "sebastianstrus/projects/" Kameran funkar inte
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
					//Path = "sebastianstrus/projects/" Kameran funkar inte
				});
			}
			entries = db.Table<Entry>().ToList();
			//BookkeeperMenager.UpdateEntryList();

		}

		public static void AddEntry(Entry e)
		{
			SQLiteConnection db = new SQLiteConnection(instance.dbPath);
			db.Insert(e);
			instance.entries = db.Table<Entry>().ToList();

		}

		public static void UpdateEntry(Entry e, int entryId)
		{
			SQLiteConnection db = new SQLiteConnection(instance.dbPath);
			Entry temp = db.Get<Entry>(entryId);
			temp = e;
			db.Update(temp);
			instance.entries = db.Table<Entry>().ToList();

		}

		/*public static void UpdateEntryList()
		{
			SQLiteConnection db = new SQLiteConnection(BookkeeperMenager.Instance.dbPath);
			instance.entries = db.Table<Entry>().ToList();
		}*/

		/*public static void UppdateEntry(Entry e)
		{
			SQLiteConnection db = new SQLiteConnection(BookkeeperMenager.Instance.dbPath);
			db.Update(e);
			instance.entries = db.Table<Entry>().ToList();
		}*/

		public string GetTaxReport()
		{
			return "abc";
		}
	}
}

/*
Listor:
- IncomeAccounts
- ExpenseAccounts
- MoneyAccounts
- TaxRates
- Entries
*/

/*
Databas-kopplingar i dataklasserna
Ni ska utöka era klasser Entry, Account och TaxRate med annoterad information om i vilka
tabeller de ska sparas.Ni kan ha en tabell per klass, dvs Entries, Accounts och TaxRates.
Till Entry och TaxRate kan ni lägga till en Id-property av typen int, som ni mappar till
_id-kolumnen och gör autoinkrementerande.
Till Account kan ni använda Number som primary key, eftersom det ändå är unikt för alla konton.
Då behöver ni inte kalla den _id, utan låt kolumnen namnges automatiskt.Ni kan också fundera
på hur olika Account-typer ska skiljas åt - hur vet man vilka som är IncomeAccounts,
ExpenseAccounts och MoneyAccounts?
*/


/*
Databas-kopplingar i BookkeeperManager
När man skapar BookkeeperManager,
vill ni först etablera en anslutning till databasen.Sedan ska ni skapa upp tabellerna om de inte
redan finns. Om tabellen av Entry innehåller 0 poster, gör det ingenting.Det betyder att
användaren inte har skapat upp något än. Däremot, om tabellerna av Account och TaxRate
innehåller 0 poster, innebär det att det är första gången appen startas.Då kan ni lägga in
exempel-data i er databas. (Gör bara detta om antal poster är 0 dock - annars får ni duplicerade
items i era spinners!)
När getter på någon lista anropas,
T.ex.IncomeAccounts, så ska ni göra ett query mot databasen, och hämta alla
IncomeAccounts.Nu lagrar ni inte längre listorna själva, utan informationen lagras i tabeller.
Då måste ni ta en tabell, göra ett query på den och konvertera resultatet till en lista som ni
returnerar.
(Eftersom er listas Adapter förväntar sig en lista, är det enklast att här köra.ToList()-metoden.
Då behöver ni inte förändra i er adapter.)
Checklista del 2
- “Visa alla händelser”­vyn
- Ny activity
- Skapa också er egen Adapter, som bygger på listor
- Skapa också er egen list item XML
- Databas-kopplingar
- Utöka era Entry, Account och TaxRate-klasser
- Tänk på: vilka egenskaper som ska vara primary keys
- Tänk på: hur ska man skilja olika Accounts från varandra?
- Förändra er BookkeperManager
- Ska inte längre lagra informationen i listor, utan i databas-tabeller
TODO:
- Konstruktor: skapa upp tabeller om de inte redan finns, och fyll
XxxAccounts/TaxRates med information om de är tomma
- Entries, XxxAccounts, TaxRates: gör ett query på respektive tabell, och
konvertera resultatet till en lista
VG del 2
Gör så att man kan klicka på en händelse i listan, och komma till en “redigera”­vy för den här
händelsen.Den ska se ut precis som “Ny händelse”, men den ska ha all information om den
klickade händelsen ifylld.När man klickar på spara, ska den aktuella händelsen uppdateras.
Ingen ny ska alltså skapas!
För att åstadkomma detta behöver ni:
- Uppdatera “Visa alla händelser” (klick på list item)
- Uppdatera er backend(ska kunna uppdatera en Entry.Hur går detta till i SQLite.net?)
- Skapa en Redigera-vy.
- Tips: Uppdatera er vy för “Ny händelse”, och skicka med extra information om
den ska gå in i “redigera”­läge.Mycket av koden är identisk; det är
uppskapandet och sparandet som varierar beroende på om vi är inne i
“ny”­läge eller “existerande”­läge.
*/