namespace mynfo.Views
{
    using Models;
    using ViewModels;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageSizePopUpPage 
    {
        #region Attributes
        public UserLocal User;
        #endregion

        #region Constructor
        public ImageSizePopUpPage()
        {
            InitializeComponent();
            User = MainViewModel.GetInstance().User;
            ImageSize.Source = User.ImageFullPath;
        }
        #endregion
    }
}