namespace TOMS.Models.ViewModel
{
    public class TicketViewModel
    {
        public DateTime TicketOrderDate { get; set; }// Depature Date
        public string PassengerType { get; set; }
        public string BusType { get; set; }
        public string RouteId { get; set; }
        public string[] SeatNo { get; set; }
        public string[] SeatRow { get; set; }
        public TimeSpan When { get; set; }
        public string? Status { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount {  get; set; }
    }
}
