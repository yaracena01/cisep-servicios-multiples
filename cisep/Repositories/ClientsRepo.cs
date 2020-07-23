using cisep.Interfaces;
using cisep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.Repositories
{
    public class ClientsRepo : IClients
    {
        private readonly cisepDBContext _context;
        public ClientsRepo(cisepDBContext context)
        {
            _context = context;
        }
        public void Delete(Clients clients)
        {
            throw new NotImplementedException();
        }

        public List<Clients> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Insert(Clients clients)
        {
            _context.Clients.Add(clients);
        }

        public void Update(Clients clients)
        {
            throw new NotImplementedException();
        }
    }
}
