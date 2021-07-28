namespace mynfo.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Rg.Plugins.Popup.Services;
    using System.Windows.Input;
    public class ImageSizeViewModel
    {
        #region Attributes
        public UserLocal User;
        #endregion

        #region Constructor
        public ImageSizeViewModel()
        {
            User = MainViewModel.GetInstance().User;
        }
        #endregion

        #region Commands
        public ICommand TestCommand
        {
            get
            {
                return new RelayCommand(Test);
            }
        }
        private async void Test()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}