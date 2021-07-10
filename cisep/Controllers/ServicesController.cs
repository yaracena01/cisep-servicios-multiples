using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using cisep.Models;
using cisep.ViewModel;

using System.IO;
using cisep.interfaces;
using AutoMapper;
using cisep.ViewModels.ServicesViewModel;
using Microsoft.AspNetCore.Http;

namespace cisep.Controllers
{
    
    public class ServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly CisepDBContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ServicesController(CisepDBContext context, IUnitOfWork unitOfWork, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {          
            var model = _unitOfWork.Services.GetAllServices();     
            var vw = _mapper.Map<List<ServicesViewModel>>(model);
            ViewBag.services = vw;
            return View();
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var services = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (services == null)
            {
                return NotFound();
            }

            return View(services);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,Photo,Url,UrlName,Type_Services")] ServicesViewModelCreate vw, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "imageServices");
                    uniqueFileName = Guid.NewGuid().ToString() + Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    vw.Photo = uniqueFileName;
                }
                vw.Type_Services = 1;
              
                var model = _mapper.Map<Services>(vw);
                _unitOfWork.Services.Insert(model);
                _unitOfWork.Save();

                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var model = _unitOfWork.Services.GetById(id);
            var vw = _mapper.Map<ServicesViewModel>(model);
            return new JsonResult(vw); 
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Photo,Url,UrlName, Type_Services")] ServicesViewModel vw, string ExistingPhotoPath, IFormFile Photo)
        {
            if (id != vw.Id)
            {
                return NotFound();
            }
      
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;
                    if (Photo != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "imageServices");
                        uniqueFileName = Guid.NewGuid().ToString() + Photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                        vw.Photo = uniqueFileName;
                    }
                    else
                    {
                        vw.Photo = ExistingPhotoPath;
                    }
                    vw.Type_Services = 1;
                    var model = _mapper.Map<Services>(vw);
                    _unitOfWork.Services.Update(model);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesExists(vw.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (services == null)
            {
                return NotFound();
            }

            return View(services);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = _unitOfWork.Services.GetById(id);                
            _unitOfWork.Services.Delete(model);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicesExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
