using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.Caja
{
    public class MontoCierreCaja
    {
        public string Descripcion { get; set; }
        public double Monto { get; set; }
        public int Total { get; set; }
        public int Tipo { get; set; } //0: Ninguno, 1: Contado, 2: Documento
    }
}
