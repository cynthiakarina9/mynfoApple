namespace mynfo.ViewModels
{
    using Models;
    using Services;
    using System;
    using System.Text;
    using Xamarin.Forms;

    public class MyQRViewModel : BaseViewModel
    {
        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        private string user;
        private UserLocal userLocal;
        private ImageSource imageSource;
        #endregion

        #region Properties
        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                SetValue(ref this.user, value);
            }
        }
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }
        public UserLocal UserLocal
        {
            get
            {
                return this.userLocal;
            }
            set
            {
                SetValue(ref this.userLocal, value);
            }
        }
        #endregion

        #region Contructor
        public MyQRViewModel()
        {
            apiService = new ApiService();
            UserLocal = MainViewModel.GetInstance().User;
            if (this.UserLocal.ImageFullPath == "noimage")
            {
                this.ImageSource = "no_image";
            }
            else
            {
                this.ImageSource = this.UserLocal.ImageFullPath;
            }
            GetDato(UserLocal);
        }
        #endregion

        #region Methods
        public string GetDato(UserLocal U)
        {
            //User = "https://boxweb.azurewebsites.net/index3.aspx?user_id=" + U.UserId + "&tag_id=";
            //return User;
            string result = string.Empty;

            result = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(U.UserId)));
            var userName = Convert.ToBase64String(Encoding.UTF8.GetBytes(U.FullName));
            var userTagId = Convert.ToBase64String(Encoding.UTF8.GetBytes("Tag_Id"));
            User = "https://boxweb.azurewebsites.net/index3.aspx?" + userName + "?" + result + "?" + userTagId + "?";

            if (MainViewModel.GetInstance().User.UserId == 1)
            {
                User = "https://boxweb.azurewebsites.net/index3.aspx?user_id=1&tag_id=";
            }

            return User;
        }
        #endregion
    }
}