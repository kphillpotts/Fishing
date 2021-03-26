using System.Collections.Generic;

namespace Fishing.Models
{
    public class FishingLocationModel
    {
        public string Name { get; set; }
        public string LatLong { get; set; }
        public List<PeopleModel> People { get; set; }
        public List<FishModel> Fish { get; set; }
        public string LocationThumbnail { get; set; }
        public string MapThumbnail { get; set; }
    }

}

