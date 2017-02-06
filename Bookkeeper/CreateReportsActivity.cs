
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Bookkeeper
{
	[Activity(Label = "CreateReportsActivity")]
	public class CreateReportsActivity : Activity
	{

		Button btnAccountReport;
		Button btnTaxReport;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_create_reports);

			btnAccountReport = FindViewById<Button>(Resource.Id.btn_account_report);
			btnTaxReport = FindViewById<Button>(Resource.Id.btn_tax_report);
			btnAccountReport.Click += DoIt;
			btnTaxReport.Click += DoIt;


			// public string GetTaxReport() w backend

			// Create your application here


		}

		void DoIt(object sender, EventArgs e)
		{
			/*string name = "Företagskonto";
			string number = "0123456789";
			Console.WriteLine(string.Format("{0} ({1})", name, number.Substring(number.Length - 4)));
			*/
			Account account = new Account
			{
				Name = "nazwa",
				Number = "12345678"
			};
			Console.WriteLine(account);
			Console.WriteLine(account.Name);
			Console.WriteLine(account.Number);
		}
	}

}
