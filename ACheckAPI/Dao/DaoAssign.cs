using ACheckAPI.Common;
using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Dao
{
    public class DaoAssign : DaoBase
    {
        public DaoAssign(TWG_ACHECKContext _context) : base(_context) { }
        

        public void AssignAsset(Assign entity)
        {
            if (string.IsNullOrEmpty(entity.AssignId))
            {
                entity.AssignId = Guid.NewGuid().ToString();
                entity.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                entity.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Assign.Add(entity);
            }
            else
            {
                var entityDB = context.Assign.Where(p => p.AssignId.Equals(entity.AssignId)).FirstOrDefault();
                //Thay đổi phòng ban
                if (entityDB.ReceiverBy != entity.ReceiverBy)
                {
                    //Cập nhật ngày hết hạn thiết bị tại phòng ban
                    entityDB.ToDate = DateTime.Now.ToString("dd-MM-yyyy");
                    context.Assign.Update(entityDB);

                    Assign assignNew = new Assign();
                    assignNew.AssignId = Guid.NewGuid().ToString();
                    assignNew.FromDate = DateTime.Now.ToString("dd-MM-yyyy");
                    assignNew.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    assignNew.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    assignNew.ReceiverBy = entity.ReceiverBy;
                    assignNew.Supporter = entity.Supporter;
                    assignNew.AssetId = entity.AssetId;
                    context.Assign.Add(assignNew);
                }
                else
                {
                    //((Assign)entity).CopyPropertiesTo<Assign>(entityDB);
                    entityDB.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    entityDB.AssetId = entity.AssetId;
                    context.Assign.Update(entityDB);
                }
            }
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
