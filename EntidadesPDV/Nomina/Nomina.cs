using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.Nomina
{
    public class Nomina
    {
        private string textonulo;

        public int DocEntry { set; get; }
        public int? NumeroNomina { set; get; }
        public DateTime Fecha { set; get; }
        public string FechaTexto { get { return Fecha != DateTime.MinValue ? Fecha.ToString("dd/MM/yyyy HH:mm") : textonulo; } }
        public string UsuarioId { set; get; }
        public string UsuarioNombre { set; get; }
        public string SupervisorId { set; get; }
        public string SupervisorNombre { set; get; }
        public string TipoAntencion { set; get; }
        public string TipoAntencionDescripcion { set; get; }
        public string TipoNomina { set; get; }
        public string TipoNominaDescripcion { set; get; }
        public string CajaId { set; get; }
        public string CajaNombre { set; get; }
        public string AsignacionId { set; get; }

        public int TotalDetalle { set; get; }
        public int TotalDetalleAprobado { set; get; }
        public int TotalDetallePendiente { set; get; }
        public int TotalDetalleRechazado { set; get; }
        public int TotalDetalleAnulado { set; get; }

        public List<NominaDetalle> DetalleNomina { get; set; }

        public Nomina()
        {
            DetalleNomina = new List<NominaDetalle>();
        }
    }
}
