namespace mynfo.Views
{
    using Helpers;
    using System;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TAGPage : ContentPage
    {        
        public static bool write_nfc = false;

        public TAGPage()
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

            Press.Text = Languages.Push + " '" + Languages.ConfigureTAG + "' " + Languages.AndStick;
        }

        void escribir_tag(object sender, EventArgs e)
        {
            try
            {
                var duration = TimeSpan.FromMilliseconds(1000);                
                Vibration.Vibrate(duration);
                DependencyService.Get<IBackgroundDependency>().ExecuteCommand();                
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2);
            }
        }
        public interface IBackgroundDependency
        {
            void ExecuteCommand();
        }
    }
}