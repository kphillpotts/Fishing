using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("OpenSans-Bold.ttf", Alias = "BoldFont")]
[assembly: ExportFont("OpenSans-Regular.ttf", Alias = "RegularFont")]
[assembly: ExportFont("OpenSans-SemiBold.ttf", Alias = "SemiBoldFont")]
namespace Fishing
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
