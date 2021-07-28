namespace mynfo
{
    using Helpers;
    using Models;
    using mynfo.Services;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ViewModels;
    using Views;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public partial class App : Xamarin.Forms.Application
    {
        #region Variables
        public static string root_db;
        #endregion

        #region Properties
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }
        public static MasterPage Master
        {
            get;
            internal set;
        }
        public static string FolderPath { get; private set; }
        #endregion

        #region Constructors
        public App(string root_DB)
        {
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));            

            InitializeComponent();

            //Set root SQLite
            root_db = root_DB;
            Xamarin.Forms.Device.SetFlags(new[] { "CarouselView_Experimental" });
            Preferences.Set("key1", Guid.NewGuid().ToString());
            Preferences.Set("IsEnabled", true);

            if (Settings.IsRemembered == "true")
            {

                var token = new TokenResponse();
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.CreateTable<TokenResponse>();
                    token = conn.Table<TokenResponse>().FirstOrDefault();
                }

                if ((token != null) && (token.Expires > DateTime.Now))
                {
                    //Connection with SQLite
                    var user = new UserLocal();
                    using (var conn = new SQLite.SQLiteConnection(App.root_db))
                    {
                        conn.CreateTable<UserLocal>();
                        user = conn.Table<UserLocal>().FirstOrDefault();
                        int a = conn.Table<UserLocal>().Count();
                    }
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = token;
                    mainViewModel.User = user;//sqlite
                    mainViewModel.Home = new HomeViewModel();
                    mainViewModel.Register = new RegisterViewModel();
                    mainViewModel.MyProfile = new MyProfileViewModel();
                    mainViewModel.Profiles = new ProfilesViewModel();
                    mainViewModel.TAG = new TAGViewModel();
                    mainViewModel.ChangePassword = new ChangePasswordViewModel();
                    mainViewModel.ListForeignBox = new ListForeignBoxViewModel();
                    mainViewModel.MenuItem = new MenuItemViewModel();

                    Current.MainPage = new MasterPage();
                }
                else
                {
                    //Delete Token and User
                    using (var conn = new SQLite.SQLiteConnection(App.root_db))
                    {
                        conn.DeleteAll<TokenResponse>();
                    }
                    using (var conn = new SQLite.SQLiteConnection(App.root_db))
                    {
                        conn.DeleteAll<UserLocal>();
                    }
                    this.MainPage = new NavigationPage(new LoginPage());
                }

            }
            else
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion

        #region Methods

        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Xamarin.Forms.Application.Current.MainPage =
                                  new NavigationPage(new LoginPage()));
            }
        }        

        /*public static async Task DisplayAlertAsync(string msg) =>
            await Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () => await Current.MainPage.DisplayAlert("message from service", msg, "ok"));*/


        public static async Task DisplayAlertAsync(string msg) =>
          await Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () => await Current.MainPage.DisplayAlert("", msg, "ok"));


        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        #endregion
    }
}