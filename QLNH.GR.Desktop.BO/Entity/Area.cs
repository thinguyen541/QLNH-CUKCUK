using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class Area : BaseEntity
    {
        public Guid? AreaID { get; set; }
        public int? AreaCode { get; set; }
        public string? AreaName { get; set; }
        public double SortOrder { get; set; }
        public Guid? BranchID { get; set; }
    }
}
