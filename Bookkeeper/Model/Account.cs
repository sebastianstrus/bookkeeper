using System;
namespace Bookkeeper
{
	public class Account
	{
		public string Name { get; set; }
		public string Number { get; set; }


		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Number.Substring(Number.Length - 4));
		}
	}
}
