using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookkeeper
{
	class EntryMenager
	{
		private static EntryMenager instance;

		public static EntryMenager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new EntryMenager();
				}
				return instance;
			}
		}

		private List<Entry> entries;
		public List<Entry> Entries { get { return entries; } }

		public List<Entry> ImportantEntries { get { return entries.Where(b => b.IsImportant).ToList(); } }
		public List<Entry> NotImportantEntries { get { return entries.Where(b => !b.IsImportant).ToList(); } }


		private EntryMenager()
		{
			entries = new List<Entry>();

			entries.Add(new Entry
			{
				Date = "1987-01-17",
				Title = "Mat på ICA",
				Amount = 100,
				IsImportant = false
			});
			entries.Add(new Entry
			{
				Date = "1987-01-18",
				Title = "Mat på Lidl",
				Amount = 200,
				IsImportant = true
			});
			entries.Add(new Entry
			{
				Date = "1987-01-19",
				Title = "Mat på Netto",
				Amount = 300,
				IsImportant = true

			});
		}
	}
}
