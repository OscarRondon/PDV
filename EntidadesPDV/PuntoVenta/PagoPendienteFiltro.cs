using System;
using System.Collections.Generic;

namespace EntidadesPDV.PuntoVenta
{
    public class PagoPendienteFiltro
    {
        public DateTime? FechaDesde { set; get; }
        public DateTime? FechaHasta { set; get; }
        public string Episodio { get; set; }
        public string Ficha { get; set; }
        public string PacienteRut { get; set; }
        public string ResponsableRut { get; set; }
        public string SocioNegocioCodigo { get; set; }

    }
}
