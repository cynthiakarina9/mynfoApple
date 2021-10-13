namespace mynfo.Views
{
    using Domain;
    using Helpers;
    using Models;
    using Resources1;
    using Rg.Plugins.Popup.Extensions;
    using Services;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsBoxEdithPage
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        public Entry BxNameEntry = new Entry();
        public string boxName; 
        public string ColorH;
        #endregion

        #region Properties
        private ObservableCollection<ProfileLocal> ProfilesSelected { get; set; }
        public ProfileLocal selectedItem { get; set; }
        public String BoxName { get; set; }
        public Box Box { get; set; }
        #endregion

        #region Constructor
        public DetailsBoxEdithPage(Box _box)
        {
            InitializeComponent();
            OSAppTheme currentTheme = Application.Current.RequestedTheme;            
            NavigationPage.SetHasNavigationBar(this, false);
            FullBackGround.BackgroundColor = Color.Transparent;
            Box = _box;
            apiService = new ApiService();
            FullBackGround.CloseWhenBackgroundIsClicked = true;
            ProfilesSelected = new ObservableCollection<ProfileLocal>();
            NameEntry.Text = _box.Name;
            int BoxId = _box.BoxId;
            int UserID = MainViewModel.GetInstance().User.UserId;
            bool BoxDefault = false;
            var BxSaveName = new Button();
            var BxBtnDelete = new ImageButton();
            var bxBtnHome = new ImageButton();
            var BxDefaultCheckBox = new CheckBox();

            #region Color                    
            //Definir color de fondo con respecto a si la box es predeterminada
            if (currentTheme == OSAppTheme.Dark)
            {
                BackG.BackgroundColor = Color.FromHex("#222b3a");
                bxBtnHome.BackgroundColor = Color.FromHex("#222b3a");
                BxSaveName.BackgroundColor = Color.FromHex("#222b3a");
                BxBtnDelete.BackgroundColor = Color.FromHex("#222b3a");
                DeleteButton.Source = "trash3";
                ColorBtn.Source = "pelta3";
            }
            else
            {
                BackG.BackgroundColor = Color.FromHex("#FFFFFF");
                bxBtnHome.BackgroundColor = Color.FromHex("#FFFFFF");
                BxSaveName.BackgroundColor = Color.FromHex("#FFFFFF");
                BxBtnDelete.BackgroundColor = Color.FromHex("#FFFFFF");
                DeleteButton.Source = "trash2";
                ColorBtn.Source = "paleta";
            }

            //Acción de boton de borrado
            DeleteButton.Clicked += new EventHandler((sender, e) => deleteBox(sender, e, BoxId, UserID, BoxDefault));

            //Acción de botón actualización de Box
            BoxUpdateBtn.Clicked += new EventHandler((sender, e) => UpdateBoxName(sender, e, BoxId, BxNameEntry.Text, UserID, BxNameEntry.IsReadOnly));

            //Acción de cambiar color de Box
            ColorBtn.Clicked += new EventHandler((sender, e) => ChangeColor(sender, e, _box));
            #endregion
        }
        #endregion

        #region Methods

        async void deleteBox(object sender, EventArgs e, int _BoxId, int _UserId, bool _BoxDefault)
        {
            bool answer = await DisplayAlert(Resource.Warning, Resource.DeleteBoxNotification, Resource.Yes, Resource.No);
            #region If
            if (answer == true)
            {
                MainViewModel.GetInstance().DetailsBoxEdith.DeleteBox(Box);
            }
            #endregion           
        }

        private async void UpdateBoxName(object sender, EventArgs e, int _BoxId, string _name, int _UserId, bool disabled)
        {
            //Actualizar el nombre de la Box
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            Box = await this.apiService.GetBox(
                  apiSecurity,
                  "/api",
                  "/Boxes",
                  _BoxId);
            var box3 = new Box();
            box3 = new Box
            {
                BoxId = Box.BoxId,
                BoxDefault = Box.BoxDefault,
                Name = NameEntry.Text,
                UserId = Box.UserId,
                Time = Box.Time,
                ColorBox = Box.ColorBox
            };            

            await MainViewModel.GetInstance().DetailsBoxEdith.EdithBox(box3);

            foreach (ProfileLocal Prof in ProfilesSelected)
            {
                if (Prof.Logo == "mail3")
                {
                    DeleteProfileEmail(_BoxId, Prof.ProfileId);
                    ProfileEmail E = Converter.ToProfileEmail(Prof);
                    MainViewModel.GetInstance().DetailsBox.removeProfileEmail(E);
                }
                else if (Prof.Logo == "tel3")
                {
                    DeleteProfilePhone(_BoxId, Prof.ProfileId);
                    ProfilePhone P = Converter.ToProfilePhone(Prof);
                    MainViewModel.GetInstance().DetailsBox.removeProfilePhone(P);
                }
                else if (Prof.Logo == "whatsapp3")
                {
                    DeleteProfileWhatsapp(_BoxId, Prof.ProfileId);
                    ProfileWhatsapp W = Converter.ToProfileWhatsapp(Prof);
                    MainViewModel.GetInstance().DetailsBox.removeProfileW(W);
                }
                else if (Prof.Logo != "mail3" && Prof.Logo != "tel3" && Prof.Logo != "whatsapp3")
                {
                    DeleteProfileSM(_BoxId, Prof.ProfileId);
                    ProfileSM SM = Converter.ToProfileSM(Prof);
                    MainViewModel.GetInstance().DetailsBox.removeProfileSM(SM);
                }
            }

            await Navigation.PopPopupAsync();
        }

        public async void DeleteProfileEmail(int _box, int _profileEmailId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Messagge,
                    Languages.Accept);
                await App.Navigator.PopAsync();
            }

            Box_ProfileEmail box_ProfileEmail = new Box_ProfileEmail
            {
                BoxId = _box,
                ProfileEmailId = _profileEmailId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_Email = await this.apiService.GetIdRelation(
               apiSecurity,
               "/api",
               "/Box_ProfileEmail/GetBox_ProfileEmail",
               box_ProfileEmail);
            var profileEmail = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfileEmail",
                idBox_Email.Box_ProfileEmailId);
        }

        public async void DeleteProfilePhone(int _box, int _profilePhoneId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Messagge,
                    Languages.Accept);
                await App.Navigator.PopAsync();
            }

            Box_ProfilePhone box_ProfilePhone = new Box_ProfilePhone
            {
                BoxId = _box,
                ProfilePhoneId = _profilePhoneId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_Phone = await this.apiService.GetIdRelation(
                apiSecurity,
                "/api",
                "/Box_ProfilePhone/GetBox_ProfilePhone",
                box_ProfilePhone);

            var profilePhone = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfilePhone",
                idBox_Phone.Box_ProfilePhoneId);
        }

        public async void DeleteProfileSM(int _box, int _profileSMId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Messagge,
                    Languages.Accept);
                await Navigation.PopPopupAsync();
            }

            Box_ProfileSM box_ProfileSM = new Box_ProfileSM
            {
                BoxId = _box,
                ProfileMSId = _profileSMId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_SM = await this.apiService.GetIdRelation(
                apiSecurity,
                "/api",
                "/Box_ProfileSM/GetBox_ProfileSM",
                box_ProfileSM);

            var profileSM = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfileSM",
                idBox_SM.Box_ProfileSMId);
        }

        public async void DeleteProfileWhatsapp(int _box, int _profileWhatsappId)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Messagge,
                    Languages.Accept);
                await App.Navigator.PopAsync();
            }

            Box_ProfileWhatsapp box_ProfileWhatsapp = new Box_ProfileWhatsapp
            {
                BoxId = _box,
                ProfileWhatsappId = _profileWhatsappId
            };
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var idBox_Whatsapp = await this.apiService.GetIdRelation(
                apiSecurity,
                "/api",
                "/Box_ProfileWhatsapp/GetBox_ProfileWhatsapp",
                box_ProfileWhatsapp);

            var profileWhatsapp = await this.apiService.Delete(
                apiSecurity,
                "/api",
                "/Box_ProfileWhatsapp",
                idBox_Whatsapp.Box_ProfileWhatsappId);
        }

        void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            selectedItem = e.CurrentSelection.FirstOrDefault() as ProfileLocal;
            ProfilesSelected.Add(selectedItem);
            if (selectedItem.Logo == "mail3")
            {
                ProfileEmail E = Converter.ToProfileEmail(selectedItem);
                MainViewModel.GetInstance().DetailsBoxEdith.removeProfileEmail(E);
            }
            else if (selectedItem.Logo == "tel3")
            {
                ProfilePhone P = Converter.ToProfilePhone(selectedItem);
                MainViewModel.GetInstance().DetailsBoxEdith.removeProfilePhone(P);
            }
            else if (selectedItem.Logo == "whatsapp3")
            {
                ProfileWhatsapp W = Converter.ToProfileWhatsapp(selectedItem);
                MainViewModel.GetInstance().DetailsBoxEdith.removeProfileW(W);
            }
            else if (selectedItem.Logo != "mail3" && selectedItem.Logo != "tel3" && selectedItem.Logo != "whatsapp3")
            {
                ProfileSM SM = Converter.ToProfileSM(selectedItem);
                MainViewModel.GetInstance().DetailsBoxEdith.removeProfileSM(SM);
            }
        }

        async void ChangeColor(object sender, EventArgs e, Box _Box)
        {
            await Navigation.PushPopupAsync(new ColorPickerPopUp(_Box));
        }

        

        #endregion

    }
}