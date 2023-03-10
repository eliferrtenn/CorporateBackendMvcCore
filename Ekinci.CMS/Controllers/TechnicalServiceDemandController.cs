using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Requests.TechnicalServiceDemandRequests;
using Ekinci.CMS.Business.Services;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class TechnicalServiceDemandController : CMSBaseController
    {
        private readonly ITechnicalServiceDemandService technicalServiceDemandService;
        private readonly ITechnicalServiceStaffService technicalServiceStaffService;


        public TechnicalServiceDemandController(ITechnicalServiceStaffService _technicalServiceStaffService, ITechnicalServiceDemandService _technicalServiceDemandService)
        {

            technicalServiceStaffService = _technicalServiceStaffService;
            technicalServiceDemandService = _technicalServiceDemandService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await technicalServiceDemandService.GetAll();
            return View(result.Data);
        }


        public async Task<IActionResult> Details(int id)
        {
            var result = await technicalServiceDemandService.GetTechnicalServiceDemand(id);
            return View(result.Data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result1 = await technicalServiceStaffService.GetAll();
            ViewBag.TechnicalServiceStaffID = new SelectList(result1.Data, "ID", "FullName");
            var result = await technicalServiceDemandService.AssignPersonelTechnicalServiceDemand(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AssignPersonelTechnicalServiceDemandRequest request)
        {
            var result = await technicalServiceDemandService.AssignPersonelTechnicalServiceDemand(request);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }

        public async Task<IActionResult> ListNonAssignment()
        {
            var result = await technicalServiceDemandService.ListNonAssignmentTechnicalServiceDemend();
            return View(result.Data);
        }

        public async Task<IActionResult> ListAssignment()
        {
            var result = await technicalServiceDemandService.ListAssignTechnicalServiceDemand();
            return View(result.Data);
        }

    }
}
