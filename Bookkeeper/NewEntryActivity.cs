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

using SQLite;

using AEnvironment = Android.OS.Environment;
using AFile = Java.IO.File;
using AUri = Android.Net.Uri;


namespace Bookkeeper
{
	[Activity(Label = "New Entry")]
	public class NewEntryActivity : Activity
	{
		string entryId;
		//Entry entryToEdit;
		TextView tvNewEntry, tvDateDisplay, tvTotalAmountExclTax;
		Button _dateSelectButton, btnAddEntry;
		ImageView _imageButton;
		RadioButton rbIncome, rbExpense;
		Spinner spinnerType, spinnerAccount, spinnerTaxRate;
		ArrayAdapter adapterIncomeType, adapterExpenseType, adapterAccount, adapterTaxRate;
		EditText etDescription, etTotalAmountInclTax;
		TaxRate currentTaxRate;
		Account currentTypeAccount, currentMoneyAccount;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_new_entry);

			// Spinners
			spinnerType = FindViewById<Spinner>(Resource.Id.spinner_type);
			spinnerType.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_TypeAccountSelected);
			//adapterIncomeType = ArrayAdapter.CreateFromResource(this, Resource.Array.type_income_array, 
			//	Android.Resource.Layout.SimpleSpinnerItem);
			adapterIncomeType = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.IncomeAccountList.ToList());
			//adapterExpenseType = ArrayAdapter.CreateFromResource(this, Resource.Array.type_expense_array,
			//	Android.Resource.Layout.SimpleSpinnerItem);
			adapterExpenseType = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.ExpenseAccountList.ToList());

			adapterIncomeType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapterExpenseType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerType.Adapter = adapterIncomeType;

			spinnerAccount = FindViewById<Spinner>(Resource.Id.spinner_account);
			spinnerAccount.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_MoneyAccountSelected);
			adapterAccount = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.MoneyAccountList.ToList());
			spinnerAccount.Adapter = adapterAccount;

			spinnerTaxRate = FindViewById<Spinner>(Resource.Id.spinner_tax_rate);
			spinnerTaxRate.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_TaxRateSelected);
			adapterTaxRate = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperMenager.Instance.TaxRateList.ToList());
			spinnerTaxRate.Adapter = adapterTaxRate;

			//=============================================================================================

			//DatePicker
			tvDateDisplay = FindViewById<TextView>(Resource.Id.tv_date_display);
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

			//=============================================================================================

			// RadioButtons clicked
			rbIncome = FindViewById<RadioButton>(Resource.Id.rb_income);
			rbExpense = FindViewById<RadioButton>(Resource.Id.rb_expense);
			rbIncome.Click += RadioButtonIncomeClick;
			rbExpense.Click += RadioButtonExpenseClick;

			// btn AddEntry
			btnAddEntry = FindViewById<Button>(Resource.Id.btn_add_entry);
			btnAddEntry.Click += AddEntry_OnClick;

			tvNewEntry = FindViewById<TextView>(Resource.Id.tv_activity_new_entry);
			etDescription = FindViewById<EditText>(Resource.Id.description);

			// set total amount excl. tax
			tvTotalAmountExclTax = FindViewById<TextView>(Resource.Id.total_amount_excl_tax);
			etTotalAmountInclTax = FindViewById<EditText>(Resource.Id.id_amount);
			//etTotalAmountInclTax.Text = "0";
			etTotalAmountInclTax.TextChanged += etTextChanged;
			etTotalAmountInclTax.KeyPress += etKeyPress;

			// check if there is Entry to Edit. If 'yes' set ID, else id= -1.
			entryId = Intent.GetStringExtra("ENTRY_ID") ?? "-1";
			Console.WriteLine("EntryID to edit: " + entryId);
			if (int.Parse(entryId) >= 0)
			{

				setEntryToEdit(int.Parse(entryId));
			}


		}

		//========================================================================================
		//========================================================================================

		// set data for Entry to edit
		private void setEntryToEdit(int entryId)
		{
			SQLiteConnection db = new SQLiteConnection(BookkeeperMenager.Instance.dbPath);
			Entry entryToEdit = db.Get<Entry>(entryId);//.Where(e => (e.Id == entryId)).ToList();
			Console.WriteLine(entryToEdit); // print Entry
			if (!entryToEdit.IsIncome)
			{
				rbExpense.Checked = true;
				spinnerType.Adapter = adapterExpenseType;
			}
			tvDateDisplay.Text = entryToEdit.Date;
			etDescription.Text = entryToEdit.Description;
			// set spinner Type
			List <Account> listInTypeSpinner = rbIncome.Checked? BookkeeperMenager.Instance.IncomeAccountList
			                             : BookkeeperMenager.Instance.ExpenseAccountList;
			string spinnerTypeValue = listInTypeSpinner.Where(a => (a.Number == entryToEdit.TypeID)).ToList()[0].ToString();

			Console.WriteLine("spinnerTypeValue is: " + spinnerTypeValue);

			int indexType = 0;
			for (int i = 0; i < listInTypeSpinner.Count(); i++)
			{
				if (listInTypeSpinner[i].ToString() == (spinnerTypeValue))
				{
					indexType = i;
				}
			}
			spinnerType.SetSelection(indexType);
	
			// set spinner Account
			List<Account> listInAccountSpinner = BookkeeperMenager.Instance.MoneyAccountList;
			string spinnerAccountValue = listInAccountSpinner.Where(a => (a.Number == entryToEdit.AccountID)).ToList()[0].ToString();
			Console.WriteLine("spinnerAccountValue is: " + spinnerAccountValue);

			int indexAccount = 0;
			for (int i = 0; i < listInAccountSpinner.Count(); i++)
			{
				if (listInAccountSpinner[i].ToString() == (spinnerAccountValue))
				{
					indexAccount = i;
				}
			}
			spinnerAccount.SetSelection(indexAccount);

			etTotalAmountInclTax.Text = entryToEdit.Amount + "";
			spinnerTaxRate.SetSelection(entryToEdit.TaxRateID-1);

			setAmountExclTax();
			tvNewEntry.Text = "";
			btnAddEntry.Text = "Save";
		}
		//========================================================================================

		// set total amount excl tax
		void etTextChanged(object sender, TextChangedEventArgs e)
		{
			setAmountExclTax();
		}

		void etKeyPress(object sender, View.KeyEventArgs e)
		{
			e.Handled = false;
			setAmountExclTax();
		}

		void setAmountExclTax()
		{
			string temp = spinnerTaxRate.SelectedItem.ToString();
			Console.WriteLine("string temp is: " + temp);
			double value = double.Parse(temp.Substring(0, temp.Length - 1)) / 100.0;
			if (etTotalAmountInclTax.Text != "")
			{
				tvTotalAmountExclTax.Text = Math.Round(double.Parse(etTotalAmountInclTax.Text.ToString()) / (1.0 + value), 2) + "";
			}
			else 
			{
				tvTotalAmountExclTax.Text = "";
			}
		}

		void spinner_TypeAccountSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			currentTypeAccount = rbIncome.Checked ? BookkeeperMenager.Instance.IncomeAccountList[e.Position] 
			                             : BookkeeperMenager.Instance.ExpenseAccountList[e.Position];
		}
		void spinner_MoneyAccountSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			currentMoneyAccount = BookkeeperMenager.Instance.MoneyAccountList[e.Position];
		}

		// set total amount excl tax
		private void spinner_TaxRateSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			currentTaxRate = BookkeeperMenager.Instance.TaxRateList[e.Position];
			string temp = ((Spinner)sender).SelectedItem.ToString();
			double value = double.Parse(temp.Substring(0, temp.Length - 1)) / 100; //t.ex. 0.12

			if (etTotalAmountInclTax.Text != "")
			{
				tvTotalAmountExclTax.Text = Math.Round(double.Parse(etTotalAmountInclTax.Text.ToString()) / (1 + value), 2) + "";
			}
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
																		 tvDateDisplay.Text = time.ToShortDateString();
																	 });
			frag.Show(FragmentManager, DatePickerFragment.TAG);
		}

		//========================================================================================
		// Camera, check if can open camera
		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities =
				PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		// Camera, create directory for pictures
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

		// Camera, Take picture
		private void TakeAPicture(object sender, EventArgs eventArgs)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			App._file = new AFile(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

			intent.PutExtra(MediaStore.ExtraOutput, AUri.FromFile(App._file));
			Console.WriteLine("TakePicture clicked, path: " + App._file.Path);
			StartActivityForResult(intent, 0);
		}

		// Camera, Create directory and save picture
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
			else
			{
				Console.WriteLine("Bitmap is empty"); // :/	
			}
			// Dispose of the Java side bitmap.
			GC.Collect();
		}
		//========================================================================================

		// Button Add Entry clicked
		void AddEntry_OnClick(object sender, EventArgs e)
		{
			// validation
			if ((etDescription.Text != "") && (tvDateDisplay.Text != "-") && (etTotalAmountInclTax.Text != ""))
			{
				// Create Entry
				Entry temp = new Entry
				{
					IsIncome = rbIncome.Checked ? true : false,
					Date = tvDateDisplay.Text,
					Description = etDescription.Text,
					TypeID = currentTypeAccount.Number,
					AccountID = currentMoneyAccount.Number,
					Amount = int.Parse(etTotalAmountInclTax.Text),
					TaxRateID = currentTaxRate.Id,
					//Path = "...", kameran funkar inte :/
				};

				// uppdate or add Entry
				if (int.Parse(entryId) >= 0)
				{
					BookkeeperMenager.UpdateEntry(temp, int.Parse(entryId));
					Toast.MakeText(this, "Saved", ToastLength.Short).Show();

					// Go to MainMenuActivity
					Intent intent = new Intent(this, typeof(AllEntriesActivity));
					this.StartActivity(intent);
				}
				else
				{
					BookkeeperMenager.AddEntry(temp);
					//db.Insert(temp);
					//BookkeeperMenager.UpdateEntryList();
					Toast.MakeText(this, "Added", ToastLength.Short).Show();

					// Go to MainMenuActivity
					Intent intent = new Intent(this, typeof(MainMenuActivity));
					this.StartActivity(intent);
				}
			}
			else
			{
				Toast.MakeText(this, "Incorrect data", ToastLength.Short).Show();
			}
		}
	}

	//Camera
	public static class App
	{
		public static AFile _file;
		public static AFile _dir;
		public static Bitmap bitmap;
	}

	//Camera
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