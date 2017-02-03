
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
	[Activity(Label = "New Entry")]
	public class ActivityNewEntry : Activity
	{

		TextView _dateDisplay;
		Button _dateSelectButton;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_new_entry);

			ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView1);
			imageView.SetImageResource(Resource.Drawable.image);

			_dateDisplay = FindViewById<TextView>(Resource.Id.tv_date_display);
			_dateSelectButton = FindViewById<Button>(Resource.Id.btn_date_button);
			_dateSelectButton.Click += DateSelect_OnClick;
		}


		void DateSelect_OnClick(object sender, EventArgs eventArgs)
		{
			DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
																	 {
																		 _dateDisplay.Text = time.ToLongDateString();
																	 });
			frag.Show(FragmentManager, DatePickerFragment.TAG);
		}
	}
}
