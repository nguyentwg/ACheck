using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.ModelViews
{
    public class ViewAudit
    {
        public Dictionary<string, string> oldValue { get; set; }
        public Dictionary<string, string> newValue { get; set; }

        public ViewAudit()
        {

        }
    }
}
