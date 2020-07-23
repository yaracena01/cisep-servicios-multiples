using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisep.Models;

namespace cisep.Interfaces
{
    public interface IClients
    {

        List<Clients> GetAll();
        void Insert(Clients clients);
        void Update(Clients clients);
        void Delete(Clients clients);
    }
}
