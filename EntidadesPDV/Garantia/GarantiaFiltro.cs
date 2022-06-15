using System;
using System.Collections.Generic;

namespace EntidadesPDV.Garantia
{
    public class GarantiaFiltro
    {
        public string IdAsignacion { get; set; }
        public int DocEntry { get; set; }
        public int? DocNum { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Episodio { get; set; }
        public string TipoDocumento { get; set; }
        public EstadoGarantia Estado { get; set; }
        public string ResponsableRut { get; set; }
        public string PacienteRut { get; set; }
    }
}
