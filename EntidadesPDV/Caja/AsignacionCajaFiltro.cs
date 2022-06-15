using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.Caja
{
    public class AsignacionCajaFiltro
    {
        public string IdCaja { set; get; }
        public string IdAsignacion { set; get; }
        public string IdSupervisorCaja { set; get; }
        public string IdUsuarioAsignado { set; get; }
        public bool EsVigente { set; get; }
        public DateTime? FechaAsignacionInicio { set; get; }
        public DateTime? FechaAsignacionFinal { set; get; }
        public DateTime? FechaCierreInicio { set; get; }
        public DateTime? FechaCierreFinal { set; get; }
    }
}
