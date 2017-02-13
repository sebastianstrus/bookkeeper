
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
			btnAccountReport.Click += delegate 
			{
				Intent intent = new Intent(this, typeof(AccountReportActivity));
				this.StartActivity(intent);
			};

			btnTaxReport.Click += delegate 
			{
				Intent intent = new Intent(this, typeof(TaxReportActivity));
				this.StartActivity(intent);
			};

			//TODO:
			// public string GetTaxReport() w backend


		}

	}

}
