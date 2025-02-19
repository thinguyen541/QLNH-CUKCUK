﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class SuggestMoney : BaseEntity
    {
        public decimal? Amount { get; set; }
        public string? DisplayText { get { if (Amount >= 0) { return Amount.Value.ToString("F2", new CultureInfo("en-us")); } else return "Custom"; } }

        public bool? IsCustomAmount { get; set; }
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
