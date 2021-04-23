namespace Fishing.Models
{
    public class FishModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public double AnchorX { get; set; }
        public double AnchorY { get; set; }
        public double[] BiteChart { get; internal set; }
        public string FishSize { get; internal set; }
    }

}

