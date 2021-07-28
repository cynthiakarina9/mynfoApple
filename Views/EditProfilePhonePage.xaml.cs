﻿namespace mynfo.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePhonePage : ContentPage
    {
        #region Constructor
        public EditProfilePhonePage(int _ProfilePhoneId)
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
    }
}