using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class Table : BaseEntity
    {
        public Guid TableID { get; set; }

        public string TableName { get; set; }
        public int TableCode { get; set; }
        public Guid? AreaID { get; set; }
        public double? CorX { get; set; }
        public double? CorY { get; set; }
        public Guid? BranchID { get; set; }
        public int? Status { get; set; }
        
  

        private bool _isServing;
        public bool IsServing
        {
            get => _isServing;
            set
            {
                if (_isServing != value)
                {
                    _isServing = value;
                    OnPropertyChanged(nameof(IsServing));
                }
            }
        }

        private string _servingTime;
        public string ServingTime
        {
            get => _servingTime;
            set
            {
                if (_servingTime != value)
                {
                    _servingTime = value;
                    OnPropertyChanged(nameof(ServingTime));
                }
            }
        }
    }
}
