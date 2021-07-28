[assembly: Xamarin.Forms.Dependency(typeof(mynfo.Implementations.Localize))]
namespace mynfo.Implementations
{
    using Interfaces;
    using System.Globalization;
    using System.Threading;

    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            CultureInfo ci = null;

            return ci;
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}