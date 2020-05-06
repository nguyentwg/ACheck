using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using shortid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Dao
{
    public class DaoAsset :DaoBase
    {
        public DaoAsset(TWG_ACHECKContext _context) : base(_context) { }
        public List<Asset> GetAll()
        {
            var result = context.Asset.AsNoTracking().Where(p=>p.Active == true).Include(p => p.Floor).Include(p=>p.Assign).AsEnumerable().ToList();
            return result;
        }

        public List<Asset> GetAssetByCategoryId(string CategoryId)
        {
            return context.Asset.AsNoTracking().Where(p => p.Active == true && p.CategoryId.Equals(CategoryId)).Include(p=>p.Floor).AsEnumerable().ToList();
        }

        public Asset GetAssetByID(string AssetID)
        {
            var result = context.Asset.AsNoTracking().Where(p => p.Active == true && p.AssetId.Equals(AssetID)).Include(p => p.Floor).Include(p => p.Assign).AsEnumerable().FirstOrDefault();
            return result;
        }

        public List<Asset> AssetFilter(string AssetID, string AssetName)
        {
            if (string.IsNullOrEmpty(AssetID))
            {
                AssetID = "";
            }
            if (string.IsNullOrEmpty(AssetName))
            {
                AssetName = "";
            }
            return context.Asset.AsNoTracking().Where(p => p.Active == true && p.AssetId.Contains(AssetID) && p.AssetName.Contains(AssetName)).Include(p => p.Floor).Include(p => p.Assign).AsEnumerable().ToList();
        }

        public int AddAsset(Asset asset, Assign assign)
        {
            DaoAssign daoAssign = new DaoAssign(context);
            asset.AssetId = this.GenerateAssetID();
            var checkCode = this.CheckUniqueAssetCode(asset.AssetCode, asset.AssetId);
            if (checkCode)
            {
                asset.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                asset.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Asset.Add(asset);
                if (assign != null)
                {
                    assign.AssetId = asset.AssetId;
                    daoAssign.AssignAsset(assign);
                }
            }
            return context.SaveChanges();
        }

        public int UpdateAsset(Asset asset, Assign assign)
        {
            DaoAssign daoAssign = new DaoAssign(context);
            var entity = context.Asset.AsNoTracking().Where(p => p.AssetId.Equals(asset.AssetId)).FirstOrDefault();
            bool checkCode = this.CheckUniqueAssetCode(asset.AssetCode, asset.AssetId);
            if (checkCode)
            {
                asset.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Asset.Update(asset);
                if (assign != null)
                {
                    assign.AssetId = asset.AssetId;
                    daoAssign.AssignAsset(assign);
                }
            }
            return context.SaveChanges();
        }

        public int DeleteAsset(string assetID)
        {
            var entity = context.Asset.AsNoTracking().Where(p => p.AssetId.Equals(assetID)).FirstOrDefault();
            if (entity != null)
            {
                entity.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                entity.Active = false;
                context.Asset.Update(entity);
                var lsAssign = context.Assign.Where(p => p.AssetId.Equals(assetID) && p.Active == true).AsEnumerable().ToList();
                foreach(Assign item in lsAssign)
                {
                    item.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    item.Active = false;
                    context.Assign.Update(item);
                }
            }
            return context.SaveChanges();
        }
        
        public string GenerateAssetID()
        {
            string today = DateTime.Now.ToString("yyMM");
            string id = ShortId.Generate(true, false, 10).ToUpper();
            string result;
            result = today + "/" + id;
            return result;
        }

        public bool CheckUniqueAssetCode(string AssetCode, string AssetId)
        {
            int count = context.Asset.AsNoTracking().Where(p => p.Active == true && p.AssetCode.Equals(AssetCode) && p.AssetId != AssetId).AsEnumerable().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckUniqueAssetID(string ID)
        {
            int count = context.Category.AsNoTracking().Where(p => p.Active == true && p.CategoryId.Equals(ID)).AsEnumerable().Count();
            if (count > 0)
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
