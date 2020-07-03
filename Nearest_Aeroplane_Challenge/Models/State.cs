namespace Nearest_Aeroplane_Challenge.Models
{
    public class State
    {
        public string ICAO24 { get; set; }
        public string CallSign { get; set; }
        public string OriginCountry { get; set; }
        public int? TimePosition { get; set; }
        public int LastContact { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? BaroAltitude { get; set; }
        public bool OnGround { get; set; }
        public decimal? Velocity { get; set; }
        public decimal? TrueTrack { get; set; }
        public decimal? VerticalRate { get; set; }
        public int[] Sensors { get; set; }
        public decimal? GeoAltitude { get; set; }
        public string Squawk { get; set; }
        public bool Spi { get; set; }
        public int PositionSource { get; set; }
    }
}