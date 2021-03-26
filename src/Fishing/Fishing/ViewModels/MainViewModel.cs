using Fishing.Models;
using MvvmHelpers;
using System;
using System.Text;

namespace Fishing.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public string UserImage { get; set; }
        public string Location { get; set; }
        public string Temp { get; set; }
        public string CurrentDate { get; set; }
        public string WeatherCondition { get; set; }
        public string WeatherIcon { get; set; }
        public string ChanceOfRain { get; set; }
        public ObservableRangeCollection<FishingLocationviewModel> FishingLocations {get;set;}

        public MainViewModel()
        {
            Location = "Mataram";
            Temp = "30°C";
            CurrentDate = DateTime.Now.ToString("ddd, dd MMMM yyyy").Replace(".", "");
            WeatherCondition = "Sunny Day";
            WeatherIcon = "SunnyIcon";
            ChanceOfRain = "15%";
            UserImage = "https://randomuser.me/api/portraits/men/43.jpg";


            var firstLocation = new FishingLocationviewModel(
                new FishingLocationModel()
                {
                    Name = "South Mandalika Lake",
                    LatLong = "8°53'54.2“S 116°18'14.2“E",
                    LocationThumbnail = "https://raw.githubusercontent.com/kphillpotts/Fishing/main/images/Location1.png",
                    MapThumbnail = "https://raw.githubusercontent.com/kphillpotts/Fishing/main/images/Location1Map.png",
                    People = new System.Collections.Generic.List<PeopleModel>()
                    {
                        new PeopleModel() 
                        {
                            Name="James",
                            Image="https://randomuser.me/api/portraits/men/13.jpg"
                        },
                        new PeopleModel()
                        {
                            Name="Jenny",
                            Image="https://randomuser.me/api/portraits/women/89.jpg"
                        },
                        new PeopleModel()
                        {
                            Name="Rose",
                            Image="https://randomuser.me/api/portraits/women/19.jpg"
                        },
                    },
                    Fish = new System.Collections.Generic.List<FishModel>()
                    {
                        new FishModel()
                        {
                            Name="Snapper",
                            Image = "https://raw.githubusercontent.com/kphillpotts/Fishing/main/images/Fish2.png"
                        },
                        new FishModel()
                        {
                            Name="BlueFish",
                            Image = "https://raw.githubusercontent.com/kphillpotts/Fishing/main/images/Fish1.png"
                        }
                    }
                });

            var secondLocation = new FishingLocationviewModel(
                new FishingLocationModel()
                {
                    Name = "North Mandalika Lake",
                    LatLong = "8°53'54.2“S 116°18'14.2“E",
                    LocationThumbnail = "https://raw.githubusercontent.com/kphillpotts/Fishing/main/images/Location2.png",
                    MapThumbnail = "https://raw.githubusercontent.com/kphillpotts/Fishing/main/images/Location2Map.png",
                    People = new System.Collections.Generic.List<PeopleModel>()
                    {
                        new PeopleModel()
                        {
                            Name="Frank",
                            Image="https://randomuser.me/api/portraits/men/11.jpg"
                        },
                        new PeopleModel()
                        {
                            Name="Jenny",
                            Image="https://randomuser.me/api/portraits/women/89.jpg"
                        },
                        new PeopleModel()
                        {
                            Name="Rose",
                            Image="https://randomuser.me/api/portraits/women/39.jpg"
                        },
                        new PeopleModel()
                        {
                            Name="Dragon",
                            Image="https://randomuser.me/api/portraits/men/12.jpg"
                        },
                        new PeopleModel()
                        {
                            Name="Jlzner",
                            Image="https://randomuser.me/api/portraits/men/15.jpg"
                        },
                        new PeopleModel()
                        {
                            Name="Bandros",
                            Image="https://randomuser.me/api/portraits/men/11.jpg"
                        },
                    },
                    Fish = new System.Collections.Generic.List<FishModel>()
                    {
              
                        new FishModel()
                        {
                            Name="BlueFish",
                            Image = "https://raw.githubusercontent.com/kphillpotts/Fishing/main/images/Fish3.png"
                        }
                    }
                });

            FishingLocations = new ObservableRangeCollection<FishingLocationviewModel>();
            FishingLocations.Add(firstLocation);
            FishingLocations.Add(secondLocation);

        }

    }

}

