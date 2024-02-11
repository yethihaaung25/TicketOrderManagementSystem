namespace TOMS.Models.ViewModel
{
    public class SeatPlan
    {
        public string SeatNo { get; set; }
        public string Status { get; set; }
        public SeatPlan(string seatNo)
        {
            this.SeatNo = seatNo;
        }

        public SeatPlan(string seatNo, string status)
        {
            this.SeatNo=seatNo;
            this.Status = status;
        }
    }
}
