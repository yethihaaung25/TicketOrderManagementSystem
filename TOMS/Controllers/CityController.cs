using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using TOMS.Helper;
using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;

namespace TOMS.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityService _cityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CityController(ICityService cityService, IWebHostEnvironment webHostEnvironment)
        {
            _cityService = cityService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult List()
        {
            IList<CityViewModel> cvm = _cityService.RetrieveAll().Select(c => new CityViewModel
            {
                ID = c.Id,
                Code = c.Code,
                Name = c.Name,
                ZipCode = c.ZipCode
            }).ToList(); 
            return View(cvm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Entry()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(CityViewModel cvm)
        {
            try
            {
                CityEntity cityEntity = new CityEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = cvm.Code,
                    Name = cvm.Name,
                    ZipCode = cvm.ZipCode,
                    CreatedDate = DateTime.Now,
                };
                _cityService.Create(cityEntity);
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
                _cityService.Delete(ID);
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
            var cityDataModel = _cityService.GetByID(ID);
            CityViewModel cityViewModel = new CityViewModel();
            if (cityDataModel != null)
            {
                cityViewModel.ID = cityDataModel.Id;
                cityViewModel.Name = cityDataModel.Name;
                cityViewModel.Code = cityDataModel.Code;
                cityViewModel.ZipCode = cityDataModel.ZipCode;
            }
            return View(cityViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(CityViewModel cityViewModel)
        {
            try
            {
                //data exchange from view model to data model
                CityEntity city = new CityEntity()
                {
                    Id = cityViewModel.ID,
                    Code = cityViewModel.Code,
                    Name = cityViewModel.Name,
                    ZipCode = cityViewModel.ZipCode,
                    UpdatedDate = DateTime.Now
                };
                _cityService.Update(city);
                TempData["info"] = "Update successfully the recrod to the system.";
            }
            catch (Exception e)
            {
                TempData["info"] = "Error when update the recrod to the system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CityDetailReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CityDetailReport(string code, string name, string zipcode)
        {
            IList<CityReportModel> cityReportModels = _cityService.RetrieveAll().Where(c => c.Code == code || c.Name == name || c.ZipCode == zipcode).Select(i => new CityReportModel
            {
                Name = i.Name,
                Code = i.Code,
                ZipCode = i.ZipCode,
            }).ToList();
            if(cityReportModels.Count > 0)
            {
                var rdlcpath = Path.Combine(_webHostEnvironment.WebRootPath,"ReportFiles","CityDetailReport.rdlc");
                var filestring = new FileStream(rdlcpath, FileMode.Open);
                Stream reportDefinition = filestring;
                LocalReport localReport = new LocalReport();
                localReport.LoadReportDefinition(reportDefinition);
                localReport.DataSources.Add(new ReportDataSource("CityDetailReport",cityReportModels));
                byte[] pdffile = localReport.Render("pdf");
                filestring.Close();
                return File(pdffile,"application/pdf");
            }
            else
            {
                TempData["info"] = "There is no data to export pdf.";
                return View();
            }
        }
    }
}
