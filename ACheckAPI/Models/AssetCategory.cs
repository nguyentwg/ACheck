using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class AssetCategory
    {
        public string Guid { get; set; }
        public string CategoryId { get; set; }
        public string AssetId { get; set; }
        public string Type { get; set; }
        [JsonIgnore]
        public virtual Asset Asset { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
