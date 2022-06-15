using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesPDV.PuntoVenta;
using EntidadesPDV.Folio;
using DatosPDV.AccesoDatos;
using EntidadesPDV;
using EntidadesPDV.Usuario;

namespace NegocioPDV.PuntoVenta
{
    public class VentasBusiness
    {
        PuntoVentaDatos puntoventaAD = new DatosPDV.AccesoDatos.PuntoVentaDatos();
        NoObjectDatos noODataAD = new NoObjectDatos();
        public VentasBusiness()
        {

        }
        public VentasBusiness(UsuarioDV Info)
        {
            puntoventaAD.UsuarioActual = Info;
            noODataAD.UsuarioActual = Info;
        }
        public TransaccionesEnt ObtenerDocumentoPorId(int documentoId)
        {
            return puntoventaAD.ObtenerDocumentoPorId(documentoId);
        }

        public List<TransaccionesEnt> ObtenerDocumentosPendientes(TransaccionFiltro filtro)
        {
            if (filtro == null)
                filtro = new TransaccionFiltro();

            var salida = puntoventaAD.ObtenerDocumentosPendientes(filtro);

            return salida;
        }
        public RespuestaTransaccion FinalizarPago(Documento pago)
        {
            return puntoventaAD.FinalizarPago(pago);
        }
        public RespuestaTransaccion AnularVenta(int IdTransaccion, string IdTrack, string episodio, string observacion, string usuario, string asignacionId)
        {
            return puntoventaAD.AnularVenta(IdTransaccion, IdTrack, episodio, observacion, usuario, asignacionId);
        }
        public RespuestaTransaccion ImprimirDocumento(string idTrack, string idAsignacionCaja)
        {
            return puntoventaAD.ImprimirDocumento(idTrack, idAsignacionCaja);
        }
        public List<TransaccionesEnt> BoletasAbiertas(TransaccionesEnt boletaSearch)
        {
            return puntoventaAD.BoletasAbiertas(boletaSearch);
        }
        public Documento ObtenerPagoRecibido(int PagoId)
        {
            return puntoventaAD.ObtenerPagoRecibido(PagoId);
        }
        public List<Documento> ObtenerListaPagosRecibidos(int DocumentoId, int? DocumentoLinea, string TrackId)
        {
            return puntoventaAD.ObtenerListaPagosRecibidos(DocumentoId, DocumentoLinea, TrackId);
        }
        public RespuestaTransaccion CancelarPagoRecibido(int PagoId, string usuario, string observacion, string asignacionId)
        {
            return puntoventaAD.CancelarPagoRecibido(PagoId, usuario, observacion, asignacionId);
        }
    }
}