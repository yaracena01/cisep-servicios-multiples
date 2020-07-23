using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cisep.Models;
namespace cisep.Interfaces
{
    public interface IFlex_pay
    {
        flex_pay GetById(string code);
        void Delete(flex_pay flex_pay);
    }
}
