using Microsoft.AspNetCore.Mvc;
using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;
using TOMS.Services.Utilities;

namespace TOMS.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly IPassengerService _passengerService;
        private readonly ITicketOrderServices _ticketOrderServices;
        private readonly ITicketOrderTransactionService _ticketOrderTransactionService;

        public PaymentController(IPaymentTypeService paymentTypeService, IPassengerService passengerService, ITicketOrderServices ticketOrderServices, ITicketOrderTransactionService ticketOrderTransactionService)
        {
            _paymentTypeService = paymentTypeService;
            _passengerService = passengerService;
            _ticketOrderServices = ticketOrderServices;
            _ticketOrderTransactionService = ticketOrderTransactionService;
        }

        [HttpPost]
        public IActionResult PaymentConfirm(PaymentConfirmViewModel payment)
        {
            TicketViewModel ticket = SessionHelper.GetDataFromSession<TicketViewModel>(HttpContext.Session, "ticketInfos");
            var paymentType = _paymentTypeService.GetByID(payment.PaymentTypeId);
            PaymentTypeViewModel paymentTypeViewModel = new PaymentTypeViewModel()
            {
                AccountName = paymentType.AccountName,
                AccountNumber = paymentType.AccountNumber,
                Type = paymentType.Type,
            };
            /* payment.RouteId = ticket.RouteId;
            payment.SeatNo = ticket.SeatNo;
            payment.SeatRow = ticket.SeatRow;
            payment.NumberOfTickets = ticket.NumberOfTickets;  
            payment.UnitPrice = ticket.UnitPrice;
            payment.TicketOrderDate = ticket.TicketOrderDate;
            payment.TotalAmount = ticket.TotalAmount;
            payment.PassengerType = ticket.PassengerType;*/

            payment.PaymentTypeViewModel = paymentTypeViewModel;
            payment.Ticket = ticket;
            return View("MakePayment",payment);
        }

        public IActionResult MakePayment() => View();

        [HttpPost]
        public IActionResult MakePayment(PaymentConfirmViewModel paymentConfirmViewModel)
        {
            TicketViewModel ticket = SessionHelper.GetDataFromSession<TicketViewModel>(HttpContext.Session, "ticketInfos");
            // collect the passenger info
            var passenger = new PassengerEntity()
            {
                Id = Guid.NewGuid().ToString(),
                Name = paymentConfirmViewModel.Name,
                NRC = paymentConfirmViewModel.NRC,
                Email = paymentConfirmViewModel.Email,
                Phone = paymentConfirmViewModel.Phone,
                Address = paymentConfirmViewModel.Address,
                Gender = paymentConfirmViewModel.Gender
            };

            // collect the ticket order transaction info
            var ticketOrderTransaction = new TicketOrderTransactionEntity()
            {
                Id = Guid.NewGuid().ToString(),
                TnxNo = DateTime.Now.ToString("yyyyMMddHHmmss"),
                Status = "Unpaid",
                PaymentTypeId = paymentConfirmViewModel.PaymentTypeId,
                PassengerId = passenger.Id,
                NumberOfTickets = ticket.NumberOfTickets,
                TotalAmount = ticket.TotalAmount,
                Remark = "Nothing",
                ScreenShootUrl = "/Images/PaymentReceived/a.png"
            };

            // collect the ticket info
            var ticketOrder = new List<TicketEntity>();
            for(int i = 0; i < ticket.NumberOfTickets; i++) 
            {
                var t = new TicketEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    RouteId = ticket.RouteId,
                    TicketOrderedDate = ticket.TicketOrderDate,
                    PassengerType = ticket.PassengerType,
                    SeatNo = ticket.SeatNo[i],
                    SeatRow = ticket.SeatRow[i],
                    SeatPlace = ticket.SeatNo[i].Contains("1") ? "Window Side" : "Lane Side",
                    Status = "Reserved",
                    OrderTransactionId = ticketOrderTransaction.Id,
                };
                ticketOrder.Add(t);
            }

            _passengerService.Create(passenger);
            _ticketOrderTransactionService.Create(ticketOrderTransaction);
            _ticketOrderServices.Create(ticketOrder);

            // passing the txno, Number of Seats,Seat(s) No to the ui
            paymentConfirmViewModel.TxNo = ticketOrderTransaction.TnxNo;
            paymentConfirmViewModel.Ticket = ticket;

            // clear the ticket info session
            SessionHelper.ClearSession(HttpContext.Session);
            return View("TicketVouchorHistoryList", paymentConfirmViewModel);
        }

        public IActionResult TicketVouchorHistoryList()
        {
            return View();
        }
    }
}
