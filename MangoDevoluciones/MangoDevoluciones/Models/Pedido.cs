using System;
using System.Collections.Generic;
using System.Text;

namespace MangoDevoluciones.Models
{
    public class Pedido
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
    }
}
