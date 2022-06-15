using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.PuntoVenta
{
    public class MediosPagoEnt
    {
        public string IdMedioPago { get; set; }
        public string MedioPago { get; set; }
        public bool Cheque { get; set; }
        public bool Efectivo { get; set; }
        public bool Tarjeta { get; set; }
        public bool Transferencia { get; set; }
    }
}
