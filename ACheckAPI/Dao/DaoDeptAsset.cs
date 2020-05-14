using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Models;

namespace ACheckAPI.Dao
{
    public class DaoDeptAsset:DaoBase
    {
        public DaoDeptAsset(TWG_ACHECKContext _context) :base(_context) { }

        public void Add(DeptAsset entity)
        {
            if (string.IsNullOrEmpty(entity.Guid))
            {
                entity.Guid = Guid.NewGuid().ToString();
                entity.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                entity.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.DeptAsset.Add(entity);
            }
            else
            {
                var entityDB = context.DeptAsset.Where(p => p.Guid.Equals(entity.Guid)).FirstOrDefault();
                entity.CopyPropertiesTo<DeptAsset>(entityDB);
                context.DeptAsset.Update(entityDB);
            }
        }
    }
}
