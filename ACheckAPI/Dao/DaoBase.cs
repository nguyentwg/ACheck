using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Models;

namespace ACheckAPI.Dao
{
    public class DaoBase
    {
        protected readonly TWG_ACHECKContext context;
        public DaoBase(TWG_ACHECKContext _context)
        {
            context = _context;
        }
    }
}
