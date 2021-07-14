using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace mynfoApple.ViewModels
{
    public class MainViewModel :BaseViewModel
    {
        #region ViewModels
        public LoginViewModel login { get; set; }

        public HomeViewModel home { get; set; }
        #endregion
    }
}