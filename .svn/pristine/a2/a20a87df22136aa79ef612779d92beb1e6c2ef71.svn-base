using System;
using System.Collections.Generic;

namespace EntidadesPDV.Folio
{
    public class Folio
    {
        private string textoNulo;

        public string IdFolio { set; get; }
        public DateTime? FechaRegistro { set; get; }
        public string FechaRegistroTexto { get { return FechaRegistro.HasValue ? FechaRegistro.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public int? FolioInicial { set; get; }
        public int? FolioFinal { set; get; }
        public int? TipoDocumento { set; get; }
        public string DocumentoDescripcion { get { return TipoDocumento.HasValue ? (TipoDocumento.Value == 38 ? "Boleta Exenta" : null) : null; } }
        public int? CantidadSegmento { set; get; }
        public int? CantidadSegmentoPendiente { set; get; }
        public string IdUsuarioCreacion { get; set; }
        public string NombreUsuarioCreacion { get; set; }
        public string IdUsuarioSupervisor { get; set; }
        public string NombreUsuarioSupervisor { get; set; }
        public bool EsUltimoFolio { get; set; }

        public bool EsAnulado { get; set; }
        public string MotivoAnulacion { get; set; }

        public int TotalFolios { get; set; }
        public int TotalFoliosAnulados { get; set; }
        public int TotalFoliosDisponibles { get; set; }
        public int TotalFoliosUsados { get; set; }

        public List<FolioSegmento> ListaSegmentos { get; set; }

        public Folio()
        {
            ListaSegmentos = new List<FolioSegmento>();
        }
    }
}
