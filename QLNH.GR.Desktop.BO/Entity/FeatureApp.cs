using QLNH.GR.Desktop.BO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class FeatureApp: BaseEntity
    {
        public string? FeatureName { get; set; }
        public string? FeatureKey { get; set; }
        public int SortOrder { get; set; }

        public string IconName { get; set; }


    }
}
