using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class Asset
    {
        public Asset()
        {
            Assign = new HashSet<Assign>();
        }

        public string AssetId { get; set; }
        public string CategoryId { get; set; }
        public string FloorId { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public string CreatedAt { get; set; }
        public string Creater { get; set; }
        public string UpdatedAt { get; set; }
        public string Updater { get; set; }
        public bool? Active { get; set; }

        public virtual Category Category { get; set; }
        public virtual Floor Floor { get; set; }
        public virtual ICollection<Assign> Assign { get; set; }
    }
}
