using System;
using System.Collections.Generic;

namespace EntidadesPDV.Folio
{
    public class FolioGlobal
    {
        public int NumeroFolio { set; get; }
        public string Estado { set; get; }
        public DateTime? Fecha { set; get; }
        public string FechaTexto { get { return Fecha.HasValue ? Fecha.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty; } }
        public string FechaCorrelativo { get { return Fecha.HasValue ? Fecha.Value.ToString("yyyyMMddHHmm") : string.Empty; } }

        public string Ficha { get; set; }
        public string Episodio { get; set; }
        public string Titular { get; set; }
        public string Usuario { get; set; }
        public double? Monto { get; set; }
        public string CajaNombre { get; set; }
        public string Observacion { get; set; }

        public FolioGlobal()
        {

        }
    }
}
