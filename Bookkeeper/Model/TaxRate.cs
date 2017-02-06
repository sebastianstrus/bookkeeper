using System;
namespace Bookkeeper
{
	public class TaxRate
	{
		public double Value { get; set; }

		public override string ToString()
		{
			return string.Format(Value * 100 + "%");
		}
	}
}
