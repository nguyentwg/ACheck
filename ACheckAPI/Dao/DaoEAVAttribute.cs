using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using shortid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Dao
{
    public class DaoEAVAttribute:DaoBase
    {
        public DaoEAVAttribute(TWG_ACHECKContext _context) : base(_context) { }
        
        public List<EavAttribute> GetAll()
        {
            return context.EavAttribute.Where(p => p.Active == true).AsEnumerable().ToList();
        }

        public int AddEavAttribute(EavAttribute entity)
        {
            entity.Guid = this.GenerateEavAttributeID();
            context.EavAttribute.Add(entity);
            return context.SaveChanges();
        }

        public int UpdateEavAttribute(EavAttribute eavAttribute)
        {
            var entity = context.EavAttribute.AsNoTracking().Where(p => p.Guid.Equals(eavAttribute.Guid)).FirstOrDefault();
            context.EavAttribute.Update(eavAttribute);
            return context.SaveChanges();
        }

        public int DeleteEavAttribute(string id)
        {
            var entity = context.EavAttribute.AsNoTracking().Where(p => p.Guid.Equals(id)).FirstOrDefault();
            entity.Active = false;
            context.EavAttribute.Update(entity);
            return context.SaveChanges();
        }


        public string GenerateEavAttributeID()
        {
            string today = DateTime.Now.ToString("yyMM");
            string id = ShortId.Generate(true, false, 10).ToUpper();
            string result;
            result = today + "/" + id;
            return result;
        }
    }
}
