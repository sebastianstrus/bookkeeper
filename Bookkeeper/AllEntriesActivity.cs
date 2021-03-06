﻿using Android.App;
using Android.Widget;
using Android.OS;
using System;


namespace Bookkeeper
{
	[Activity(Label = "All Entries")]
	public class AllEntriesActivity : Activity
	{
		private ListView entryList;
		private RadioButton rbAllEntries, rbImportantEntries, rbNotImportantEntries;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
						SetContentView(Resource.Layout.activity_all_entries);


			entryList = FindViewById<ListView>(Resource.Id.entry_list);
			entryList.Adapter = new EntryAdapter(this, BookkeeperMenager.Instance.Entries);

			rbAllEntries = FindViewById<RadioButton>(Resource.Id.rb_all_entries);
			rbImportantEntries = FindViewById<RadioButton>(Resource.Id.rb_important_entries);
			rbNotImportantEntries = FindViewById<RadioButton>(Resource.Id.rb_not_important_entries);

			rbAllEntries.Click += button_UpdateEntries;
			rbImportantEntries.Click += button_UpdateEntries;
			rbNotImportantEntries.Click += button_UpdateEntries;

		}

		private void button_UpdateEntries(object sender, EventArgs e)
		{
			if (rbImportantEntries.Checked)
			{
				entryList.Adapter = new EntryAdapter(this, BookkeeperMenager.Instance.IncomeEntries);
			}
			else if (rbNotImportantEntries.Checked)
			{
				entryList.Adapter = new EntryAdapter(this, BookkeeperMenager.Instance.ExpenseEntries);
			}
			else
			{
				entryList.Adapter = new EntryAdapter(this, BookkeeperMenager.Instance.Entries);
			}
		}
	}
}
