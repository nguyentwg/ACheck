using ACheckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.ModelViews
{
    public class ViewCategory
    {
        public int countAsset { get; set; }
        public int countSubCategory { get; set; }
        public virtual List<Asset> lsAsset { get; set; }
        public virtual List<Category> lsSubCategory { get; set; }
        public virtual Category Category { get; set; }

        public ViewCategory()
        {
            countAsset = 0;
            countSubCategory = 0;
            lsSubCategory = new List<Category>();
            lsAsset = new List<Asset>();
        }
    }
}
