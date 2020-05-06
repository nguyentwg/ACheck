using ACheckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Dao
{
    public class DaoAssign : DaoBase
    {
        public DaoAssign(TWG_ACHECKContext _context) : base(_context) { }
        public void AssignAsset(Assign assign)
        {
            if(string.IsNullOrEmpty(assign.AssignId))
            {
                assign.AssignId = Guid.NewGuid().ToString();
                assign.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                assign.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Assign.Add(assign);
            }
            else
            {
                assign.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Assign.Update(assign);
            }
            //return context.SaveChanges();
        }

        public bool CheckAssignTime(string AssetID, string FromDate, string ToDate)
        {
            int count = context.Assign.Where(p => p.Active == true && p.AssetId.Equals(AssetID) &&
                (p.ToDate != null && p.FromDate != null && (string.Compare(p.ToDate, FromDate) >= 0 && string.Compare(p.FromDate, ToDate) <= 0))
            ).AsEnumerable().Count();
            if(count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
