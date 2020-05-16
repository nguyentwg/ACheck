using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACheckAPI.Models
{
    public partial class Asset
    {
        public Asset()
        {
            AssetCategory = new HashSet<AssetCategory>();
            Assign = new HashSet<Assign>();
            DeptAsset = new HashSet<DeptAsset>();
            Image = new HashSet<Image>();
        }

        public string AssetId { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public string CreatedAt { get; set; }
        public string Creater { get; set; }
        public string UpdatedAt { get; set; }
        public string Updater { get; set; }
        public bool? Active { get; set; }

        [NotMapped]
        public string CategoryID { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }

        [NotMapped]
        public string LocationID { get; set; }
        [NotMapped]
        public string LocationName { get; set; }

        public virtual ICollection<AssetCategory> AssetCategory { get; set; }
        public virtual ICollection<Assign> Assign { get; set; }
        public virtual ICollection<DeptAsset> DeptAsset { get; set; }
        public virtual ICollection<Image> Image { get; set; }
    }
}
