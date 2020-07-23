using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using cisep.interfaces;
using cisep.ViewModels.ServicesViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cisep.Controllers
{
    public class ServicesPageController : Controller
    {
        // GET: ServicesPage
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ServicesPageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreditRepairIndex()
        {
            var model = _unitOfWork.Services.GetAllCreditServices();
            var vw = _mapper.Map<List<ServicesViewModel>>(model);
            ViewBag.services = vw;
            return View();
        }

        public ActionResult PushServices()
        {
            return View();
        }

        // GET: ServicesPage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ServicesPage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServicesPage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ServicesPage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServicesPage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ServicesPage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServicesPage/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}