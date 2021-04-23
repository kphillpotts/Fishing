using Fishing.Models;
using MvvmHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Fishing.ViewModels
{
    public class FishingLocationViewModel : BaseViewModel
    {
        private FishingLocationModel location;

        public FishingLocationModel Location 
        { 
            get => location; 
            set => location = value; 
        }

        int peopleToShow = 3;

        public string PeopleAtLocation 
        { 
            get
            {
                var peopleCount = location.People.Count;
                var first = location.People.FirstOrDefault();
                if (first == null)
                    return "It's just you";

                var names = location.People.Select(x => x.Name).OrderBy(o => o).Take(peopleToShow).ToList();
                string nameList = string.Join(", ", names);

                if (peopleCount > peopleToShow)
                    return $"{nameList} and {peopleCount - peopleToShow} others";
                else
                    return nameList;
                    
            }
        }
        
        public List<object> PeopleIcons
        {
            get
            {
                var peopleCount = location.People.Count;
                

                List<object> returnList = new List<object>();
                returnList.AddRange(location.People.Take(peopleToShow));

                // do we need a more badge
                if (peopleCount > peopleToShow)
                {
                    returnList.Add(peopleCount - peopleToShow);
                }
                return returnList;
                
            }
        }


        public FishingLocationViewModel(FishingLocationModel location)
        {
            this.location = location;
        }

    }

}

