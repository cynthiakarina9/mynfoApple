using Foundation;
using mynfoApple.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace mynfoApple.Infraestructure
{
    public class InstanceLocator
    {
        #region Properties
        public MainViewModel Main { get; set; }
        #endregion

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}