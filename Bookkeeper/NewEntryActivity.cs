
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
using Android.Text;
using Android.Views;
using Android.Widget;

using AEnvironment = Android.OS.Environment;
using AFile = Java.IO.File;
using AUri = Android.Net.Uri;



namespace Bookkeeper
{
	[Activity(Label = "New Entry")]
	public class NewEntryActivity : Activity
	{

		TextView _dateDisplay;
		Button _dateSelectButton;
		ImageView _imageButton;

		RadioButton rbIncome;
		RadioButton rbExpense;

		Spinner spinnerType;
		ArrayAdapter adapterIncomeType;
		ArrayAdapter adapterExpenseType;

		Spinner spinnerAccount;
		ArrayAdapter adapterAccount;

		Spinner spinnerTaxRate;
		ArrayAdapter adapterTaxRate;

		EditText etDescription;
		EditText etTotalAmountInclTax;

		TextView tvTotalAmountExclTax;
		Button btnAddEntry;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_new_entry);


			//=============================================================================================
			// Spinners
			spinnerType = FindViewById<Spinner>(Resource.Id.spinner_type);
			// TODO: change adapter and move array to BookkeeperMenager if needed?
			adapterIncomeType = ArrayAdapter.CreateFromResource(this, Resource.Array.type_income_array, 
				Android.Resource.Layout.SimpleSpinnerItem);
			//adapterIncomeType = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.IncomeTypeList.ToList());
			adapterExpenseType = ArrayAdapter.CreateFromResource(this, Resource.Array.type_expense_array,
				Android.Resource.Layout.SimpleSpinnerItem);
			//adapterExpenseType = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.ExpenseTypeList.ToList());
			adapterIncomeType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapterExpenseType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerType.Adapter = adapterIncomeType;

			//=============================================================================================
			spinnerAccount = FindViewById<Spinner>(Resource.Id.spinner_account);
			/*adapterAccount = ArrayAdapter.CreateFromResource(this, Resource.Array.account_array,
				Android.Resource.Layout.SimpleSpinnerItem);*/
			adapterAccount = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.AccountList.ToList());
			spinnerAccount.Adapter = adapterAccount;
			//=============================================================================================

			spinnerTaxRate = FindViewById<Spinner>(Resource.Id.spinner_tax_rate);
			spinnerTaxRate.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_TaxRateSelected);
			/*adapterTaxRate = ArrayAdapter.CreateFromResource(this, Resource.Array.tax_rate_array,
				Android.Resource.Layout.SimpleSpinnerItem);*/
			adapterTaxRate = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.TaxRateList.ToList());
			spinnerTaxRate.Adapter = adapterTaxRate;

			//=============================================================================================

			//DatePicker
			// TODO: Change date format if needed?
			_dateDisplay = FindViewById<TextView>(Resource.Id.tv_date_display);
			_dateSelectButton = FindViewById<Button>(Resource.Id.btn_date_button);
			_dateSelectButton.Click += DateSelect_OnClick;

			//=============================================================================================
			// Camera
			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();

				Button button = FindViewById<Button>(Resource.Id.btn_open_camera);
				_imageButton = FindViewById<ImageView>(Resource.Id.image_button);
				button.Click += TakeAPicture;
			}

			// RadioButtons clicked
			rbIncome = FindViewById<RadioButton>(Resource.Id.rb_income);
			rbExpense = FindViewById<RadioButton>(Resource.Id.rb_expense);
			rbIncome.Click += RadioButtonIncomeClick;
			rbExpense.Click += RadioButtonExpenseClick;


			btnAddEntry = FindViewById<Button>(Resource.Id.btn_add_entry);
			btnAddEntry.Click += AddEntry_OnClick;


			etDescription = FindViewById<EditText>(Resource.Id.description);

			// set total amount excl. tax
			tvTotalAmountExclTax = FindViewById<TextView>(Resource.Id.total_amount_excl_tax);
			etTotalAmountInclTax = FindViewById<EditText>(Resource.Id.id_amount);
			etTotalAmountInclTax.Text = "0";
			etTotalAmountInclTax.TextChanged += etTextChanged;
			etTotalAmountInclTax.KeyPress += etKeyPress;

			//tvTotalAmountExclTax.Text = 
				//brutto / (1 + skatt)
		}

		void etTextChanged(object sender, TextChangedEventArgs e)
		{
			string temp = spinnerTaxRate.SelectedItem.ToString();

			double value = double.Parse(temp.Substring(0, temp.Length - 1)) / 100;

			if (etTotalAmountInclTax.Text != "")
			{
				tvTotalAmountExclTax.Text = Math.Round(double.Parse(etTotalAmountInclTax.Text.ToString()) / (1 + value), 2) + "";
				Console.WriteLine("EditText value is: " + value);
			}
		}

		void etKeyPress(object sender, View.KeyEventArgs e)
		{
			e.Handled = false;
			string temp = spinnerTaxRate.SelectedItem.ToString();

			double value = double.Parse(temp.Substring(0, temp.Length - 1)) / 100;

			if (etTotalAmountInclTax.Text != "")
			{
				tvTotalAmountExclTax.Text = Math.Round(double.Parse(etTotalAmountInclTax.Text.ToString()) / (1 + value), 2) + "";
				Console.WriteLine("EditText value is: " + value);
			}
		}

		private void spinner_TaxRateSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			//etTotalAmountInclTax.Text = "0";
			//Spinner s = (Spinner)sender;
			string temp = ((Spinner)sender).SelectedItem.ToString();
			double value = double.Parse(temp.Substring(0, temp.Length - 1))/100; //t.ex. 0.12

			if (etTotalAmountInclTax.Text != "")
			{
				tvTotalAmountExclTax.Text = Math.Round(double.Parse(etTotalAmountInclTax.Text.ToString()) / (1 + value), 2) + "";
				Console.WriteLine("EditText value is: " + value);
			}
			//int a = 0, s.SelectedItem.ToString().Length -1;
			Console.WriteLine(value);
			//TODO: odjac znak '%'
			// mam "12%" i liczbe brutto
			//int.Parse(etTotalAmountInclTax.Text.ToString())

			//netto = brutto/(1+skatt)
			// Number.Substring(Number.Length - 4))
		}






		// RadioButtons clicked, change adapter for spinnerType
		void RadioButtonIncomeClick(object sender, EventArgs e)
		{
			spinnerType.Adapter = adapterIncomeType;
		}
		void RadioButtonExpenseClick(object sender, EventArgs e)
		{
			spinnerType.Adapter = adapterExpenseType;
		}

		// DateButton
		void DateSelect_OnClick(object sender, EventArgs eventArgs)
		{
			DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
																	 {
				_dateDisplay.Text = time.ToShortDateString();
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

		// Create Entry clicked
		void AddEntry_OnClick(object sender, EventArgs e)
		{
			if (true)
			{


				Entry temp = new Entry
				{
					Kind = rbIncome.Checked ? "Inkomst" : "Utgift",
					Date = _dateDisplay.Text,
					Description = etDescription.Text,
					Type = spinnerType.SelectedItem.ToString(),
					Account = new Account
					{
						Name = "nazwa konta",
						Number = "12345678"
					},//spinnerAccount.SelectedItem.ToString();
					Amount = int.Parse(etTotalAmountInclTax.Text.ToString()),
					TaxRate = new TaxRate
					{
						Value = 0.12
					},
					Path = "abc",
				};
				BookkeeperMenager.AddEntry(temp);

				Toast.MakeText(this, "Added", ToastLength.Short).Show();
				Intent intent = new Intent(this, typeof(MainMenuActivity));
				this.StartActivity(intent);
			}
			else
			{
				// TODO: some form validation if needed
				Toast.MakeText(this, "Ange rätt uppgifter.", ToastLength.Short).Show();

			}

		}

		// TODO: set amount excl tax

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
//netto = brutto/(1+skatt)