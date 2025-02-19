﻿
using QLNH.GR.Desktop.BO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class OrderDetail : BaseEntity
    {
        public Guid? OrderDetailId { get; set; }
        public Guid? OrderId { get; set; }

        public List<DetailItem> ListDetailItem { get; set; }
        public List<DetailItem> ListNormalDetailItem
        {
            get
            {
                if (ListDetailItem != null)
                {
                    return ListDetailItem.Where(item => item.DetailItemType == EnumDetailItemType.Normal).ToList();
                }
                return null;
            }
        }
        public List<DetailItem> ListModifierDetailItem
        {
            get
            {
                if (ListDetailItem != null)
                {
                    return ListDetailItem.Where(item => item.DetailItemType == EnumDetailItemType.Modifier).ToList();
                }
                return null;
            }
        }

        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; } = 0;

        public decimal? AmountBeforeTax { get; set; } = 0;

        public decimal? AmountAfterTax { get; set; } = 0;

        public EnumOrderDetailStatus OrderDetailStatus { get; set; }

        private bool _isSendKitchen;
        public bool IsSendKitchen
        {
            get => OrderDetailStatus == EnumOrderDetailStatus.Send;
            set
            {
                if (_isSendKitchen != value)
                {
                    _isSendKitchen = value;
                    OnPropertyChanged(nameof(IsSendKitchen));
                }
            }
        }

        public EnumOrderDetailType OrderDetailType { get; set; } = EnumOrderDetailType.Normal;

        public bool IsDisplay { get
            {
                return OrderDetailType == EnumOrderDetailType.Normal || OrderDetailType == EnumOrderDetailType.Modifier || OrderDetailType == EnumOrderDetailType.Combo;
            } }

    }
}
