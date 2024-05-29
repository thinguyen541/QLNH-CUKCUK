using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class Card : BaseEntity
    {
        public Guid CardID { get; set; }    
        public string CardName { get; set; }    
        public EnumCardType CardType { get; set; }   
        public bool IsSelected { get; set; }
        public string ImagePath { get; set; }
    }
}
