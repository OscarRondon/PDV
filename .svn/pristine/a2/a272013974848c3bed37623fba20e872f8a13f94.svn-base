using System;
using System.Collections.Generic;

namespace EntidadesPDV.PuntoVenta
{
    public class TransaccionesEnt
    {
        #region Datos Documento
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public int? NumeroFolio { get; set; }
        public int? NumeroComprobante { get; set; }

        public int IdTransaccion { get; set; }
        public string IdTrack { get; set; }

        public DateTime FechaTransaccion { get; set; }
        public string FechaTransaccionTexto { get { return FechaTransaccion != DateTime.MinValue ? FechaTransaccion.ToString("dd/MM/yyyy HH:mm") : string.Empty; } }
        public string FechaTransaccionCorrelativo { get { return FechaTransaccion != DateTime.MinValue ? FechaTransaccion.ToString("yyyyMMddHHmm") : string.Empty; } }


        public string CodigoTarifaTransaccion { get; set; }
        public string TarifaTransaccion { get; set; }
        public string NumeroBono { get; set; }
        public string NroGarantia { get; set; }

        public string FichaClinica { get; set; }
        public string Episodio { get; set; }

        public string Estado { get; set; }
        public bool Impreso { get; set; }
        public bool Pagado { get; set; }
        public TipoTransaccion Tipo { get; set; }
        public string TipoTexto
        {
            get
            {
                return
                    Tipo == TipoTransaccion.Boleta ? "Boleta" :
                    Tipo == TipoTransaccion.Convenio ? "Convenio" :
                    Tipo == TipoTransaccion.Factura ? "Factura" :
                    Tipo == TipoTransaccion.OrdenVenta ? "Orden de Venta" :
                    Tipo == TipoTransaccion.Pagare ? "Pagaré" :
                    Tipo == TipoTransaccion.Anticipo ? "Anticipo" :
                    Tipo == TipoTransaccion.Imed ? "IMED" : string.Empty;
            }
        }
        public string BoletaAsociada { get; set; }
        #endregion

        #region "Datos Paciente"
        public string RutPaciente { get; set; }
        public string PacienteNombre { get; set; }
        public string DireccionPaciente { get; set; }
        public string ComunaPaciente { get; set; }
        #endregion

        #region "Datos Responsable"
        public string RutResponsable { get; set; }
        public string ResponsableNombre { get; set; }
        public string PrevisionResponsable { get; set; }
        public string DireccionResponsable { get; set; }
        public string ComunaResponsable { get; set; }
        #endregion

        #region "Datos Médico"
        public string NombreMedico { get; set; }
        public string EspecialidadMedico { get; set; }
        public string ServicioAtencion { get; set; }
        public string CodigoUnidad { get; set; }
        #endregion

        #region "Datos Transacción"
        public double MontoPago { get; set; }
        public string CodigoPrestacion { get; set; }
        public string DescripcionPrestacion { get; set; }
        public string CentroCosto { get; set; }
        public int Cantidad { get; set; }
        public int ValorUnitario { get; set; }
        public int SubTotal { get; set; }
        public decimal BonificacionFinanciador { get; set; }
        public int BonificacionComplemnentaria { get; set; }
        public int CopagoBeneficiario { get; set; }

        public string Categoria { get; set; }
        public string Convenio { get; set; }
        public string CategoriaConvenio
        {
            get
            {
                string texto = string.Empty;

                if (!string.IsNullOrEmpty(this.Categoria) && !string.IsNullOrEmpty(this.Convenio))
                {
                    texto = (this.Categoria == this.Convenio) ?
                        this.Categoria :
                        string.Format("{0} / {1}", this.Categoria, this.Convenio);

                }
                else if (!string.IsNullOrEmpty(this.Categoria))
                {
                    texto = this.Categoria;
                }
                else if (!string.IsNullOrEmpty(this.Convenio))
                {
                    texto = this.Convenio;
                }
                return texto;
            }
        }
        #endregion

        #region Datos Asiento
        public int? AsientoNumero { get; set; }
        public int? AsientoLinea { get; set; }
        public int? AsientoLineaTotal { get; set; }
        public string AsientoCuota
        {
            get
            {
                return AsientoLinea.HasValue && AsientoLineaTotal.HasValue && AsientoLineaTotal.Value > 1 ?
                    string.Format("{0} de {1}", AsientoLinea.Value + 1, AsientoLineaTotal.Value) :
                    string.Empty;
            }
        }
        public string AsientoCuotaCorrelativo
        {
            get
            {
                return AsientoLinea.HasValue && AsientoLineaTotal.HasValue && AsientoLineaTotal.Value > 1 ?
                    string.Concat(AsientoLinea.Value + 1, AsientoLineaTotal.Value).ToString().PadLeft(6, '0') :
                    string.Empty;
            }
        }
        public DateTime? FechaVencimiento { get; set; }
        /// <summary>
        /// Campo que se utiliza para identifcar si aplica, si tiene una cuota anterior pendiente de pago.
        /// </summary>
        public bool TieneCuotaPendiente { get; set; }
        #endregion

        #region Socio Negocio
        public string SocioNegocioCode { get; set; }
        public string SocioNegocioName { get; set; }
        #endregion

        #region "Listados
        public List<TransaccionesEnt> DetalleAtencion { get; set; }
        #endregion

        public TransaccionesEnt()
        {
            DetalleAtencion = new List<TransaccionesEnt>();
        }
    }

    public enum TipoTransaccion
    {
        Ninguno = 0,
        OrdenVenta = 1,
        Boleta = 2,
        Pagare = 3,
        Convenio = 4,
        Factura = 5,
        Anticipo = 6,
        Imed = 7
    }
}
