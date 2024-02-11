using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;

namespace TOMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketOrderTransactionController : Controller
    {
        private readonly ITicketOrderTransactionService _ticketOrderTransactionService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly IPassengerService _passengerService;

        public TicketOrderTransactionController(ITicketOrderTransactionService ticketOrderTransactionService, IPaymentTypeService paymentTypeService, IPassengerService passengerService)
        {
            _ticketOrderTransactionService = ticketOrderTransactionService;
            _paymentTypeService = paymentTypeService;
            _passengerService = passengerService;
        }

        public IActionResult List()
        {
            IList<OrderTransactionViewModel> orders = this._ticketOrderTransactionService.RetrieveAll().Select(b => new OrderTransactionViewModel
            {
                TnxNo = b.TnxNo,
                Status = b.Status,
                PaymentMethodId = _paymentTypeService.GetByID(b.PaymentTypeId).AccountName,
                NumberOfTickets = b.NumberOfTickets,
                TotalAmount = b.TotalAmount,
                Remark = b.Remark
                }).ToList();
            
            return View(orders);
        }
    }
}
