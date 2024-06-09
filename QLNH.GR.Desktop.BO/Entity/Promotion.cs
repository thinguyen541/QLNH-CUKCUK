using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class Promotion : BaseEntity
    {
        public Guid PromotionId { get; set; }
        public string? PromotionName { get; set; }
        public decimal? PercentageValue {get;set;}
        public decimal? AmountValue {get;set;}
        public EnumPromotionValueType? PromotionValueType { get; set; }

        public decimal? AmountCondition { get;set;}
        public EnumAmountConditionType? AmountConditionType { get;set;}
        public DateTime? StartTime { get;set;}
        public DateTime? EndTime { get;set;}

        private bool _selected;
        public bool IsSelected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
    }
}
