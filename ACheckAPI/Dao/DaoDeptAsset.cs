using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Common;
using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;

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
                //Thay đổi phòng ban
                if (entityDB.DeptId != entity.DeptId)
                {
                    //Cập nhật ngày hết hạn thiết bị tại phòng ban
                    entityDB.ToDate = DateTime.Now.ToString("dd-MM-yyyy");
                    context.DeptAsset.Update(entityDB);

                    DeptAsset deptAsset = new DeptAsset();
                    deptAsset.Guid = Guid.NewGuid().ToString();
                    deptAsset.FromDate = DateTime.Now.ToString("dd-MM-yyyy");
                    deptAsset.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    deptAsset.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    deptAsset.Supporter = entity.Supporter;
                    deptAsset.DeptId = entity.DeptId;
                    deptAsset.AssetId = entity.AssetId;
                    context.DeptAsset.Add(deptAsset);
                }
                else
                {
                    //entity.CopyPropertiesTo<DeptAsset>(entityDB);
                    //entityDB.AssetId = entity.AssetId;
                    entityDB.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    context.DeptAsset.Update(entityDB);
                }
            }
        }


    }
}
