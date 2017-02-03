﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using AEnvironment = Android.OS.Environment;
using AFile = Java.IO.File;
using AUri = Android.Net.Uri;



namespace Bookkeeper
{
	[Activity(Label = "New Entry")]
	public class ActivityNewEntry : Activity
	{

		TextView _dateDisplay;
		Button _dateSelectButton;
		ImageView _imageButton;

		Spinner spinnerType;
		Spinner spinnerAccount;
		Spinner spinnerTaxRate;
		Spinner spinnerTotalAmount;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_new_entry);

			// Spinners
			spinnerType = FindViewById<Spinner>(Resource.Id.spinner_type);
			ArrayAdapter adapterType = ArrayAdapter.CreateFromResource(this, Resource.Array.type_array, 
				Android.Resource.Layout.SimpleSpinnerItem);
			adapterType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerType.Adapter = adapterType;


			spinnerAccount = FindViewById<Spinner>(Resource.Id.spinner_account);
			ArrayAdapter adapterAccount = ArrayAdapter.CreateFromResource(this, Resource.Array.account_array,
				Android.Resource.Layout.SimpleSpinnerItem);
			spinnerAccount.Adapter = adapterAccount;


			spinnerTaxRate = FindViewById<Spinner>(Resource.Id.spinner_tax_rate);
			ArrayAdapter adapterTaxRate = ArrayAdapter.CreateFromResource(this, Resource.Array.tax_rate_array,
				Android.Resource.Layout.SimpleSpinnerItem);
			spinnerTaxRate.Adapter = adapterTaxRate;


			spinnerTotalAmount = FindViewById<Spinner>(Resource.Id.spinner_total_amount);
			ArrayAdapter adapterTotalAmount = ArrayAdapter.CreateFromResource(this, Resource.Array.total_amount_array,
				Android.Resource.Layout.SimpleSpinnerItem);
			spinnerTotalAmount.Adapter = adapterTotalAmount;


			//DatePicker
			_dateDisplay = FindViewById<TextView>(Resource.Id.tv_date_display);
			_dateSelectButton = FindViewById<Button>(Resource.Id.btn_date_button);
			_dateSelectButton.Click += DateSelect_OnClick;


			// Camera
			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();

				Button button = FindViewById<Button>(Resource.Id.btn_open_camera);
				_imageButton = FindViewById<ImageView>(Resource.Id.image_button);
				button.Click += TakeAPicture;
			}
		}


		// DateButton
		void DateSelect_OnClick(object sender, EventArgs eventArgs)
		{
			DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
																	 {
																		 _dateDisplay.Text = time.ToLongDateString();
																	 });
			frag.Show(FragmentManager, DatePickerFragment.TAG);
		}

		// Create directory for pictures
		private void CreateDirectoryForPictures()
		{
			App._dir = new AFile(
				AEnvironment.GetExternalStoragePublicDirectory(
					AEnvironment.DirectoryPictures), "CameraAppDemo");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}
		}

		// check if can open camera
		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities =
				PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		//se strony developera
		// Take picture
		private void TakeAPicture(object sender, EventArgs eventArgs)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			App._file = new AFile(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, AUri.FromFile(App._file));
			StartActivityForResult(intent, 0);
		}

		//se strony developera
		// Create directory and save picture
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// Make it available in the gallery

			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
			AUri contentUri = AUri.FromFile(App._file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// Display in ImageView. We will resize the bitmap to fit the display.
			// Loading the full sized image will consume to much memory
			// and cause the application to crash.

			int height = Resources.DisplayMetrics.HeightPixels;
			int width = _imageButton.Height;
			App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
			if (App.bitmap != null)
			{
				_imageButton.SetImageBitmap(App.bitmap);
				App.bitmap = null;
			}

			// Dispose of the Java side bitmap.
			GC.Collect();
		}

		private void AddEntry()
		{
			string currentType = spinnerType.SelectedItem.ToString(); // do something
			string currentAccount = spinnerAccount.SelectedItem.ToString();
			string currentTaxRate = spinnerTaxRate.SelectedItem.ToString();
			string currentTotalAmount = spinnerTotalAmount.SelectedItem.ToString();

			// TODO: create and use constructor
		}
	}



	//se strony developera
	public static class App
	{
		public static AFile _file;
		public static AFile _dir;
		public static Bitmap bitmap;
	}



	//se strony developera
	public static class BitmapHelpers
	{
		public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
								   ? outHeight / height
								   : outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			return resizedBitmap;
		}
	}




}
