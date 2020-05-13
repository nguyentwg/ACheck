using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using shortid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace ACheckAPI.Dao
{
    public class DaoEAVAttribute:DaoBase
    {
        public DaoEAVAttribute(TWG_ACHECKContext _context) : base(_context) { }
        
        public List<EavAttribute> GetAll()
        {
            return context.EavAttribute.Where(p => p.Active == true).AsEnumerable().ToList();
        }

        public List<EavAttribute> GetAttributeByGroup(string AttributeGroup)
        {
            return context.EavAttribute.Where(p => p.Active == true && p.AttributeGroup.Equals(AttributeGroup)).AsEnumerable().ToList();
        }

        public async Task<int> AddEavAttribute(EavAttribute entity)
        {
            entity.Guid = this.GenerateEavAttributeID();
            context.EavAttribute.Add(entity);
            return await context.SaveChangesAsync();
        }



        public async Task<int> UpdateEavAttribute(EavAttribute eavAttribute)
        {
            var entity = context.EavAttribute.Where(p => p.Guid.Equals(eavAttribute.Guid)).FirstOrDefault();
            eavAttribute.CopyPropertiesTo<EavAttribute>(entity);
            context.EavAttribute.Update(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteEavAttribute(string id)
        {
            var entity = context.EavAttribute.Where(p => p.Guid.Equals(id)).FirstOrDefault();
            entity.Active = false;
            context.EavAttribute.Update(entity);
            return await context.SaveChangesAsync();
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
