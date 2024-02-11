namespace TOMS.Models.ViewModel
{
    public class PaymentConfirmViewModel
    {
        public string PaymentTypeId { get; set; }
        public PaymentTypeViewModel PaymentTypeViewModel { get; set; }
        public string Name { get; set; }
        public string NRC { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }

        // For Ticket Info
        public TicketViewModel Ticket { get; set; }
        
        // for showing TxNo to the UI
        public string TxNo { get; set; }
    }
}
