using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisep.Models;

namespace cisep.interfaces
{
    public interface IServices
    {
        List<Services> GetAll();
        List<Services> GetAllCreditServices();
        List<Services> GetAllServices();
        Services GetById(int? id);
        void Insert(Services services);
        void Update(Services services);
        void Delete(Services services);
    }
}
