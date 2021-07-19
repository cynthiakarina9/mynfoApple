namespace mynfoApple
{
    using mynfoApple.Views;
    using System;
    using System.IO;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
        public static LoginPage Master
        {
            get;
            internal set;
        }
        public static string FolderPath { get; private set; }
        #endregion
        public App(string _root_DB = "")
        {
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            InitializeComponent();

            //Set root DB sqlite
            root_db = _root_DB;

            this.MainPage = new NavigationPage(new LoginPage());

        }
    }
}