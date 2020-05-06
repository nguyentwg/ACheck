using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ACheckAPI.Dao
{
    public class DaoBuilding : DaoBase
    {
        public DaoBuilding(TWG_ACHECKContext _context) : base(_context) { }

    }
}
