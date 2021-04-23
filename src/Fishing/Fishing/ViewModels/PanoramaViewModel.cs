using Fishing.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fishing.ViewModels
{
    public class PanoramaViewModel : BaseViewModel
    {
        private FishingLocationModel location;
        private FishModel selectedFish;

        public string WeatherIcon { get; set; }

        public FishModel SelectedFish 
        { 
            get => selectedFish; 
            set => SetProperty(ref selectedFish, value); 
        }

        public string LocationName { get; set; }
        public string LatLong { get; set; }

        public PanoramaViewModel(FishingLocationViewModel locationViewModel)
        {
            this.location = locationViewModel.Location;
        }

        public FishingLocationModel Location
        {
            get => location;
            private set => location = value;
        }
    }
}
