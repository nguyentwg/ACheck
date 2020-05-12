using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class Assign
    {
        public string AssignId { get; set; }
        public string AssetId { get; set; }
        public string ReceiverBy { get; set; }
        public string Supporter { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CreatedAt { get; set; }
        public string Creater { get; set; }
        public string UpdatedAt { get; set; }
        public string Updater { get; set; }
        public bool? Active { get; set; }

        public virtual Asset Asset { get; set; }
    }
}
