﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace Bookkeeper
{
	public class EntryAdapter : BaseAdapter
	{

		private Activity context;
		private List<Entry> entries;

		public EntryAdapter(Activity activity, List<Entry> entries)
		{
			this.context = activity;
			this.entries = entries;
		}


		public override int Count
		{
			get
			{ return entries.Count; }
		}


		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}


		public override long GetItemId(int position)
		{
			Intent intent = new Intent(context, typeof(NewEntryActivity));
			Console.WriteLine("GetItemId in EntryAdapter is going:");
			Console.WriteLine("w intent:  int position: " + position);
			Console.WriteLine("w intent: entries[position].Id.ToString() " + entries[position].Id.ToString());
			intent.PutExtra("POSITION_NR", position);
			intent.PutExtra("ENTRY_ID", entries[position].Id.ToString());
			context.StartActivity(intent);
			context.Finish();

			return position;
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.entry_list_item, parent, false);

			view.FindViewById<TextView>(Resource.Id.tvDate).Text = entries[position].Date; 
			view.FindViewById<TextView>(Resource.Id.tvTitle).Text = entries[position].Description;
			view.FindViewById<TextView>(Resource.Id.tvAmount).Text = (entries[position].IsIncome ? "" : "-") + entries[position].Amount +"kr";


			return view;
		}


	}
}
