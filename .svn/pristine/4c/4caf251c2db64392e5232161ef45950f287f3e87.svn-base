using System;
using System.Collections.Generic;

namespace EntidadesPDV.Folio
{

    public class FolioAnulado
    {
        private string textoNulo;

        public string IdCaja { get; set; }
        public string NombreCaja { get; set; }
        public string IdFolio { get; set; }
        public int LineaAnulacion { get; set; }

        public bool Anulado { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public string FechaAnulacionTexto { get { return FechaAnulacion != DateTime.MinValue ? FechaAnulacion.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public int? FolioFinal { get; set; }
        public int? FolioInicial { get; set; }
        public string Motivo { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public string NombreUsuarioAnulacion { get; set; }
        public string IdUsuarioAprobacion { get; set; }
        public string NombreUsuarioAprobacion { get; set; }
    }
}
