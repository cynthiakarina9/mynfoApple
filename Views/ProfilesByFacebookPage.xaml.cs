namespace mynfo.Views
{
    using Domain;
    using System;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilesByFacebookPage : ContentPage
    {
        #region Constructors
        public ProfilesByFacebookPage()
        {
            InitializeComponent();
            OSAppTheme currentTheme = App.Current.RequestedTheme;
            if (currentTheme == OSAppTheme.Dark)
            {
                Logosuperior.Source = "logo_superior2.png";
            }
            else
            {
                Logosuperior.Source = "logo_superior3.png";
            }

        }
        #endregion

        #region Commands
        private void NewProfileFacebook_Clicked(object sender, EventArgs e)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CreateProfileSM = new CreateProfileSMViewModel("Facebook");
            App.Navigator.PushAsync(new CreateProfileFacebookPage());
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Profiles = new ProfilesViewModel();
            Application.Current.MainPage = new NavigationPage(new ProfilesPage());
        }
        private void BackHome_Clicked(object sender, EventArgs e)
        {
            MainViewModel.GetInstance().Home = new HomeViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ProfileSM selectedItem = e.SelectedItem as ProfileSM;
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            ProfileSM tappedItem = e.Item as ProfileSM;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditProfileFacebook = new EditProfileFacebookViewModel(tappedItem.ProfileMSId);
            App.Navigator.PushAsync(new EditProfileFacebookPage());
        }
        #endregion
    }
}