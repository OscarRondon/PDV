using System;
using System.Collections.Generic;

namespace EntidadesPDV.Garantia
{
    public class GarantiaEnt
    {
        public string textoNulo;

        #region Datos Pago
        public string GarIdAsignacion { get; set; }
        public string IdTrack { get; set; }
        public int GarDocEntry { get; set; }
        public int GarDocNum { get; set; }
        public DateTime GarFechaIngreso { get; set; }
        public string GarFechaIngresoTexto { get { return this.GarFechaIngreso != DateTime.MinValue ? this.GarFechaIngreso.ToString("dd/MM/yyyy") : string.Empty; } }
        public DateTime GarFechaFin { get; set; }
        public string GarFechaFinTexto { get { return this.GarFechaFin != DateTime.MinValue ? this.GarFechaFin.ToString("dd/MM/yyyy") : string.Empty; } }
        public string GarFichaClinica { get; set; }
        public string GarEpisodio { get; set; }
        public int? GarIndicador { get; set; }
        public int? NumPagRecib { get; set; }
        public int? NumDocBoleta { get; set; }

        public string GarIndicadorTexto
        {
            get
            {
                return this.GarIndicador.HasValue ? (this.GarIndicador.Value == 0 ? "EFECTIVO" : "CHEQUE") : string.Empty;
            }
        }
        public string GarEntidad { get; set; }
        public string GarEntidadDescripcion { get; set; }
        public string GarNumeroCheque { get; set; }
        public double GarMonto { get; set; }
        public string GarMontoTexto { get { return string.Format("$ {0:n0}", GarMonto); } }
        public string IdTipoDocumento { get; set; }
        public string TipoDocumentoTexto
        {
            get
            {
                string tipo = string.Empty;
                switch (IdTipoDocumento)
                {
                    case "G":
                        tipo = "Garantía";
                        break;
                    case "P":
                        tipo = "Pagaré";
                        break;
                    case "C":
                        tipo = "Carta Resguardo";
                        break;
                    case "A":
                        tipo = "Anticipo";
                        break;
                    default:
                        break;
                }
                return tipo;
            }
        }
        //JMpublic bool? Vencido { set; get; }

        public EstadoGarantia GarEstado { get; set; }
        public string GarEstadoTexto
        {
            get
            {
                string estado = string.Empty;
                switch (GarEstado)
                {
                    case EstadoGarantia.Activo:
                        estado = "Disponible";
                        break;
                    case EstadoGarantia.Devuelto:
                        estado = "Devuelto";
                        break;
                    case EstadoGarantia.Aplicado:
                        estado = "Aplicado";
                        break;
                    case EstadoGarantia.Lleno:
                        estado = "Lleno";
                        break;
                    case EstadoGarantia.Protesto:
                        estado = "Protesto";
                        break;
                    case EstadoGarantia.Anulado:
                        estado = "Anulado";
                        break;
                    case EstadoGarantia.Vencido:
                        estado = "Vencido";
                        break;
                    default:
                        break;
                }
                return estado;
            }
        }

        public IDictionary<string, string> TipoDocumento { get; set; }
        #endregion

        #region Datos Responsable
        public string GarRutResponsable { get; set; }
        public string PasResponsable { get; set; }
        public string NombreResponsable { get; set; }
        public string DireccionResponsable { get; set; }
        public string ComunaResponsable { get; set; }
        #endregion

        #region Datos Paciente
        public string GarRutPaciente { get; set; }
        public string PasPaciente { get; set; }
        public string NombrePaciente { get; set; }
        public string DireccionPaciente { get; set; }
        public string ComunaPaciente { get; set; }

        public int? DiasVencimiento { get; set; }
        #endregion

        #region Comentario
        public string Comentario { set; get; }
        #endregion

        #region "Listados"
        public IDictionary<string, string> LstComunas { get; set; }
        #endregion

        #region Datos de caja
        public string IdAsignacion { set; get; }

        public string IdCaja { set; get; }
        #endregion


        public GarantiaEnt()
        {
            LstComunas = new Dictionary<string, string>();
        }
    }
    public enum EstadoGarantia
    {
        Vencido = -1,
        NoAsignado = 0,
        Activo = 1,
        Devuelto = 2,
        Aplicado = 3,
        Lleno = 4,
        Protesto = 5,
        Anulado = 6,
    }
}
