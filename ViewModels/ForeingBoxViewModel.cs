namespace mynfo.ViewModels
{
    using Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class ForeingBoxViewModel : BaseViewModel
    {
        #region Atrributes
        private ImageSource imageSource;
        private ObservableCollection<ProfileLocal> profilesF;
        private ForeingBox foreingBox;
        #endregion

        #region Properties
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }
        public ObservableCollection<ProfileLocal> ProfilesF
        {
            get { return profilesF; }
            private set
            {
                SetValue(ref profilesF, value);
            }
        }

        public ForeingBox ForeingBox
        {
            get { return foreingBox; }
            private set
            {
                SetValue(ref foreingBox, value);
            }
        }
        #endregion

        #region Constructor
        public ForeingBoxViewModel(ForeingBox _foreingBox)
        {
            ImageSource = _foreingBox.ImageFullPath;
            DataFill(_foreingBox);
        }
        #endregion

        #region Methods
        public ObservableCollection<ProfileLocal> DataFill(ForeingBox _foreingBox)
        {
            ProfilesF = new ObservableCollection<ProfileLocal>();
            List<ForeingProfile> foreingProfileList;
            using (var conn = new SQLite.SQLiteConnection(App.root_db))
            {
                foreingProfileList = conn.Query<ForeingProfile>("Select * from ForeingProfile where ForeingProfile.BoxId = ?", _foreingBox.BoxId);
            }
            foreach (ForeingProfile Pro in foreingProfileList)
            {
                string Image = string.Empty;
                if (_foreingBox.UserRecivedId == MainViewModel.GetInstance().User.UserId)
                {
                    switch (Pro.ProfileType)
                    {
                        case "Email":
                            Image = "mail2";
                            break;
                        case "Phone":
                            Image = "tel2";
                            break;
                        case "Facebook":
                            Image = "facebook2";
                            break;
                        case "Instagram":
                            Image = "instagramlogo2";
                            break;
                        case "Twitter":
                            Image = "twitterlogo2";
                            break;
                        case "Snapchat":
                            Image = "snapchat2";
                            break;
                        case "LinkedIn":
                            Image = "linkedin2";
                            break;
                        case "TikTok":
                            Image = "tiktok2";
                            break;
                        case "Youtube":
                            Image = "youtube2";
                            break;
                        case "Spotify":
                            Image = "spotify2";
                            break;
                        case "Twitch":
                            Image = "twitch2";
                            break;
                        case "WebPage":
                            Image = "gmail2";
                            break;
                        case "Whatsapp":
                            Image = "whatsapp2";
                            break;
                        case "Telegram":
                            Image = "telegram2";
                            break;
                        default:
                            break;
                    }
                    ProfileLocal Local = new ProfileLocal
                    {
                        IdBox = Pro.BoxId,
                        UserId = Pro.UserId,
                        ProfileName = Pro.ProfileName,
                        value = Pro.value,
                        ProfileType = Pro.ProfileType,
                        Logo = Image,
                    };
                    ProfilesF.Add(Local);
                }
            }
            return ProfilesF;
        }
        #endregion

    }
}