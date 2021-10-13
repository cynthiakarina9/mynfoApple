namespace mynfo.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using System;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRemembered
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsRunning = false;
            this.IsRemembered = true;
            this.IsEnabled = true;
        }
        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        private async void Login()
        {            
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                     Languages.Error,
                     Languages.PasswordValidation,
                     Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.isEnabled = false;

            try
            {
                var connection = await this.apiService.CheckConnection();

                if (!connection.IsSuccess)
                {
                    this.IsRunning = false;
                    this.isEnabled = true;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Messagge,
                        Languages.Accept);
                    return;
                }

                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                var token = await this.apiService.GetToken(
                    apiSecurity,
                    this.Email,
                    this.Password);

                if (token == null)
                {
                    this.IsRunning = false;
                    this.isEnabled = true;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        Languages.SomethingWrong,
                        Languages.Accept);
                    return;
                }

                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    this.IsRunning = false;
                    this.isEnabled = true;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        Languages.LoginError,
                        Languages.Accept);
                    this.Password = string.Empty;
                    return;
                }

                var user = await this.apiService.GetUserByEmail(
                    apiSecurity,
                    "/api",
                    "/Users/GetUserByEmail",
                    token.TokenType,
                    token.AccessToken,
                    this.Email);

                var userLocal = Converter.ToUserLocal(user, false);
                userLocal.Password = this.Password;

                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = token;
                mainViewModel.User = userLocal;

                if (this.IsRemembered)
                {
                    Settings.IsRemembered = "true";
                }
                else
                {
                    Settings.IsRemembered = "false";
                }

                //Save Local User in SQLite
                try
                {
                    using (var conn = new SQLite.SQLiteConnection(App.root_db))
                    {
                        conn.CreateTable<UserLocal>();
                        conn.Insert(userLocal);
                    }
                }
                catch (Exception es)
                {
                    Console.WriteLine(es);
                    return;
                }

                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.CreateTable<ForeingProfile>();
                }
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.CreateTable<ForeingBox>();
                }
                using (var conn = new SQLite.SQLiteConnection(App.root_db))
                {
                    conn.CreateTable<TokenResponse>();
                    conn.Insert(token);
                }

                using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                {
                    connSQLite.CreateTable<ForeingBox>();
                    connSQLite.CreateTable<ForeingProfile>();
                }

                mainViewModel.Home = new HomeViewModel();
                mainViewModel.MenuItem = new MenuItemViewModel();
                mainViewModel.Register = new RegisterViewModel();
                mainViewModel.MyProfile = new MyProfileViewModel();
                mainViewModel.Profiles = new ProfilesViewModel();
                mainViewModel.TAG = new TAGViewModel();
                mainViewModel.ListForeignBox = new ListForeignBoxViewModel();
                Application.Current.MainPage = new MasterPage();

                this.IsRunning = false;
                this.isEnabled = true;

                this.Email = string.Empty;
                this.Password = string.Empty;
            }
            catch(Exception e)
            {                
                await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        "Ah ocurrido un error, vuelva a intentalo más tarde.",
                        Languages.Accept) ;
                this.IsRunning = false;
                this.isEnabled = true;
                return;
            }

        }

        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }
        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public ICommand RecoverPasswordCommand
        {
            get
            {
                return new RelayCommand(RecoverPassword);
            }
        }
        async void RecoverPassword()
        {
            MainViewModel.GetInstance().PasswordRecovery = new PasswordRecoveryViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new PasswordRecoveryPage());
        }
        #endregion
    }
}