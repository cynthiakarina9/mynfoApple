namespace mynfo.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using mynfo.Domain;
    using mynfo.Services;
    using Rg.Plugins.Popup.Services;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Views;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class MenuItemViewModel : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        private User user1;
        #endregion

        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        public bool Share { get; set; }
        public UserLocal User { get; set; }
        public User User1 
        {
            get { return this.user1; }
            set { SetValue(ref this.user1, value); }
        }
        #endregion

        #region Constructor
        public MenuItemViewModel()
        {
            apiService = new ApiService();
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
                //using (var conn = new SQLite.SQLiteConnection(App.root_db))
                //{
                //    conn.DeleteAll<BoxLocal>();
                //}
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
                await Browser.OpenAsync("https://store.mynfo.mx/compra-tu-mytag/", BrowserLaunchMode.SystemPreferred);
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

        #region Methods
        public async Task<User> GetUser(bool value)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            User1 = await apiService.GetUser(
                apiSecurity,
                "/api",
                "/Users/",
                User.UserId);           
            var userDomain = new User
            {
                UserId = User1.UserId,
                FirstName = User1.FirstName,
                LastName = User1.LastName,
                Email = User1.Email,
                ImagePath = User1.ImagePath,
                UserTypeId = User1.UserTypeId,
                Share = value,
                Edad = User1.Edad,
                Ubicacion = User1.Ubicacion,
                Ocupacion = User1.Ocupacion,
                Conexiones = User1.Conexiones,
                Fecha = User1.Fecha,
            };
            
            var response = await apiService.Put(
                apiSecurity,
                "/api",
                "/Users",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                userDomain);
            var userLocal = Converter.ToUserLocal(userDomain);
            using (var conn = new SQLite.SQLiteConnection(App.root_db))
            {
                conn.Update(userLocal);
            }
            if (response.IsSuccess == false)
            {
                return null;
            }
            return User1;
        }

        public async Task<User> GetUser1()
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            User1 = new User();
            User1 = await apiService.GetUser(
                apiSecurity,
                "/api",
                "/Users/",
                User.UserId);
            
            return User1;
        }
        #endregion
    }
}