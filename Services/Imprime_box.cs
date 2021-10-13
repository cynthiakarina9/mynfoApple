namespace mynfo.Services
{
    using Domain;
    using Models;
    using Rg.Plugins.Popup.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using ViewModels;
    using Views;
    using Xamarin.Forms;
    using Device = Xamarin.Forms.Device;

    public class Imprime_box : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        private bool notNull1, notNull2, notNull3, notNull4;
        private Box box;
        private ObservableCollection<ProfileEmail> profileEmail;
        private ObservableCollection<ProfilePhone> profilePhone;
        private ObservableCollection<ProfileSM> profileSM;
        private ObservableCollection<ProfileWhatsapp> profileWhatsapp;
        private ObservableCollection<ProfileLocal> profilePerfiles;
        #endregion

        #region Properties
        public bool NotNull1
        {
            get { return this.notNull1; }
            set { SetValue(ref this.notNull1, value); }
        }

        public bool NotNull2
        {
            get { return this.notNull2; }
            set { SetValue(ref this.notNull2, value); }
        }

        public bool NotNull3
        {
            get { return this.notNull3; }
            set { SetValue(ref this.notNull3, value); }
        }

        public bool NotNull4
        {
            get { return this.notNull4; }
            set { SetValue(ref this.notNull4, value); }
        }

        public Box Box
        {
            get { return this.box; }
            set { SetValue(ref this.box, value); }
        }

        public ObservableCollection<ProfileEmail> ProfileEmail
        {
            get { return profileEmail; }
            private set
            {
                SetValue(ref profileEmail, value);
            }
        }

        public ObservableCollection<ProfilePhone> ProfilePhone
        {
            get { return profilePhone; }
            private set
            {
                SetValue(ref profilePhone, value);
            }
        }

        public ObservableCollection<ProfileSM> ProfileSM
        {
            get { return profileSM; }
            private set
            {
                SetValue(ref profileSM, value);
            }
        }

        public ObservableCollection<ProfileWhatsapp> ProfileWhatsapp
        {
            get { return profileWhatsapp; }
            private set
            {
                SetValue(ref profileWhatsapp, value);
            }
        }

        public ObservableCollection<ProfileLocal> ProfilePerfiles
        {
            get { return profilePerfiles; }
            private set
            {
                SetValue(ref profilePerfiles, value);
            }
        }

        public ProfileSM selectedProfileSM { get; set; }
        #endregion

        #region Constructor
        public Imprime_box()
        {
            apiService = new ApiService();
            Box = new Box();
        }
        #endregion

        public async void Consulta_user(string user_id, string tag_id, string nameUser)
        {
            try
            {
                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();

                //Get User
                var user = await apiService.GetUser(
                            apiSecurity,
                            "/api",
                            "/Users/",
                            Int32.Parse(user_id));

                if (nameUser == user.FullName || nameUser == "mynfo")
                {
                    //Get Box Default

                    if (user.Share != true)
                    {
                        user_id = "1";
                    }
                    await GetBoxe(Convert.ToInt32(user_id));
                    int get_box_id = Box.BoxId;
                    InsertForeignData(Convert.ToInt32(user_id), get_box_id);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                          Helpers.Languages.Error,
                          Helpers.Languages.InvalidUser,
                          Helpers.Languages.Accept);
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void InsertForeignData(int user_id, int box_id)
        {
            try
            {
                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                AddViewToUser(user_id);
                var ForeingUser = await apiService.GetUserId(apiSecurity,
                                                    "/api",
                                                    "/Users",
                                                    user_id);
                using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                {
                    connSQLite.CreateTable<ForeingProfile>();
                }

                ForeingBox foreingBox;
                ForeingBox A = new ForeingBox();
                ForeingBox oldForeing = new ForeingBox();

                //Validar que la box no exista
                using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                {
                    A = connSQLite.FindWithQuery<ForeingBox>("select * from ForeingBox where ForeingBox.BoxId = " + box_id + " and ForeingBox.UserRecivedId = " + MainViewModel.GetInstance().User.UserId);
                }

                if (A == null)
                {
                    //Inicializar la box foranea
                    foreingBox = new ForeingBox
                    {
                        BoxId = box_id,
                        UserId = user_id,
                        Time = DateTime.Now,
                        ImagePath = ForeingUser.ImagePath,
                        UserTypeId = ForeingUser.UserTypeId,
                        FirstName = ForeingUser.FirstName,
                        LastName = ForeingUser.LastName,
                        Edad = ForeingUser.Edad,
                        Ubicacion = ForeingUser.Ubicacion,
                        Ocupacion = ForeingUser.Ocupacion,
                        Conexiones = ForeingUser.Conexiones,
                        UserRecivedId = MainViewModel.GetInstance().User.UserId
                    };

                    //Insertar la box foranea
                    using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                    {
                        connSQLite.CreateTable<ForeingBox>();
                        connSQLite.Insert(foreingBox);
                    }
                }
                else
                {
                    using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                    {
                        oldForeing = A;
                        connSQLite.ExecuteScalar<ForeingBox>("UPDATE ForeingBox SET Conexiones = ? WHERE ForeingBox.UserId = ?", ForeingUser.Conexiones, ForeingUser.UserId);
                        connSQLite.ExecuteScalar<ForeingBox>("UPDATE ForeingBox SET Time = ? WHERE ForeingBox.BoxId = ?", DateTime.Now, A.BoxId);
                        A = connSQLite.FindWithQuery<ForeingBox>("select * from ForeingBox where ForeingBox.BoxId = ?", box_id);
                    }
                    foreingBox = A;
                }

                if (box_id != 0)
                {
                    if (A != null)
                    {
                        using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                        {
                            connSQLite.Execute("Delete from ForeingProfile Where ForeingProfile.BoxId = ?", A.BoxId);
                        }
                    }

                    #region Foreing Profiles New Code                    
                    await GetListEmail(user_id, box_id);
                    await GetListPhone(user_id, box_id);
                    await GetListSM(user_id, box_id);
                    await GetListWhatsapp(user_id, box_id);
                    #endregion                    

                    if (NotNull1 == true && NotNull2 == true && NotNull3 == true && NotNull4 == true)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MainViewModel.GetInstance().ForeingBox = new ForeingBoxViewModel(foreingBox);
                            if (PopupNavigation.PopupStack.Count != 0)
                            {
                                PopupNavigation.Instance.PopAllAsync();
                            }

                            if (A == null)
                            {
                                MainViewModel.GetInstance().ListForeignBox.AddList(foreingBox);
                            }
                            else
                            {
                                //Box anterior
                                //oldForeing
                                MainViewModel.GetInstance().ListForeignBox.UpdateList(foreingBox.UserId);

                            }
                            PopupNavigation.Instance.PushAsync(new ForeingBoxPage(foreingBox, true));
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async void AddViewToUser(int _UserId)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var user = await apiService.GetUser(
                apiSecurity,
                "/api",
                "/Users/",
                _UserId);

            var userDomain = new User
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                UserTypeId = user.UserTypeId,
                Share = user.Share,
                Edad = user.Edad,
                Ubicacion = user.Ubicacion,
                Ocupacion = user.Ocupacion,
                Conexiones = user.Conexiones + 1,
                Fecha = user.Fecha,
            };

            var response = await apiService.Put(
                apiSecurity,
                "/api",
                "/Users",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                userDomain);
            if (response.IsSuccess == false)
            {
                return;
            }
        }

        #region Box
        public async Task<Box> GetBoxe(int UserId)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            Box = await this.apiService.GetBoxDefault<Box>(
                apiSecurity,
                "/api",
                "/Boxes/GetBoxDefault",
                UserId);

            return Box;
        }
        #endregion

        #region Email
        public async Task<bool> GetListEmail(int _UserId, int _BoxId)
        {
            try
            {
                List<ProfileEmail> listEmail = new List<ProfileEmail>();
                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                listEmail = await this.apiService.GetListByUser<ProfileEmail>(
                            apiSecurity,
                            "/api",
                            "/ProfileEmails",
                            _UserId);
                foreach (ProfileEmail ItemEmail in listEmail)
                {
                    Box_ProfileEmail RelationEmail;
                    RelationEmail = new Box_ProfileEmail
                    {
                        BoxId = _BoxId,
                        ProfileEmailId = ItemEmail.ProfileEmailId
                    };

                    var response = await this.apiService.Get(
                        apiSecurity,
                        "/api",
                        "/Box_ProfileEmail/GetBox_ProfileEmail",
                        RelationEmail);

                    ItemEmail.Exist = response.IsSuccess;

                    if (ItemEmail.Exist == true)
                    {
                        var foreingProfile = new ForeingProfile
                        {
                            BoxId = _BoxId,
                            UserId = ItemEmail.UserId,
                            ProfileName = ItemEmail.Name,
                            value = ItemEmail.Email,
                            ProfileType = "Email"
                        };
                        //Crear perfil de correo de box local predeterminada
                        using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                        {
                            connSQLite.Insert(foreingProfile);
                        }
                    }
                }
                NotNull1 = true;
                return NotNull1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                NotNull1 = false;
                return NotNull1;
            }
        }
        #endregion

        #region Phone
        public async Task<bool> GetListPhone(int _UserId, int _BoxId)
        {
            try
            {
                List<ProfilePhone> listPhone = new List<ProfilePhone>();
                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                listPhone = await this.apiService.GetListByUser<ProfilePhone>(
                    apiSecurity,
                    "/api",
                    "/ProfilePhones",
                    _UserId);
                foreach (ProfilePhone ItemPhone in listPhone)
                {
                    Box_ProfilePhone RelationPhone;
                    RelationPhone = new Box_ProfilePhone
                    {
                        BoxId = _BoxId,
                        ProfilePhoneId = ItemPhone.ProfilePhoneId
                    };

                    var response = await this.apiService.Get(
                        apiSecurity,
                        "/api",
                        "/Box_ProfilePhone/GetBox_ProfilePhone",
                        RelationPhone);

                    ItemPhone.Exist = response.IsSuccess;
                    if (ItemPhone.Exist == true)
                    {
                        var foreingProfile = new ForeingProfile
                        {
                            BoxId = _BoxId,
                            UserId = ItemPhone.UserId,
                            ProfileName = ItemPhone.Name,
                            value = ItemPhone.Number,
                            ProfileType = "Phone"
                        };
                        //Crear perfil de teléfono de box local predeterminada
                        using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                        {
                            connSQLite.Insert(foreingProfile);
                        }
                    }
                }
                NotNull2 = true;
                return NotNull2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                NotNull2 = false;
                return NotNull2;
            }
        }
        #endregion

        #region SM
        public async Task<bool> GetListSM(int _UserId, int _BoxId)
        {
            try
            {
                List<ProfileSM> listSM = new List<ProfileSM>();
                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();

                listSM = await this.apiService.GetListByUser<ProfileSM>(
                    apiSecurity,
                    "/api",
                    "/ProfileSMs",
                    _UserId);
                foreach (ProfileSM ItemSM in listSM)
                {
                    Box_ProfileSM RelationSM;
                    RelationSM = new Box_ProfileSM
                    {
                        BoxId = _BoxId,
                        ProfileMSId = ItemSM.ProfileMSId
                    };

                    var response = await this.apiService.Get(
                        apiSecurity,
                        "/api",
                        "/Box_ProfileSM/GetBox_ProfileSM",
                        RelationSM);
                    ItemSM.Exist = response.IsSuccess;

                    if (ItemSM.Exist == true)
                    {
                        var profileSM = await this.apiService.GetRS(
                           apiSecurity,
                           "/api",
                           "/RedSocials",
                           ItemSM.RedSocialId);

                        var foreingProfileSM = new ForeingProfile
                        {
                            BoxId = _BoxId,
                            UserId = ItemSM.UserId,
                            ProfileName = ItemSM.ProfileName,
                            value = ItemSM.link,
                            ProfileType = profileSM.Name,
                        };
                        //Crear perfil de redes sociales de box local predeterminada
                        using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                        {
                            connSQLite.Insert(foreingProfileSM);
                        }
                    }
                }
                NotNull3 = true;
                return NotNull3;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                NotNull3 = false;
                return NotNull3;
            }
        }
        #endregion

        #region Whatsapp
        public async Task<bool> GetListWhatsapp(int _UserId, int _BoxId)
        {
            try
            {
                List<ProfileWhatsapp> listWhatsapp = new List<ProfileWhatsapp>();
                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                listWhatsapp = await this.apiService.GetListByUser<ProfileWhatsapp>(
                                apiSecurity,
                                "/api",
                                "/ProfileWhatsapps",
                                _UserId);
                foreach (ProfileWhatsapp ItemWhatsapp in listWhatsapp)
                {
                    Box_ProfileWhatsapp RelationWhatsapp;
                    RelationWhatsapp = new Box_ProfileWhatsapp
                    {
                        BoxId = _BoxId,
                        ProfileWhatsappId = ItemWhatsapp.ProfileWhatsappId
                    };
                    //apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                    var response = await this.apiService.Get(
                        apiSecurity,
                        "/api",
                        "/Box_ProfileWhatsapp/GetBox_ProfileWhatsapp",
                        RelationWhatsapp);

                    ItemWhatsapp.Exist = response.IsSuccess;
                    if (ItemWhatsapp.Exist == true)
                    {
                        ForeingProfile foreingProfile = new ForeingProfile
                        {
                            BoxId = _BoxId,
                            UserId = ItemWhatsapp.UserId,
                            ProfileName = ItemWhatsapp.Name,
                            value = ItemWhatsapp.Number,
                            ProfileType = "Whatsapp"
                        };
                        //Crear perfil de whatsapp de box local predeterminada
                        using (var connSQLite = new SQLite.SQLiteConnection(App.root_db))
                        {
                            connSQLite.Insert(foreingProfile);
                        }
                    }
                }
                NotNull4 = true;
                return NotNull4;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                NotNull4 = false;
                return NotNull4;
            }
        }
        #endregion

    }
}