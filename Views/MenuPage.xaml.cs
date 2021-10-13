namespace mynfo.Views
{
    using ViewModels;
    using System;
    using System.Data.SqlClient;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using mynfo.Services;
    using mynfo.Helpers;
    using mynfo.Domain;
    using System.Threading.Tasks;
    using mynfo.Models;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Properties
        public UserLocal User
        {
            get;
            set;
        }
        public User User2
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public MenuPage()
        {            
            User = MainViewModel.GetInstance().User;
            InitializeComponent();                        
            
            if (User.Share == true)
            {
                TagLabel.Text = "myTAG ON";
            }
            else
            {
                TagLabel.Text = "myTAG OFF";
            }
            //get_share();
        }
        #endregion

        #region Methods
        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MenuItemViewModel selectedItem = e.SelectedItem as MenuItemViewModel;
        }
        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            MenuItemViewModel tappedItem = e.Item as MenuItemViewModel;
        }
        async void OnToggled(object sender, ToggledEventArgs e)
        {            
            try
            {
                bool value = e.Value;                
                
                if (value == true) 
                {                     
                    TagLabel.Text = "myTAG ON"; 
                }
                else if (value == false) 
                {                     
                    TagLabel.Text = "myTAG OFF"; 
                }                
                UpdateUser(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async void UpdateUser(bool value)
        {
            await MainViewModel.GetInstance().MenuItem.GetUser(value);
            User2 = MainViewModel.GetInstance().MenuItem.User1;
            bool Share = User2.Share;
            MainViewModel.GetInstance().User.Share = Share;
        }
        public async void get_share()
        {                        
            await MainViewModel.GetInstance().MenuItem.GetUser1();
            User2 = MainViewModel.GetInstance().MenuItem.User1;
            bool Share = User2.Share;
            MainViewModel.GetInstance().User.Share = Share;
        }
        
        #endregion

        #region Commands
        private void GotoMyProfile_Clicked(object sender, EventArgs e)
        {
            MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
            App.Navigator.PushAsync(new MyProfilePage());
        }
        #endregion
    }
}