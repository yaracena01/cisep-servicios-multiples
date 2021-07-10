using cisep.interfaces;
using cisep.Interfaces;
using cisep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.Repositories
{
    public class UnitOfWorkRepo : IUnitOfWork
    {
        private readonly CisepDBContext _context;
        private IServices _IservicesRepo;
        private IClients _IclientsRepo;
        private IFlex_pay _Iflex_pay;
        public UnitOfWorkRepo(CisepDBContext context)
        {
            _context = context;
        }
        public IServices Services
        {
            get
            {
                return _IservicesRepo = _IservicesRepo ?? new ServicesRepo(_context);
            }
            set { }
        }

        public IClients Clients {
            get
            {
                return _IclientsRepo = _IclientsRepo ?? new ClientsRepo(_context);
            }
            set { }
        }

        public IFlex_pay Flex_Pay {
            get
            {
                return _Iflex_pay = _Iflex_pay ?? new Flex_payRepo(_context);
            }
            set { }
        }

        public void Save()
        {
           _context.SaveChanges();
        }
    }
}
