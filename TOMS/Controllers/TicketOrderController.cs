using Microsoft.AspNetCore.Mvc;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;
using TOMS.Services.Utilities;

namespace TOMS.Controllers
{
    public class TicketOrderController : Controller
    {
        private readonly ICityService _cityService;
        private readonly IRouteService _routeService;
        private readonly IBusLineService _busLineService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly ITicketOrderServices _ticketOrderServices;
        private readonly ITicketOrderTransactionService _ticketOrderTransactionService;
        public TicketOrderController(ICityService cityService,IRouteService routeService, IBusLineService busLineService , IPaymentTypeService paymentTypeService,ITicketOrderServices ticketOrderServices, ITicketOrderTransactionService ticketOrderTransactionService) 
        {
            _cityService = cityService;
            _routeService = routeService;
            _busLineService = busLineService;
            _paymentTypeService = paymentTypeService;
            _ticketOrderServices = ticketOrderServices;
            _ticketOrderTransactionService = ticketOrderTransactionService;
        }
        public IActionResult SearchRoute()
        {
            ViewBag.BusType = new List<string>() { ConstantsModel.BusLineTypeVip, ConstantsModel.BusLineTypeStandard };
            ViewBag.PassengerType = new List<string>() { ConstantsModel.PassengerTypeLocal, ConstantsModel.PassengerTypeForeign };
            List<SearchRouteViewModel> routeViewModel = _cityService.RetrieveAll().Select(s => new SearchRouteViewModel
            {
                FromCityId = s.Id,
                FromCityName = _cityService.GetByID(s.Id).Name,
                ToCityId = s.Id,
                ToCityName = _cityService.GetByID(s.Id).Name,
            }).ToList() ; 
            return View(routeViewModel);
        }

        [HttpGet]
        public IActionResult SearchRouteBy(SearchRouteViewModel searchRouteViewModel)
        {
            List<RouteViewModel> routeViewModels = _routeService.RetrieveAll().Where(w => w.FromCityId == searchRouteViewModel.FromCityId && w.ToCityId == searchRouteViewModel.ToCityId).Select(s => new RouteViewModel
            {
                Id = s.Id,
                FromCityId = s.FromCityId,
                FromCityName = _cityService.GetByID(s.FromCityId).Name,
                ToCityId = s.ToCityId,
                ToCityName = _cityService.GetByID(s.ToCityId).Name,
                UnitPrice = s.UnitPrice,
                When = s.When,
                BusLineId = s.BusLineId,
                OwnerName = _busLineService.GetByID(s.BusLineId).Owner,
                Remark = s.Remark,
            }).ToList();

            List<BusLineViewModel> busLineViewModels = _busLineService.RetrieveAll().Where(s => s.Type == searchRouteViewModel.BusType).Select(b => new BusLineViewModel
            {
                Id = b.Id,
                No = b.No,
                Owner = b.Owner,
                Driver1 = b.Driver1,
                PhoneOfDriver1 = b.PhoneOfDriver1,
                Helper1 = b.Helper1,
                PhoneOfHelper1 = b.PhoneOfHelper1,
                Driver2 = b.Driver2,
                PhoneOfDriver2 = b.PhoneOfDriver2,
                Helper2 = b.Helper2,
                PhoneOfHelper2 = b.PhoneOfHelper2,
                NumberOfSeat = b.NumberOfSeat,
                Type = b.Type,
            }).ToList();

            List<SearchRouteResultViewModel> searchRouteResultViewModels =
                (
                    from b in busLineViewModels join r in routeViewModels on b.Id equals r.BusLineId select new SearchRouteResultViewModel
                    {
                        RouteID = r.Id,
                        Type = b.Type,
                        Owner = b.Owner,
                        NumberOfSeat = b.NumberOfSeat,
                        FromCity = r.FromCityName,
                        ToCity = r.ToCityName,
                        UnitPrice = r.UnitPrice,
                        When = r.When,
                        Remark = r.Remark,
                        PassengerType = searchRouteViewModel.PassengerType,
                        DepatureDate = searchRouteViewModel.DepatureDate,
                    }).ToList();
            return View("SearchRouteResult", searchRouteResultViewModels);
        }

        public IActionResult SearchRouteResult() => View();

        [HttpGet]
        public IActionResult SelectRouteByPassenger(string routeid, string passengerType,DateTime depatureDate)
        {
            var defaultSeats = SeatPlanHelper.GetSeatPlans();
            var saledSeats = _ticketOrderServices.ReteriveByTicketOrderedDateAndRouteId(depatureDate,routeid);
            for(int i = 0; i < defaultSeats.Count; i++)
            {
                foreach(var s in saledSeats)
                {
                    if(s.SeatNo.Trim().ToLower() == defaultSeats[i].SeatNo.ToLower())
                    {
                        defaultSeats[i].Status = "red";
                    }                    
                }
            }
            var route = _routeService.GetByID(routeid);
            var busline = _busLineService.GetByID(route.BusLineId);

            SeatPlanViewModel seatplan = new SeatPlanViewModel()
            {
                PassengerType = passengerType,
                BusLineType = busline.Type,
                FromCity = _cityService.GetByID(route.FromCityId).Name,
                ToCity = _cityService.GetByID(route.ToCityId).Name,
                When = route.When,
                DepaturedDate = depatureDate,
                UnitPrice = route.UnitPrice,
                RouteId = routeid,
                NumberOfSeat = busline.NumberOfSeat,
                Seats = defaultSeats,
            };
            return View(seatplan);
        }

        [HttpPost]
        public JsonResult Checkout(TicketViewModel ticket)
        {
            // input data to session
            SessionHelper.SetDataToSession(HttpContext.Session,"ticketInfos",ticket);
            return Json(ticket);
        }

        [HttpGet]
        public IActionResult Confirm()
        {
            TicketViewModel ticket = SessionHelper.GetDataFromSession<TicketViewModel>(HttpContext.Session,"ticketInfos");
            ViewBag.PaymentTypeInfo = _paymentTypeService.RetrieveAll().Select(s => new PaymentTypeViewModel
            {
                Id = s.Id,
                Type = s.Type,
                AccountName = s.AccountName,
                AccountNumber = s.AccountNumber,
            }).ToList();
            return View(ticket);
        }
    }
}
