using System;
using System.Collections.Generic;

namespace EntidadesPDV.PuntoVenta
{
    public class Documento
    {
        public string IdCaja { get; set; }
        public string IdAsignacion { get; set; }

        public string ResponsableRut { set; get; }
        public string ResponsableNombre { set; get; }
        public DateTime FechaDocumento { set; get; }
        public string FechaDocumentoTexto { get { return FechaDocumento != DateTime.MinValue ? FechaDocumento.ToString("dd/MM/yyyy") : string.Empty; } }
        public string FechaDocumentoCorrelativo { get { return FechaDocumento != DateTime.MinValue ? FechaDocumento.ToString("yyyyMMdd") : string.Empty; } }

        public DateTime FechaVencimientoDocumento { set; get; }
        public string FechaVencimientoDocumentoTexto { get { return FechaVencimientoDocumento != DateTime.MinValue ? FechaVencimientoDocumento.ToString("dd/MM/yyyy") : string.Empty; } }
        public string FechaVencimientoDocumentoCorrelativo { get { return FechaVencimientoDocumento != DateTime.MinValue ? FechaVencimientoDocumento.ToString("yyyyMMdd") : string.Empty; } }

        public int DocEntry { set; get; }
        public int DocNum { set; get; }

        public double MontoEfectivo { set; get; }
        public double MontoRedondeo { set; get; }
        public double MontoCheque { set; get; }
        public double MontoTarjeta { set; get; }
        public double MontoTotal { set; get; }

        #region Transferencia
        public DateTime? FechaTransferencia { set; get; }
        public string FechaTransferenciaTexto { get { return FechaTransferencia.HasValue ? FechaTransferencia.Value.ToString("dd/MM/yyyy") : string.Empty; } }

        public double MontoTransferencia { set; get; }
        public string NumeroTransferencia { get; set; }
        public string BancoTransferencia { get; set; }
        public string BancoTransferenciaNombre { get; set; }
        #endregion

        public string CodigoFolio { set; get; }
        public string FolioAsignado { set; get; }
        public int LineaFolio { set; get; }

        public string CajaNombre { get; set; }
        public string UsuarioCajaId { get; set; }
        public string UsuarioCajaNombre { get; set; }
        public string IdTrack { get; set; }

        public int? AsientoId { get; set; }
        public int? AsientoLinea { get; set; }

        public List<DocumentoLinea> ListaArticulos { set; get; }
        public List<DocumentoPago> ListaPago { set; get; }

        public TipoTransaccion TipoPago { get; set; }

        public string BoletaAsociada { get; set; }

        public Documento()
        {
            ListaArticulos = new List<DocumentoLinea>();
            ListaPago = new List<DocumentoPago>();
        }
    }
    public class DocumentoLinea
    {
        public string CodigoArticulo { set; get; }
        public double Cantidad { set; get; }
        public double Precio { set; get; }

    }
    public class DocumentoPago
    {
        public string CodigoBanco { set; get; }
        public string NombreBanco { set; get; }

        #region Cheque
        public double MontoCheque { set; get; }
        public bool EsValeVista { set; get; }
        public int NumeroCheque { set; get; }
        public DateTime? FechaVencimientoCheque { set; get; }
        public string FechaVencimientoChequeTexto { get { return FechaVencimientoCheque.HasValue ? FechaVencimientoCheque.Value.ToString("dd/MM/yyyy") : string.Empty; } }
        #endregion

        #region Tarjeta
        public double MontoTarjeta { set; get; }
        public int CodigoTarjeta { set; get; }
        public string NombreTarjeta { set; get; }
        public string NumeroOperacionTarjeta { set; get; }
        public string NumeroTajeta { set; get; }
        public int NumeroCuotasTarjeta { set; get; }
        #endregion
    }
}
