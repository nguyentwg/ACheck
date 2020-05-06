using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class Building
    {
        public Building()
        {
            Floor = new HashSet<Floor>();
        }

        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string BuildingAddress { get; set; }
        public string FloorNumber { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string Creater { get; set; }
        public string UpdatedAt { get; set; }
        public string Updater { get; set; }
        public bool? Active { get; set; }

        [JsonIgnore]
        public virtual ICollection<Floor> Floor { get; set; }
    }
}
