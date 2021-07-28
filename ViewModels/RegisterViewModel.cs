namespace mynfo.ViewModels
{
    using Domain;
    using GalaSoft.MvvmLight.Command;
    using mynfo.Helpers;
    using mynfo.Views;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class RegisterViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }
        public string FirstName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }       
        #endregion

        #region Constructor
        public RegisterViewModel()
        {
            this.apiService = new ApiService();
            this.ImageSource = "no_image";
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand NextCommand
        {
            get
            {
                return new RelayCommand(Next);
            }
        }
        private async void Next()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameValidation,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameValidation,
                    Languages.Accept);
                return;
            }
            
            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnection = await this.apiService.CheckConnection();
            if (!checkConnection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnection.Messagge,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var user = new User
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                ImageArray = imageArray,
                UserTypeId = 1,
            };
            MainViewModel.GetInstance().Register2 = new Register2ViewModel(user);
            await Application.Current.MainPage.Navigation.PushAsync(new Register2Page());
        }

       public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }
        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                if(CrossMedia.Current.IsCameraAvailable &&
                    CrossMedia.Current.IsTakePhotoSupported)
                {
                    var source = await Application.Current.MainPage.DisplayActionSheet(
                        Languages.SourceImageQuestion,
                        Languages.Cancel,
                        null,
                        Languages.FromGallery,
                        Languages.FromCamera);

                    if(source == Languages.Cancel)
                    {
                        this.file = null;
                        return;
                    }

                    if(source == Languages.FromCamera)
                    {
                        this.file = await CrossMedia.Current.TakePhotoAsync(
                            new StoreCameraMediaOptions
                            {
                                CompressionQuality = 80,
                                SaveToAlbum = true,
                                Directory = "mynfo",
                                Name = "picture.jpg",
                                PhotoSize = PhotoSize.Small,
                            }
                        );
                    }
                    else
                    {
                        this.file = await CrossMedia.Current.PickPhotoAsync(
                            new PickMediaOptions
                            {
                                CompressionQuality = 80,
                            });
                    }
                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        CompressionQuality = 80,
                    });
                }
                if (this.file != null)
                {
                    this.ImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                }
            return;
            }
   
            catch(Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }
        #endregion
    }
}