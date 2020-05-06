using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.ModelViews
{
    public class ViewArrangeCategory
    {
        public string Category_ID { get; set; }
        public int No { get; set; }

        public ViewArrangeCategory()
        {
            Category_ID = null;
            No = 0;
        }
    }
}
