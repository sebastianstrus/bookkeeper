using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Bookkeeper
{
	[Activity(Label = "Bookkeeper", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainMenuActivity : Activity
	{
		string path = System.Environment.GetFolderPath
 (System.Environment.SpecialFolder.Personal);
		SQLiteConnection db = new SQLiteConnection(path + @”\database.db”);
		db.CreateTable<Person>();

		private Button btnNewEntry;
		private Button btnAllEntries;
		private Button btnCreateReports;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main_menu);

			btnNewEntry = FindViewById<Button>(Resource.Id.btn_new_entry);
						btnNewEntry.Click += delegate
						{
							Intent intent = new Intent(this, typeof(NewEntryActivity));
							this.StartActivity(intent);
						};


			btnAllEntries = FindViewById<Button>(Resource.Id.btn_all_antries);
						btnAllEntries.Click += delegate
						{
							Intent intent = new Intent(this, typeof(AllEntriesActivity));
							this.StartActivity(intent);
						};


			btnCreateReports = FindViewById<Button>(Resource.Id.btn_create_reports);
						btnCreateReports.Click += delegate
						{
							Intent intent = new Intent(this, typeof(CreateReportsActivity));
							this.StartActivity(intent);
						};

		}
	}
}
