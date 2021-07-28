﻿namespace mynfo.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Rg.Plugins.Popup.Services;
    using System.Windows.Input;
    using Views;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class MenuItemViewModel : BaseViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        public bool Share { get; set; }
        public UserLocal User { get; set; }
        #endregion

        #region Constructor
        public MenuItemViewModel()
        {
            User = MainViewModel.GetInstance().User;
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }
        private async void Navigate()
        {
            App.Master.IsPresented = false;
            var mainViewModal = MainViewModel.GetInstance();
            //Logout
            if (this.PageName == "LoginPage")
            {
                Settings.IsRemembered = "false";
                mainViewModal.Token = null;
                mainViewModal.User = null;
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.DeleteAll<UserLocal>();
                }
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.DeleteAll<TokenResponse>();
                }
                //Borrar la box local
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.DeleteAll<BoxLocal>();
                }
                ////Borrar perfiles foraneos
                //using (var conn = new SQLite.SQLiteConnection(App.root_db))
                //{
                //    conn.DeleteAll<ProfileLocal>();
                //}
                //using (var conn = new SQLite.SQLiteConnection(App.root_db))
                //{
                //    conn.DeleteAll<ForeingBox>();
                //}
                ////Borrar perfiles foraneos
                //using (var conn = new SQLite.SQLiteConnection(App.root_db))
                //{
                //    conn.DeleteAll<ForeingProfile>();
                //}
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else if (this.PageName == "MyProfilePage")
            {
                MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                await App.Navigator.PushAsync(new MyProfilePage());
            }

            else if (this.PageName == "QR")
            {
                await App.Navigator.PushAsync(new QRTabbedPage());
            }

            else if (this.PageName == "TAGPage")
            {
                MainViewModel.GetInstance().TAG = new TAGViewModel();
                await App.Navigator.PushAsync(new TAGPage());
            }

            else if (this.PageName == "Help")
            {
                MainViewModel.GetInstance().GIF = new GifViewModel("All");
                await PopupNavigation.Instance.PushAsync(new GifPage());
            }

            else if (this.PageName == "Comments")
            {
                await Browser.OpenAsync("https://mynfo.mx/preguntas-frecuentes/", BrowserLaunchMode.SystemPreferred);
            }

            else if (this.PageName == "Store")
            {
                await Browser.OpenAsync("https://mynfo.mx/compra-tu-mytag/", BrowserLaunchMode.SystemPreferred);
            }
            else if (this.PageName == "BasicIntro")
            {
                MainViewModel.GetInstance().GIF = new GifViewModel("BasicIntro");
                await PopupNavigation.Instance.PushAsync(new GifPage());
            }
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }
        private void ChangeImage()
        {
            App.Master.IsPresented = false;
            MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
            App.Navigator.PushAsync(new MyProfilePage());
        }
        #endregion
    }
}