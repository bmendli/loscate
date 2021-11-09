namespace Loscate.DTO.Map
{
    public class Pin 
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string PhotoBase64 { get; set; }
        public string UserUID { get; set; }
    }
}
