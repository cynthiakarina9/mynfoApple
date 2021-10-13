namespace mynfo.Views
{
    using Domain;
    using Models;
    using Services;
    using ViewModels;
    using Rg.Plugins.Popup.Services;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using mynfo.Helpers;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailBoxPopUpPage
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attibutes
        public ImageButton BxSaveName = new ImageButton();
        public ImageButton BxBtnDelete = new ImageButton();
        public ImageButton bxBtnHome = new ImageButton();
        //public CheckBox BxDefaultCheckBox = new CheckBox();
        public Entry BxNameEntry = new Entry();
        public String BoxName;
        public bool A, B, C, D;
        #endregion

        #region Properties
        public bool IsTF
        {
            get;
            set;
        }
        public Box Box 
        { 
            get; 
            set; 
        }        
        #endregion

        #region Constructor
        public DetailBoxPopUpPage(Box _Box)
        {
            Box = _Box;
            InitializeComponent();
            apiService = new ApiService();
            NavigationPage.SetHasNavigationBar(this, false);
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            FrameB.CloseWhenBackgroundIsClicked = true;
            int BoxId = _Box.BoxId;
            var boxLocal = new BoxLocal();            
            int UserID = MainViewModel.GetInstance().User.UserId;
            bool BoxDefault = _Box.BoxDefault;

            #region Color                 
            //Definir color de fondo con respecto a si la box es predeterminada
            if (currentTheme == OSAppTheme.Light)
            {
                if (_Box.ColorBox != null)
                {
                    FrameB.BackgroundColor = Color.FromHex(_Box.ColorBox);
                }
                else
                {
                    //FrameB.BackgroundColor = Color.FromHex("#c6c6c6");
                    FrameB.BackgroundColor = Color.FromRgba(198, 198, 198, 0.5);
                }
                BackG.BackgroundColor = Color.FromHex("#FFFFFF");
                bxBtnHome.BackgroundColor = Color.FromHex("#FFFFFF");
                BxSaveName.BackgroundColor = Color.FromHex("#FFFFFF");
                BxBtnDelete.BackgroundColor = Color.FromHex("#FFFFFF");
            }
            else
            {
                if (_Box.ColorBox != null)
                {
                    FrameB.BackgroundColor = Color.FromHex(_Box.ColorBox);
                }
                else
                {
                    //FrameB.BackgroundColor = Color.FromHex("#a4a4a4");
                    FrameB.BackgroundColor = Color.FromRgba(164, 164, 164, 0.5);
                }
                BackG.BackgroundColor = Color.FromHex("#222b3a");
                bxBtnHome.BackgroundColor = Color.FromHex("#222b3a");
                BxSaveName.BackgroundColor = Color.FromHex("#222b3a");
                BxBtnDelete.BackgroundColor = Color.FromHex("#222b3a");
            }
            
            defaultLabel.FontSize = 13;            

            //Navegación a ventana de perfiles
            BoxProfiles.Clicked += new EventHandler((sender, e) => BoxDetails_Clicked(sender, e, BoxId));

            //Botón de Editar
            EdithButton.Clicked += new EventHandler((sender, e) => edithBox(sender, e, _Box));

            //Creación del checkbox de box predeterminada
            BxDefaultCheckBox.IsChecked = BoxDefault;
            BxDefaultCheckBox.VerticalOptions = LayoutOptions.Start;
            
            Thickness thick = new Thickness(20, -6, 0, 0);
            //BoxDefaultCheckBox.Margin = thick;
            
            if (BoxDefault == true)
            {
                BxDefaultCheckBox.IsEnabled = false;
                if (currentTheme == OSAppTheme.Dark)
                {
                    BxDefaultCheckBox.Color = Color.White;
                    EdithButton.Source = "edit3";
                    BoxProfiles.Source = "plusb";
                }
                else
                {
                    BxDefaultCheckBox.Color = Color.FromHex("#FF5521");
                    EdithButton.Source = "edit2";
                    BoxProfiles.Source = "Plus";
                }
            }
            else
            {
                if (currentTheme == OSAppTheme.Dark)
                {
                    BxDefaultCheckBox.IsEnabled = true;
                    BxDefaultCheckBox.Color = Color.FromHex("#FFFFFF");
                    EdithButton.Source = "edit3";
                    BoxProfiles.Source = "plusb";
                }
                else
                {
                    BxDefaultCheckBox.IsEnabled = true;
                    BxDefaultCheckBox.Color = Color.FromHex("#FF5521");
                    EdithButton.Source = "edit2";
                    BoxProfiles.Source = "Plus";
                }
            }          
            #endregion
        }
        #endregion

        #region Methods
        //Marcar o desmarcar la box predeterminada
        async void CheckDefaultBox(object sender, EventArgs e)
        {
            await MainViewModel.GetInstance().DetailsBox.GetListByUser(Box);
        }


        #endregion

        #region Commands
        async void edithBox(object sender, EventArgs e, Box Box)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.DetailsBoxEdith = new DetailsBoxEdithViewModel(Box);
            await PopupNavigation.Instance.PushAsync(new DetailsBoxEdithPage(Box));
        }
        private void BoxDetails_Clicked(object sender, EventArgs e, int _BoxId)
        {
            MainViewModel.GetInstance().ListOfNetworks = new ListOfNetworksViewModel(_BoxId);
            PopupNavigation.Instance.PushAsync(new ListOfNetworksPage(_BoxId));
        }
        #endregion
    }
}