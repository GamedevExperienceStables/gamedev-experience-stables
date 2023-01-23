namespace Game.Level
{
    public class LocationData
    {
        public LocationPointData LastLocation { get; set; }
        public LocationPointData CurrentLocation { get; set; }
        
        public bool allowExit;
        public bool allowInventory;
    }
}