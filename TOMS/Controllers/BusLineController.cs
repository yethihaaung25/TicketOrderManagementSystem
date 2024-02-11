using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using TOMS.Helper;
using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;

namespace TOMS.Controllers
{
    public class BusLineController : Controller
    {
        private readonly IBusLineService _busLineService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BusLineController(IBusLineService busLineService, IWebHostEnvironment webHostEnvironment)
        {
            _busLineService = busLineService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult List()
        {
            IList<BusLineViewModel> blvm = _busLineService.RetrieveAll().Select(b => new BusLineViewModel
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
            return View(blvm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Entry()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(BusLineViewModel blvm)
        {
            try
            {
                BusLineEntity busline = new BusLineEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    No = blvm.No,
                    Owner = blvm.Owner,
                    Driver1 = blvm.Driver1,
                    PhoneOfDriver1 = blvm.PhoneOfDriver1,
                    Helper1 = blvm.Helper1,
                    PhoneOfHelper1 = blvm.PhoneOfHelper1,
                    Driver2 = blvm.Driver2,
                    PhoneOfDriver2 = blvm.PhoneOfDriver2,
                    Helper2 = blvm.Helper2,
                    PhoneOfHelper2 = blvm.PhoneOfHelper2,
                    NumberOfSeat = blvm.NumberOfSeat,
                    Type = blvm.Type,
                    CreatedDate = DateTime.Now,
                };
                _busLineService.Create(busline);
                TempData["info"] = "Save Succesully the record in the system.";
            }
            catch (Exception e)
            {

                TempData["info"] = "Error occur when save the record to the system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string ID)
        {
            try
            {
                _busLineService.Delete(ID);
                TempData["info"] = "Delete Succesully the record in the system.";
            }
            catch (Exception e)
            {

                TempData["info"] = "Error occur when save the record to the system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string ID)
        {
            var buslineDataModel = _busLineService.GetByID(ID);
            BusLineViewModel busLineViewModel = new BusLineViewModel();
            if (buslineDataModel != null)
            {
                busLineViewModel.Id = buslineDataModel.Id;
                busLineViewModel.No = buslineDataModel.No;
                busLineViewModel.Owner = buslineDataModel.Owner;
                busLineViewModel.Driver1 = buslineDataModel.Driver1;
                busLineViewModel.PhoneOfDriver1 = buslineDataModel.PhoneOfDriver1;
                busLineViewModel.Helper1 = buslineDataModel.Helper1;
                busLineViewModel.PhoneOfHelper1 = buslineDataModel.PhoneOfHelper1;
                busLineViewModel.Driver2 = buslineDataModel.Driver2;
                busLineViewModel.PhoneOfDriver2 = buslineDataModel.PhoneOfDriver2;
                busLineViewModel.Helper2 = buslineDataModel.Helper2;
                busLineViewModel.PhoneOfHelper2 = buslineDataModel.PhoneOfHelper2;
                busLineViewModel.NumberOfSeat = buslineDataModel.NumberOfSeat;
                busLineViewModel.Type = buslineDataModel.Type;
            }
            return View(busLineViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(BusLineViewModel blvm)
        {
            try
            {
                //data exchange from view model to data model
                BusLineEntity bus = new BusLineEntity()
                {
                    Id = blvm.Id,
                    No = blvm.No,
                    Owner = blvm.Owner,
                    Driver1 = blvm.Driver1,
                    PhoneOfDriver1 = blvm.PhoneOfDriver1,
                    Helper1 = blvm.Helper1,
                    PhoneOfHelper1 = blvm.PhoneOfHelper1,
                    Driver2 = blvm.Driver2,
                    PhoneOfDriver2 = blvm.PhoneOfDriver2,
                    Helper2 = blvm.Helper2,
                    PhoneOfHelper2 = blvm.PhoneOfHelper2,
                    NumberOfSeat = blvm.NumberOfSeat,
                    Type = blvm.Type,
                    UpdatedDate = DateTime.Now
                };
                _busLineService.Update(bus);
                TempData["info"] = "Update successfully the recrod to the system.";
            }
            catch (Exception e)
            {
                TempData["info"] = "Error when update the recrod to the system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult BusLineDetailReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BusLineDetailReport(string No, string Type, string Owner)
        {
            IList<BusLineReportModel> busLineReportModels = _busLineService.RetrieveAll().Where(w => w.No == No || w.Type == Type || w.Owner == Owner).Select(s => new BusLineReportModel
            {
                No = s.No,
                Type = s.Type,
                Owner = s.Owner,
                Driver1 = s.Driver1,
                PhoneOfDriver1 = s.PhoneOfDriver1,
                Helper1 = s.Helper1,
                NumberOfSeat = s.NumberOfSeat,
            }).ToList();
            if(busLineReportModels.Count > 0 )
            {
                var rdlcPath = Path.Combine(_webHostEnvironment.WebRootPath, "ReportFiles", "BusLineDetailReport.rdlc");
                var filestring = new FileStream(rdlcPath, FileMode.Open);
                Stream reportDefinition = filestring;
                LocalReport localReport = new LocalReport();
                localReport.LoadReportDefinition(reportDefinition);
                localReport.DataSources.Add(new ReportDataSource("BusLineDataSet", busLineReportModels));
                byte[] pdfFile = localReport.Render("pdf");
                filestring.Close();
                return File(pdfFile, "application/pdf");
            }
            else
            {
                TempData["info"] = "There is no data to export pdf.";
                return View();
            }
        }
    }
}
