namespace mynfo.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Domain;
    using Helpers;
    using Services;
    using Views;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using ZXing;

    public class LectorQRViewModel : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isScanning;
        private Result qrCode;
        private Box Box;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
            set
            {
                SetValue(ref this.isRunning, value);
            }
        }
        public bool IsScanning
        {
            get
            {
                return this.isScanning;
            }
            set
            {
                SetValue(ref this.isScanning, value);
            }
        }
        public Result QrCode
        {
            get
            {
                return this.qrCode;
            }
            set
            {
                SetValue(ref this.qrCode, value);
            }
        }

        public User User { get; set; }
        #endregion

        #region Constructor
        public LectorQRViewModel()
        {
            apiService = new ApiService();
            IsScanning = true;
        }
        #endregion

        #region Methods
        
        public void OnScanResult(Result result)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                IsScanning = false;
                string a = result.Text;
                string[] b = a.Split('=', '&');
                string[] R = a.Split('?');
                string Id = "";
                string NameUser = "";

                if (b[1] == "1")
                {
                    await GetUser(Convert.ToInt32(b[1]));
                }
                else
                {
                    Id = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(R[2]));
                    NameUser = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(R[1]));
                    await GetUser(Convert.ToInt32(Id));
                }
                var U = User;
                var A = Convert.ToInt32(Id);
                if (b.Length != 4 && A != U.UserId || NameUser != U.FullName)
                {
                    await Application.Current.MainPage.DisplayAlert(
                      Languages.Error,
                      Languages.InvalidUser,
                      Languages.Accept);
                    await App.Navigator.PopAsync();
                    return;
                }
                if (b[1] == "1")
                {
                    await GetBoxDefault(Convert.ToInt32(b[1]));
                }
                else
                {
                    await GetBoxDefault(Convert.ToInt32(Id));
                }

                result = null;
            });
            IsScanning = true;
        }

        public async Task<Box> GetBoxDefault(int id)
        {
            if (id == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.UserNotFound,
                   Languages.Accept);
                return null;
            }
            Box = new Box();
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Messagge,
                    Languages.Accept);
                return null;
            }

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var UserFornaneo = await this.apiService.GetUserId(
                                apiSecurity,
                                "/api",
                                "/Users/",
                                id);
            if (UserFornaneo == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.UserNotFound,
                    Languages.Accept);
                return null;
            }
            var BoxL = await this.apiService.GetBoxDefault<Box>(
                        apiSecurity,
                        "/api",
                        "/Boxes/GetBoxDefault",
                        id);
            if (BoxL == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.UserWithoutBoxes,
                    Languages.Accept);
                return null;
            }

            int UserIdToSend = UserFornaneo.UserId;

            //Perfil predeterminado, que es el perfil de Mynfo y box predeterminada de Ese perfil
            if (UserFornaneo.Share != true)
            {
                UserIdToSend = 1;
                BoxL = await apiService.GetBoxDefault<Box>(
                        apiSecurity,
                        "/api",
                        "/Boxes/GetBoxDefault",
                        UserIdToSend);

                if (BoxL == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        Languages.UserWithoutBoxes,
                        Languages.Accept);
                    return null;
                }
            }
            await App.Navigator.PopAsync();
            MainViewModel.GetInstance().Imprime_box = new Imprime_box();
            MainViewModel.GetInstance().Imprime_box.InsertForeignData(UserIdToSend, BoxL.BoxId);
            return BoxL;
        }

        public async Task<User> GetUser(int id)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            //Get User
            User = await apiService.GetUser(
                        apiSecurity,
                        "/api",
                        "/Users/",
                        id);
            return User;
        }
        #endregion

        #region Commands
        public ICommand BackHomeCommand
        {
            get
            {
                return new RelayCommand(BackHome);
            }
        }
        private void BackHome()
        {
            MainViewModel.GetInstance().Home = new HomeViewModel();
            MainViewModel.GetInstance().MenuItem = new MenuItemViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        #endregion
    }
}