using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Models;

namespace ACheckAPI.ModelViews
{
    public class ViewAddCategory
    {
        public Category Category { get; set; }
        public virtual List<EavAttributeValue> EavAttributeValue { get; set; }

        public ViewAddCategory()
        {
            Category = new Category();
            EavAttributeValue = new List<EavAttributeValue>();
        }
    }
}
