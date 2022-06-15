using System;
using System.Collections.Generic;
using System.Configuration;

namespace EntidadesPDV.Caja
{
    public class CajasEnt
    {
        public string IdCaja { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string IdentificacionCaja { get; set; }
        public bool EsFuncionalidadCaja { get { return IdFuncionalidadCaja == "C"; } }
        public string IdFuncionalidadCaja { get; set; }
        public string FuncionalidadCaja { get; set; }
        public string IdFuncionCaja { get; set; }
        public string FuncionCaja { get; set; }
        public string IdSeccionCaja { get; set; }
        public string SeccionCaja { get; set; }
        public string IdPisoCaja { get; set; }
        public string PisoCaja { get; set; }
        public bool HabilitaCaja { get; set; }
        public string MacCaja { get; set; }
        public string IpCaja { get; set; }

        public string MontoFijo
        {
            get
            {
                string resultado = string.Empty;
                switch (this.IdFuncionCaja)
                {
                    case "A"://Ambulatorio
                        resultado = ConfigurationManager.AppSettings["montoAmbulatorio"];
                        break;
                    case "U"://Urgencia
                        resultado = ConfigurationManager.AppSettings["montoUrgencia"];
                        break;
                    case "H"://Hospitalario
                        resultado = ConfigurationManager.AppSettings["montoHospitalario"];
                        break;
                    default:
                        resultado = "0";
                        break;
                }
                return string.Format("{0:n0}", int.Parse(resultado));
            }
        }

        public string IdSupervisorCaja { get; set; }
        public string SupervisorCaja { get; set; }

    }
}
