using Foundation;
using GalaSoft.MvvmLight.Command;
using mynfoApple.Helpers;
using mynfoApple.Services;
using mynfoApple.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using UIKit;

namespace mynfoApple.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string email, password;
        #endregion

        #region Properties
        public string Email { get { return this.email; } set { SetValue(ref this.email, value); } }

        public string Password { get { return this.password; } set { SetValue(ref this.password, value); } }
        #endregion

        public LoginViewModel()
        {
            this.apiService = new ApiService();

        }

        #region Commands
        public ICommand LoginCommand
        { 
            get 
            {
                return new RelayCommand(Login);
            } 
        }

        public async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "No debe de estar vacío", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "No debe de estar vacío", "Aceptar");
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "No hay conexión", "Aceptar");
            }

            var apiSecurity = Xamarin.Forms.Application.Current.Resources["APISecurity"].ToString();

            var token = await this.apiService.GetToken(apiSecurity, this.Email, this.Password);

            if(token == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Alerta", "Algo ocurrió mal", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Alerta", "Usuario o contraseña incorrectos", "Aceptar");
                return;
            }

            var user = await this.apiService.GetUserByEmail(apiSecurity, "/api", "/Users/GetUserByEmail", token.TokenType, token.AccessToken, this.Email);

            var userLocal = Converter.ToUserLocal(user);

            userLocal.Password = this.Password;

            var mainViewModel = MainViewModel.GetInstance();

            mainViewModel.Token = token;

            mainViewModel.User = userLocal;

            mainViewModel.home = new HomeViewModel();

            Xamarin.Forms.Application.Current.MainPage = new HomePage();
        }
        #endregion
    }
}