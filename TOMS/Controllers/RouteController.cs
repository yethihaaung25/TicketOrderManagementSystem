using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;

namespace TOMS.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteService _routeService;
        private readonly ICityService _citySercive;
        private readonly IBusLineService _busLineService;
        public RouteController(IRouteService routeService, ICityService cityService, IBusLineService busLineService)
        {
            _routeService = routeService;
            _citySercive = cityService;
            _busLineService = busLineService;
        }
        public IActionResult List()
        {
            IList<RouteViewModel> cvm = _routeService.RetrieveAll().Select(c => new RouteViewModel
            {
                Id = c.Id,
                //FromCityId = c.FromCityId,
                FromCityName = _citySercive.GetByID(c.FromCityId).Name,
                //ToCityId = c.ToCityId,
                ToCityName = _citySercive.GetByID(c.ToCityId).Name,
                UnitPrice = c.UnitPrice,
                Remark = c.Remark,
                //BusLineId = c.BusLineId,
                OwnerName = _busLineService.GetByID(c.BusLineId).Owner,
            }).ToList();
            return View(cvm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Entry()
        {
            ViewBag.FromCityId = _citySercive.RetrieveAll().Select(c => new CityViewModel
            {
                ID = c.Id,
                Name = c.Name,
            }).ToList();

            ViewBag.ToCityId = _citySercive.RetrieveAll().Select(c => new CityViewModel
            {
                ID = c.Id,
                Name = c.Name,
            }).ToList();

            ViewBag.BusLineId = _busLineService.RetrieveAll().Select(c => new BusLineViewModel
            {
                Id = c.Id,
                Owner = c.Owner,
            }).ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(RouteViewModel rvm)
        {
            try
            {
                RouteEntity route = new RouteEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    FromCityId = rvm.FromCityId,
                    ToCityId = rvm.ToCityId,
                    UnitPrice = rvm.UnitPrice,
                    When = rvm.When,
                    Remark = rvm.Remark,
                    BusLineId = rvm.BusLineId,
                    CreatedDate = DateTime.Now,
                };
                _routeService.Create(route);
                TempData["info"] = "Save Succesully the record in the system.";
            }
            catch (Exception e)
            {

                TempData["info"] = "Error occur when save the record to the system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            try
            {
                _routeService.Delete(id);
                TempData["info"] = "Delete Succesully the record in the system.";
            }
            catch (Exception e)
            {

                TempData["info"] = "Error occur when save the record to the system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var routeDataModel = _routeService.GetByID(id);
            RouteViewModel routeViewModel = new RouteViewModel();
            if (routeDataModel != null)
            {
                routeViewModel.Id = routeDataModel.Id;
                routeViewModel.FromCityId = routeDataModel.FromCityId;
                routeViewModel.FromCityName = _citySercive.GetByID(routeDataModel.FromCityId).Name;
                routeViewModel.ToCityId = routeDataModel.ToCityId;
                routeViewModel.ToCityName = _citySercive.GetByID(routeDataModel.ToCityId).Name;
                routeViewModel.UnitPrice = routeDataModel.UnitPrice;
                routeViewModel.When = routeDataModel.When;
                routeViewModel.Remark = routeDataModel.Remark;
                routeViewModel.BusLineId = routeDataModel.BusLineId;
                routeViewModel.OwnerName = _busLineService.GetByID(routeDataModel.BusLineId).Owner;
            }

            ViewBag.FromCityId = _citySercive.RetrieveAll().Where(w => w.Id != routeViewModel.FromCityId).Select(c => new CityViewModel
            {
                ID = c.Id,
                Name = c.Name,
            }).ToList();

            ViewBag.ToCityId = _citySercive.RetrieveAll().Where(w => w.Id != routeViewModel.ToCityId).Select(c => new CityViewModel
            {
                ID = c.Id,
                Name = c.Name,
            }).ToList();

            ViewBag.BusLineId = _busLineService.RetrieveAll().Where(w => w.Id != routeViewModel.BusLineId).Select(c => new BusLineViewModel
            {
                Id = c.Id,
                Owner = c.Owner,
            }).ToList();
            return View(routeViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(RouteViewModel rvm)
        {
            try
            {
                //data exchange from view model to data model
                RouteEntity route = new RouteEntity()
                {
                    Id = rvm.Id,
                    FromCityId = rvm.FromCityId,
                    ToCityId = rvm.ToCityId,
                    When = rvm.When,
                    BusLineId = rvm.BusLineId,
                    UnitPrice = rvm.UnitPrice,
                    Remark = rvm.Remark,
                    UpdatedDate = DateTime.Now
                };
                _routeService.Update(route);
                TempData["info"] = "Update successfully the recrod to the system.";
            }
            catch (Exception e)
            {
                TempData["info"] = "Error when update the recrod to the system.";
            }
            return RedirectToAction("List");
        }
    }
}
