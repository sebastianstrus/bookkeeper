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
