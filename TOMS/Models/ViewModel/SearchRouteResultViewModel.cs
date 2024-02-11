namespace TOMS.Models.ViewModel
{
    public class SearchRouteResultViewModel
    {
        public string PassengerType { get; set; }
        public string Type { get; set; }
        public DateTime DepatureDate { get; set; }
        public string Owner { get; set; }
        public int NumberOfSeat { get; set; }
        public string RouteID { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public decimal UnitPrice { get; set; }
        public TimeSpan When { get; set;}
        public string Remark { get; set;}
    }
}
