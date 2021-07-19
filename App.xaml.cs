using mynfoApple.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mynfoApple
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Xamarin.Forms.Application
    {
        #region Variables
        public static string root_db;
        #endregion
        #region Properties
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