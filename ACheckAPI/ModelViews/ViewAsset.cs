﻿using ACheckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.ModelViews
{
    public class ViewAsset
    {
        public Asset asset { get; set; }
        public Assign assign { get; set; }
        public DeptAsset DeptAsset { get; set; }
        public virtual List<EavAttributeValue> EavAttributeValue { get; set; }

        public ViewAsset()
        {
            asset = new Asset();
            assign = new Assign();
            DeptAsset = new DeptAsset();
            EavAttributeValue = new List<EavAttributeValue>();
        }
    }
}
