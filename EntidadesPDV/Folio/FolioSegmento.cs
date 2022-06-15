using System;
using System.Collections.Generic;

namespace EntidadesPDV.Folio
{

    public class FolioSegmento
    {
        private string textoNulo;

        public string IdCaja { get; set; }
        public string NombreCaja { get; set; }
        public string IdFolio { get; set; }

        public DateTime FechaSegmento { get; set; }
        public string FechaSegmentoTexto { get { return FechaSegmento != DateTime.MinValue ? FechaSegmento.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }

        public int? FolioActual { get; set; }
        public int? FolioFinal { get; set; }
        public int? FolioInicial { get; set; }
        public int? CantidadFolio { get; set; }
        public int LineaSegmento { get; set; }

        public int TotalFolios { get; set; }
        public int TotalFoliosAnulados { get; set; }
        public int TotalFoliosDisponibles { get; set; }
        public int TotalFoliosUsados { get; set; }
    }
}
