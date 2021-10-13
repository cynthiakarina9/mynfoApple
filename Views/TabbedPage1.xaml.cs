namespace mynfo.Views
{
    using Models;
    using mynfo.Domain;
    using Rg.Plugins.Popup.Services;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.PlatformConfiguration;
    using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1
    {
        #region Attributes
        ApiService apiService;
        #endregion

        #region Properties
        public int NetworksQty { get; set; }
        #endregion

        public TabbedPage1()
        {            
            InitializeComponent();
            NetworksQty = 0;
            apiService = new ApiService();
            OSAppTheme currentTheme = App.Current.RequestedTheme;
            
            if (currentTheme == OSAppTheme.Dark)
            {                       
                Logosuperior.Source = "logo_superior2.png";
                //BarBackgroundColor = Color.FromHex("#222b3a");
                
                //BarBackgroundColor = Color.FromHex("#222b3a");
            }
            else
            {
                Logosuperior.Source = "logo_superior3.png";
                //BarBackgroundColor = Color.FromHex("#FFFFFF");
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Profiles = new ProfilesViewModel();
            mainViewModel.ListForeignBox = new ListForeignBoxViewModel();

            On<Windows>().SetHeaderIconsEnabled(true);
            On<Windows>().SetHeaderIconsSize(new Size(50, 50));
            BackgroundColor = Color.Transparent;

            Children.Add(new ProfilesPage { IconImageSource = "Networks_icon" });
            Children.Add(new HomePage { IconImageSource = "Home_icon" });
            Children.Add(new ListForeignBoxPage { IconImageSource = "Connections_icon" });

            StartChildren();
        }

        private async void StartChildren()
        {
            await GetEmailQty();
            await GetPhoneQty();
            await GetWhatsAppQty();
            await GetSocialMediaQty();
            CheckIntroductionBool();

            if (NetworksQty == 1)
            {
                CurrentPage = Children[1];
            }
            else
            {
                CurrentPage = Children[0];
            }
        }
        private async Task<int> GetEmailQty()
        {
            var apiSecurity = App.Current.Resources["APISecurity"].ToString();
            
            var listEmail = new List<ProfileEmail>();
            listEmail = await this.apiService.GetListByUser<ProfileEmail>(
                apiSecurity,
                "/api",
                "/ProfileEmails",
                MainViewModel.GetInstance().User.UserId);
            if(listEmail.Count != 0)
            {
                NetworksQty++;
            }
            return NetworksQty;
        }

        private async Task<int> GetPhoneQty()
        {            
            var apiSecurity = App.Current.Resources["APISecurity"].ToString();            
            var listPhone = new List<ProfilePhone>();
            listPhone = await this.apiService.GetListByUser<ProfilePhone>(
                apiSecurity,
                "/api",
                "/ProfilePhones",
                MainViewModel.GetInstance().User.UserId);
            if (listPhone.Count != 0)
            {
                NetworksQty++;
            }
            return NetworksQty;
        }

        private async Task<int> GetWhatsAppQty()
        {            
            var apiSecurity = App.Current.Resources["APISecurity"].ToString();            
            var listWhatsapp = new List<ProfileWhatsapp>();
            listWhatsapp = await this.apiService.GetListByUser<ProfileWhatsapp>(
                apiSecurity,
                "/api",
                "/ProfileWhatsapps",
                MainViewModel.GetInstance().User.UserId);
            if (listWhatsapp.Count != 0)
            {
                NetworksQty++;
            }
            return NetworksQty;
        }

        private async Task<int> GetSocialMediaQty()
        {            
            var apiSecurity = App.Current.Resources["APISecurity"].ToString();            
            var listSM = new List<ProfileSM>();
            listSM = await this.apiService.GetListByUser<ProfileSM>(
                apiSecurity,
                "/api",
                "/ProfileSMs",
                MainViewModel.GetInstance().User.UserId);
            if (listSM.Count != 0)
            {
                NetworksQty++;
            }
            return NetworksQty;
        }

        /// <summary>
        /// Sirve para recargar las conexiones del usuario en home
        /// </summary>
        public static async void ReloadConnections()
        {
            var apiService = new ApiService();
            var apiSecurity = Xamarin.Forms.Application.Current.Resources["APISecurity"].ToString();
            var user = await apiService.GetUserId(apiSecurity,
                                                "/api",
                                                "/Users",
                                                MainViewModel.GetInstance().User.UserId);

            MainViewModel.GetInstance().User.Conexiones = user.Conexiones;
        }

        /// <summary>
        /// Sirve para calcular el tiempo que le queda a las boxes recibidas, y si es que ya expiraron, las borra
        /// </summary>
        public void CheckTimeForeingBox()
        {
            //Detectar las boxes recibidas que van a expirar
            List<ForeingBox> list = new List<ForeingBox>();
            List<ForeingBox> deleteList = new List<ForeingBox>();

            using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
            {
                list = connSQLite.Query<ForeingBox>("select * from ForeingBox");
            }
            if (list.Count > 0)
            {
                foreach (ForeingBox foreing in list)
                {
                    DateTime expiration = foreing.Time.AddMinutes(5);
                    DateTime actual = DateTime.Now;
                    int res = DateTime.Compare(expiration, actual);

                    if (res <= 0)
                    {
                        deleteList.Add(foreing);
                    }

                }
            }

            if (deleteList.Count > 0)
            {
                foreach (ForeingBox foreingDelete in deleteList)
                {
                    using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                    {
                        connSQLite.Query<ForeingBox>("delete from ForeingProfile where ForeingProfile.BoxId = ?", foreingDelete.BoxId);
                        connSQLite.Query<ForeingBox>("delete from ForeingBox where ForeingBox.BoxId = ?", foreingDelete.BoxId);
                    }
                }
            }
        }

        public async void CheckIntroductionBool()
        {
            if (MainViewModel.GetInstance().User.MostrarTutorial == false)
            {
                MainViewModel.GetInstance().IntroductionGif = new IntroductionGifViewModel();
                await PopupNavigation.Instance.PushAsync(new IntroductionGifPage());
            }
        }
    }
}