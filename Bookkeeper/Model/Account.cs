using System;
namespace Bookkeeper
{
	public class Account
	{
		string name;
		string number;

		public Account(string n, string nr)
		{
			name = n;
			number = nr;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", name, number.Substring(number.Length - 4));
		}
	}
}
