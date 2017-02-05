using System;
namespace Bookkeeper
{
	public class Entry
	{
		
		public String Kind { get; set; }
		public String Date { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public String Account { get; set; }//class
		public int Amount { get; set; }
		public bool IsImportant { get; set; }//TaxRate
		public String Path { get; set; } //Path



	}
}
