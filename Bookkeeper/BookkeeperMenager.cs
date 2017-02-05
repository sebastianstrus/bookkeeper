using System;
using System.Collections.Generic;
using System.Linq;

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

		public List<Entry> ImportantEntries { get { return entries.Where(b => b.IsImportant).ToList(); } }
		public List<Entry> NotImportantEntries { get { return entries.Where(b => !b.IsImportant).ToList(); } }


		private BookkeeperMenager()
		{
			entries = new List<Entry>();

			entries.Add(new Entry//Kind, Date, Amount, IsImportant

			{//must show date, description and brutto
				Kind = "Inkomst",
				Date = "Monday, January 1, 2017",
				Description = "Dator till ITHS",
				Type = "Försäljning (3000)",
				Account = "Företagskonto (1930)",
				Amount = 10000,
				IsImportant = true,//to delete
				Path = "sebastianstrus/projects/"

			});
			entries.Add(new Entry
			{
				Kind = "Utgift",
				Date = "Tuesday, January 2, 2017",
				Description = "Mat på ICA",
				Type = "Övriga egna uttag (2013)",
				Account = "Egna insättningar (2018)",
				Amount = 200,
				IsImportant = false,//to delete
				Path = "sebastianstrus/projects/"//to delete
			});
			entries.Add(new Entry
			{
				Kind = "Utgift",
				Date = "Wednesday, January 3, 2017",
				Description = "Ny domän",
				Type = "Reklam och PR (5900)",
				Account = "Företagskonto (1930)",
				Amount = 5000,
				IsImportant = true,//to delete
				Path = "sebastianstrus/projects/"//to delete

			});
			//TODO: lists in spinnere

		}

		public static void AddEntry(Entry e)
		{
			Instance.entries.Add(e); //or instance hmmm...
		}
	}
}

/*
 *
BookkeeperManager ska ha:
● En lista för alla Entries som finns i systemet
● En lista för alla möjliga inkomst-konton användaren kan välja
● En lista för alla möjliga utgift-konton användaren kan välja
● En lista för alla möjliga penga-konton användaren kan välja
● En lista för alla möjliga skattesatser användaren kan välja
● En metod AddEntry(Entry e) som lägger till ett Entry till systemet
Notera att ni själva ska anropa properties/metoderna ovan (med undantag av Entries) när ni
programmerar er “Ny händelse”­vy.
 **/

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