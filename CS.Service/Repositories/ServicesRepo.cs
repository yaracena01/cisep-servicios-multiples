using cisep.interfaces;
using cisep.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.Repositories
{
    public class ServicesRepo : IServices
    {
        private readonly CisepDBContext _context;

        public ServicesRepo(CisepDBContext context)
        {
            _context = context;
        }
        public void Delete(Services services)
        {
            _context.Services.Remove(services);
        }

        public  List<Services> GetAll()
        {
            return _context.Services.Include(s => s.Services_Details).Where(ts => ts.Type_Services == 1).ToList();
           
        }
        public List<Services> GetAllCreditServices()
        {
            return _context.Services.Include(s => s.Services_Details).Where(ts => ts.Type_Services == 2).ToList();

        }
        public List<Services> GetAllServices()
        {
            return _context.Services.Include(s => s.Services_Details).ToList();

        }

        public Services GetById(int? id)
        {
            return _context.Services.Include(s => s.Services_Details).FirstOrDefault(x => x.Id == id);
          
        }

        public void Insert(Services services)
        {
            _context.Services.Add(services);
        }

        public void Update(Services services)
        {
            _context.Services.Update(services);
        }
    }
}
