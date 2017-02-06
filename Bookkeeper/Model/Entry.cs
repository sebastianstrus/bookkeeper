using System;
namespace Bookkeeper
{
	public class Entry
	{
		
		public String Kind { get; set; }
		public String Date { get; set; }
		public String Description { get; set; }
		public String Type { get; set; }
		public Account Account { get; set; }//class bylo String
		public int Amount { get; set; }
		public TaxRate TaxRate { get; set; }//TaxRate
		public String Path { get; set; } //Path



	}
}
