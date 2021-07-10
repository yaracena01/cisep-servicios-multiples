using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cisep.interfaces;
using AutoMapper;
using cisep.ViewModels.ServicesViewModel;

namespace cisep.Controllers
{
    public class SecurityCameraController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public SecurityCameraController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var model = _unitOfWork.Services.GetAllCreditServices();
            var vw = _mapper.Map<List<ServicesViewModel>>(model);
            ViewBag.services = vw;
            return View();
        }
    }
}