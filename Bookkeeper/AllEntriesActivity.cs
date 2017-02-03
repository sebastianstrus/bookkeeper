using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace Bookkeeper
{
	[Activity(Label = "MainActivity")]
	public class AllEntriesActivity : Activity
	{
		private ListView entryList;
		private RadioButton rbAllEntries, rbImportantEntries, rbNotImportantEntries;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_all_entries);

			// Get our button from the layout resource,
			// and attach an event to it
			entryList = FindViewById<ListView>(Resource.Id.entry_list);
			entryList.Adapter = new EntryAdapter(this, EntryMenager.Instance.Entries);

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
				entryList.Adapter = new EntryAdapter(this, EntryMenager.Instance.ImportantEntries);
			}
			else if (rbNotImportantEntries.Checked)
			{
				entryList.Adapter = new EntryAdapter(this, EntryMenager.Instance.NotImportantEntries);
			}
			else
			{
				entryList.Adapter = new EntryAdapter(this, EntryMenager.Instance.Entries);
			}
		}
	}
}

