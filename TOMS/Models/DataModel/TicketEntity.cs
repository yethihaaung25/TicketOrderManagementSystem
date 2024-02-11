using System.ComponentModel.DataAnnotations.Schema;

namespace TOMS.Models.DataModel
{
    [Table("Ticket")]
    public class TicketEntity : BaseEntity
    {
        public DateTime TicketOrderedDate { get; set; }
        public string RouteId { get; set; }
        public string SeatNo { get; set; }
        public string PassengerType { get; set; }
        public string SeatRow { get; set; }
        public string SeatPlace { get; set; }
        public string? Status { get; set; }
        public string? OrderTransactionId { get; set; }
    }
}
