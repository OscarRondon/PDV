using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.Nomina
{
    public class NominaDetalle
    {
        private string textoNulo;

        public int DocEntry { set; get; }
        public int? NumeroNomina { set; get; }
        public int? NumeroNominaSupervisor { set; get; }
        public int LineaDetalle { set; get; }

        public string CajaNombre { set; get; }
        public string AdmisionistaNombre { set; get; }

        public string TipoDocumento { set; get; }
        public string TipoDocumentoDescripcion { set; get; }
        public string SubTipoDocumento { set; get; }
        public string NumeroDocumento { set; get; }
        public DateTime? FechaDocumento { set; get; }
        public string FechaDocumentoTexto { get { return FechaDocumento.HasValue ? FechaDocumento.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }

        public string EstadoSupervisor { set; get; }
        public DateTime? FechaAprobacionSupervisor { set; get; }
        public string FechaAprobacionSupervisorTexto { get { return FechaAprobacionSupervisor.HasValue ? FechaAprobacionSupervisor.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public string ObservacionSupervisor { set; get; }
        public string EstadoResponsable { set; get; }
        public DateTime? FechaAprobacionResponsable { set; get; }
        public string ObservacionResponsable { set; get; }

        public string NumeroOrdenAtencion { set; get; }
        public DateTime? FechaOrdenAtencion { set; get; }
        public string FechaOrdenAtencionTexto { get { return FechaOrdenAtencion.HasValue ? FechaOrdenAtencion.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }

        public string PacienteCategoria { set; get; }
        public string PacienteRut { set; get; }
        public string PacienteNombre { set; get; }
        public string TitularRut { set; get; }
        public string TitularNombre { set; get; }
        public double? MontoPago { set; get; }

        public string FichaClinica { get; set; }
        public string Episodio { get; set; }

        public string UnidadCobroEpisodio { get; set; }
        public string UnidadCobroPaciente { get; set; }

        public string TipoDocumentoSAP { get; set; }
        public int? NumeroSAP { get; set; }
    }
}
