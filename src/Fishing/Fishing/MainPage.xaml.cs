using Fishing.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Fishing
{
    public partial class MainPage : ContentPage
    {
        Page onboardingPage;
        public MainPage()
        {
            InitializeComponent();

            if (ShouldShowOnboarding() == true)
            {
                App.Current.ModalPopping += Current_ModalPopping;
                onboardingPage = new OnboardingPage();
                Navigation.PushModalAsync(onboardingPage, false);
            }
        }

        private void Current_ModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == onboardingPage)
            {
                FadeBox.FadeTo(0, 400);
                onboardingPage = null;
                App.Current.ModalPopping -= Current_ModalPopping;
            }
        }

        private bool ShouldShowOnboarding()
        {
            return true;
            //return VersionTracking.IsFirstLaunchEver;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            MainViewModel mainVm = this.BindingContext as MainViewModel;
            var locationViewModel = FishingLocationCarousel.CurrentItem as FishingLocationViewModel;
            PanoramaViewModel panViewModel = new PanoramaViewModel(locationViewModel);
            panViewModel.WeatherIcon = mainVm.WeatherIcon;

            var locationPage = new FishingLocationPage(panViewModel);
            this.Navigation.PushAsync(locationPage);
        }
    }
}
