namespace mynfo.ViewModels
{
    using Domain;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class EditProfilePhoneViewModel : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ProfilePhone profilephone;
        #endregion

        #region Properties
        public ProfilePhone profilePhone
        {
            get { return profilephone; }
            private set
            {
                SetValue(ref profilephone, value);
            }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Constructor
        public EditProfilePhoneViewModel(int _ProfilePhoneId)
        {
            apiService = new ApiService();
            GetProfilePhone(_ProfilePhoneId);            
        }
        #endregion

        #region Methods
        private async Task<ProfilePhone> GetProfilePhone(int _ProfilePhoneId)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            profilePhone = new ProfilePhone();
            profilePhone = await this.apiService.GetProfilePhone(
               apiSecurity,
               "/api",
               "/ProfilePhones/GetProfilePhone",
               _ProfilePhoneId);
            return profilePhone;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        private async void Save()
        {
            if (string.IsNullOrEmpty(this.profilePhone.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameValidation,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.profilePhone.Number))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NumberValidation,
                    Languages.Accept);
                return;
            }
            if (!(this.profilePhone.Number).ToCharArray().All(Char.IsDigit))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NumberValidation,
                    Languages.Accept);
                return;
            }
            if (this.profilePhone.Number.Length != 10)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneValidation2,
                    Languages.Accept);
                return;
            }
            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Messagge,
                    Languages.Accept);
                return;
            }

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var profile = await this.apiService.PutProfile(
                apiSecurity,
                "/api",
                "/ProfilePhones/PutProfilePhone",
                profilePhone);

            this.IsRunning = false;
            this.IsEnabled = true;

            MainViewModel.GetInstance().ProfilesByPhone.updateProfile(profile);

            await App.Navigator.PopAsync();
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }
        private async void Delete()
        {
            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Messagge,
                    Languages.Accept);
                return;
            }
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.DeleteRelationPhone(
                apiSecurity,
                "/api",
                "/Box_ProfilePhone/DeleteBox_ProfilePhoneRelations",
                profilePhone.ProfilePhoneId);

            var response2 = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/ProfilePhones",
                profilePhone.ProfilePhoneId);

            this.IsRunning = false;
            this.IsEnabled = true;

            MainViewModel.GetInstance().Profiles.removeProfilePhone();

            await App.Navigator.PopAsync();
        }

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
            Application.Current.MainPage = new MasterPage();
        }
        #endregion
    }
}