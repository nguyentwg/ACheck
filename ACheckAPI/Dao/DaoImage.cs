using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Models;

namespace ACheckAPI.Dao
{
    public class DaoImage:DaoBase
    {
        public DaoImage(TWG_ACHECKContext _context) : base(_context) { }

        public async Task<int> DeleteImage(string ImageID)
        {
            var img = context.Image.Where(p => p.Guid.Equals(ImageID)).AsEnumerable().FirstOrDefault();
            if (img != null)
            {
                img.Active = false;
                context.Image.Update(img);
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
            
        }
    }
}
