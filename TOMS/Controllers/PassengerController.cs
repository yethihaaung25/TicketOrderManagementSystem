using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System.Security.Cryptography.X509Certificates;
using TOMS.Helper;
using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;

namespace TOMS.Controllers
{
	public class PassengerController : Controller
	{
		private readonly IPassengerService _passengerService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public PassengerController(IPassengerService passengerService, IWebHostEnvironment webHostEnvironment)
		{
			_passengerService = passengerService;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult List()
		{
			List<PassengerViewModel> pvm = _passengerService.RetrieveAll().Select(m => new PassengerViewModel
			{
				Id = m.Id,
				Name = m.Name,
				NRC = m.NRC,
				Email = m.Email,
				Phone = m.Phone,
				Gender = m.Gender,
				Address = m.Address,
				DOB = m.DOB,
			}).ToList();
			return View(pvm);
		}

        [Authorize(Roles = "Admin")]
        public IActionResult Entry()
		{
			return View();
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		public IActionResult Entry(PassengerViewModel pvm)
		{
			try
			{
				PassengerEntity passenger = new PassengerEntity()
				{
					Id = Guid.NewGuid().ToString(),
					Name = pvm.Name,
					NRC = pvm.NRC,
					DOB = pvm.DOB,
					Email = pvm.Email,
					Phone = pvm.Phone,
					Gender = pvm.Gender,
					Address = pvm.Address,
					CreatedDate = DateTime.Now,
				};
				_passengerService.Create(passenger);
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
				_passengerService.Delete(ID);
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
			var passengerDataModel = _passengerService.GetByID(ID);
			PassengerViewModel passengerViewModel = new PassengerViewModel();
            if (passengerDataModel != null)
            {
                passengerViewModel.Id = passengerDataModel.Id;
                passengerViewModel.Name = passengerDataModel.Name;
                passengerViewModel.Address = passengerDataModel.Address;
                passengerViewModel.DOB = passengerDataModel.DOB;
                passengerViewModel.Phone = passengerDataModel.Phone;
                passengerViewModel.Gender = passengerDataModel.Gender;
                passengerViewModel.Email = passengerDataModel.Email;
                passengerViewModel.NRC = passengerDataModel.NRC;
            }
            return View(passengerViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(PassengerViewModel pvm)
        {
            try
            {
                //data exchange from view model to data model
                PassengerEntity passenger = new PassengerEntity()
                {
                    Id = pvm.Id,
                    Name = pvm.Name,
                    NRC = pvm.NRC,
                    Email = pvm.Email,
                    Phone = pvm.Phone,
                    Address = pvm.Address,
                    Gender = pvm.Gender,
                    DOB = pvm.DOB,
                    UpdatedDate = DateTime.Now
                };
                _passengerService.Update(passenger);
                TempData["info"] = "Update successfully the recrod to the system.";
            }
            catch (Exception e)
            {
                TempData["info"] = "Error when update the recrod to the system.";
            }
            return RedirectToAction("List");
        }

		public IActionResult PassengerDetailReport()
		{
			return View();
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		public IActionResult PassengerDetailReport(string name, string nrc, string email)
		{
			IList<PassengerReportModel> passengerReportModels = _passengerService.RetrieveAll().Where(p => p.Name == name || p.NRC == nrc || p.Email == email).Select(i => new PassengerReportModel
			{
				Name = i.Name,
				NRC = i.NRC,
				Email = i.Email,
				Phone = i.Phone,
				Address = i.Address,
				Gender = i.Gender,
				DOB = i.DOB,
			}).ToList();

			if(passengerReportModels.Count > 0)
			{
				var rdlcpath = Path.Combine(_webHostEnvironment.WebRootPath,"ReportFiles","PassengerDetailReport.rdlc");
				var filestring = new FileStream(rdlcpath,FileMode.Open);
				Stream reportDefinition = filestring;
				LocalReport localReport = new LocalReport();
				localReport.LoadReportDefinition(reportDefinition);
				localReport.DataSources.Add(new ReportDataSource("PassengerDataSet", passengerReportModels));
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
