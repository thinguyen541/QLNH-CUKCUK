﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLNH.GR.Desktop.Common
{
    public class DishService: BaseService
    {
        static string Route { get; set; } = "Dish";
        public DishService() : base(Route)
        {
        }
    
    }
}
