using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;
using TOMS.Services.Domains;

namespace TOMS.Controllers
{
    public class PaymentTypeController : Controller
    {
        private readonly IPaymentTypeService _paymentTypeService;
        public PaymentTypeController(IPaymentTypeService paymentTypeService)
        {
            _paymentTypeService = paymentTypeService;
        }

        public IActionResult List()
        {
            IList<PaymentTypeViewModel> ptvm = _paymentTypeService.RetrieveAll().Select(p => new PaymentTypeViewModel 
            {
                Id = p.Id,
                Type = p.Type,
                AccountName = p.AccountName,
                AccountNumber = p.AccountNumber,
            }).ToList();
            return View(ptvm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Entry()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(PaymentTypeViewModel ptvm) 
        {
            try
            {
                PaymentTypeEntity payment = new PaymentTypeEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = ptvm.Type,
                    AccountName = ptvm.AccountName,
                    AccountNumber = ptvm.AccountNumber,
                    CreatedDate = DateTime.Now,
                };
                _paymentTypeService.Create(payment);
                TempData["Info"] = "Successfully save in this system.";
            }
            catch (Exception ex) 
            {
                TempData["Info"] = "Error occur when save the data in this system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            try
            {
                _paymentTypeService.Delete(id);
                TempData["Info"] = "Successfully delete in this system.";
            }
            catch (Exception ex)
            {
                TempData["Info"] = "Error occur when delete the data in this system.";
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var payment = _paymentTypeService.GetByID(id);
            PaymentTypeViewModel ptvm = new PaymentTypeViewModel();
            if (payment != null)
            {
                ptvm.Id = payment.Id;
                ptvm.Type = payment.Type;
                ptvm.AccountName = payment.AccountName;
                ptvm.AccountNumber = payment.AccountNumber;
            }
            return View(ptvm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(PaymentTypeViewModel ptvm)
        {
            try
            {
                PaymentTypeEntity paymentDataModel = new PaymentTypeEntity()
                {
                    Id = ptvm.Id,
                    Type = ptvm.Type,
                    AccountName = ptvm.AccountName,
                    AccountNumber = ptvm.AccountNumber,
                    UpdatedDate = DateTime.Now,
                };

                _paymentTypeService.Update(paymentDataModel);
                TempData["Info"] = "Successfully update in this system.";
            }
            catch (Exception ex)
            {
                TempData["Info"] = "Error occur when update the data in this system.";
            }
            return RedirectToAction("List");
        }
    }
}
