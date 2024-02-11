namespace TOMS.Models.ViewModel
{
    public class SearchRouteViewModel
    {
        public string? BusType { get; set; }
        public string? PassengerType { get; set; }
        public string RouteID { get; set; }
        public string FromCityId { get; set; }
        public string FromCityName { get; set; }
        public string ToCityId { get; set;}
        public string ToCityName { get; set;}
        public DateTime DepatureDate { get; set; }
    }
}
