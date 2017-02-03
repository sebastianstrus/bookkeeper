
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
	[Activity(Label = "Bookkeeper", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainMenuActivity : Activity
	{

		private Button btnNewEntry;
		private Button btnAllEntries;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main_menu);

			btnNewEntry = FindViewById<Button>(Resource.Id.btn_new_entry);
			btnNewEntry.Click += delegate
			{
				Intent intent = new Intent(this, typeof(ActivityNewEntry));
				this.StartActivity(intent);
			};
			btnAllEntries = FindViewById<Button>(Resource.Id.btn_all_antries);
			btnAllEntries.Click += delegate
			{
				Intent intent = new Intent(this, typeof(AllEntriesActivity));
				this.StartActivity(intent);
			};
		}
	}
}
