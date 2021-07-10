using cisep.Interfaces;
using cisep.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.Repositories
{
    public class Flex_payRepo : IFlex_pay
    {
        private readonly CisepDBContext _context;

        public Flex_payRepo(CisepDBContext context)
        {
            _context = context;
        }
        public void Delete(flex_pay flex_pay)
        {
            _context.Flex_pay.Remove(flex_pay);
        }

        public flex_pay GetById(string code)
        {
            return _context.Flex_pay.FirstOrDefault(x => x.Code ==code);
        }
    }
}
