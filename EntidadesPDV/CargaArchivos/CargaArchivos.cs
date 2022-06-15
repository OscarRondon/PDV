using System;

namespace EntidadesPDV.CargaArchivos
{
    public class CargaArchivosDesInst
    {
        public string DP1117 { get; set; }
        public string NCuenta { get; set; }
        public string Rut { get; set; }
        public string C { get; set; }
        public long Monto { get; set; }
        public string Cien { get; set; }
        public string FechaEnvio { get; set; }
        public string No { get; set; }
    }
    public class CargaArchivosFNCabecera
    {
        public string Numero { get; set; }
        public string Fecha { get; set; }
        public string DocumentosCantidad { get; set; }
        public string DocumentosValor { get; set; }
        public string PrestadorRut { get; set; }
    }
    public class CargaArchivosFNDetalle
    {
        public string Codigo { get; set; }
        public string Numero { get; set; }
        public string Fecha { get; set; }
        public string TipoDocumento { get; set; }
        public string RunPaciente { get; set; }
        public string RunTitular { get; set; }
        public string TransaccionId { get; set; }
        public string TransaccionFecha { get; set; }
        public string AtencionID { get; set; }
        public string Prestacion { get; set; }
        public string TipoPrestacion { get; set; }
        public string Bonificacion { get; set; }
        public string Seguros { get; set; }
        public string Copago { get; set; }
    }
    public class CargaArchivosFAntiguo
    {
        public string RunPaciente { get; set; }
        public string PAO { get; set; }
        public string RunTitular { get; set; }
        public string NumeroFicha { get; set; }
        public string RutPacienteDV { get; set; }
        public string NombrePaciente { get; set; }
        public string RutNN { get; set; }
        public string ItemPrestacion { get; set; }
        public string Ceros { get; set; }
        public string TipoPrestacion { get; set; }
        public string ValorUnitario { get; set; }
        public string Cantidad { get; set; }
        public string BAsegurador { get; set; }
        public string Badicional { get; set; }
        public string Copago { get; set; }
        public string CopagoFormulado { get; set; }
        public string NumeroDocumento { get; set; }
        public string FechaAtencion { get; set; }
        public string HorarioInHabil { get; set; }
        public string AmbHos { get; set; }
        public string FechaDocumento { get; set; }
        public string NumeroGuia { get; set; }
        public string NumeroFactura { get; set; }
        public string FechaFactura { get; set; }
    }
    public class CargaArchivosCSV
    {
    public string Cuenta { get; set; }
    public string Tipotitular { get; set; }
    public string Runresponsable { get; set; }
    public string Numeroficha { get; set; }
    public string Runpaciente { get; set; }
    public string Nombrepaciente { get; set; }
    public string Rutprestador { get; set; }
    public string Itemprestacion { get; set; }
    public string Codigoprestacion { get; set; }
    public string Tipoprestacion { get; set; }
    public string Valorunitario { get; set; }
    public string Cantidad { get; set; }
    public string Asegurador { get; set; }
    public string Fosafe { get; set; }
    public string Pagoefectivo { get; set; }
    public string Descuentohms { get; set; }
    public string Numerodocumento { get; set; }
    public string Fechaatencion { get; set; }
    public string Horario { get; set; }
    public string Tipoatencion { get; set; }
    public string Fechafactura { get; set; }
    public string Numeroguia { get; set; }
    public string Numerofactura { get; set; }
    public string Fechafactura2 { get; set; }
}
}