using System.ComponentModel;

namespace TOMS.Services.Utilities
{
    public class ConstantsModel
    {
        public const string BusLineTypeVip = "VIP";
        public const string BusLineTypeStandard = "Standard";

        public const string PassengerTypeLocal = "Local";
        public const string PassengerTypeForeign = "Foreign";

        public enum TicketOrderTransaction
        {
            [Description("Unpaid")]
            Unpaid,
            [Description("Paid")]
            Paid,
        }

        public enum TicketSeatStatus
        {
            [Description("Reserved")]
            Reserved,
            [Description("Confirmed")]
            Confirmed,
            [Description("Pending")]
            Pending,
            [Description("Cancaled")]
            Cancaled
        }
    }
}
