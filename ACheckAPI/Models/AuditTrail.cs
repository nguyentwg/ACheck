using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Models
{
    public class AuditTrail
    {
        public string AuditTrailId { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Date { get; set; }
        public string KeyValues { get; set; }
    }
}
