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
	[Activity(Label = "TaxReportActivity")]
	public class TaxReportActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_tax_report);

			TextView tvTaxReport = FindViewById<TextView>(Resource.Id.tax_report);
			tvTaxReport.Text = BookkeeperMenager.Instance.GetTaxReport();

		}
	}
}

