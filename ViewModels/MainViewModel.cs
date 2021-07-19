using Foundation;
using mynfoApple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace mynfoApple.ViewModels
{
    public class MainViewModel :BaseViewModel
    {
        #region Attributes
        private UserLocal user;
        #endregion

        #region Properties
        public TokenResponse Token { get; set; }

        public UserLocal User { get { return this.user; } set { SetValue(ref this.user, value); } }
        #endregion

        #region ViewModels
        public LoginViewModel login { get; set; }

        public HomeViewModel home { get; set; }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if(instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}