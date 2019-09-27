using System;
using System.Collections.Generic;
using System.Text;

namespace MangoDevoluciones.Models
{
    public class ItemPrenda
    {
        public string Reference { get; set; }
        public string UrlImage { get; set; }
        public string NumberReference
        {
            get
            {
                return $"REF: {Reference}";
            }
        }
    }
}
