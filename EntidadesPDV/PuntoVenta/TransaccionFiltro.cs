using System;
using System.Collections.Generic;

namespace EntidadesPDV.PuntoVenta
{
    public class TransaccionFiltro
    {
        public string AsignacionId { get; set; }
        public string Usuario { get; set; }
        public string Supervisor { get; set; }
        public string TipoDocumento { get; set; }
        public string Episodio { get; set; }
        public string FichaClinica { get; set; }
        public int? NumeroFolio { get; set; }
        public int? NumeroComprobante { get; set; }
        public TransaccionEstado Estado { get; set; }
        public TransaccionBuscar BuscarPor { get; set; }
        public DateTime? FechaDesde { set; get; }
        public DateTime? FechaHasta { set; get; }
        public string Paciente { get; set; }
        public string Responsable { get; set; }
        public string SocioNegocio { get; set; }
    }
    public enum TransaccionEstado
    {
        Todos,
        Abierto,
        Cerrado
    }
    public enum TransaccionBuscar
    {
        Todo = 0,
        Usuario = 1
    }
}
