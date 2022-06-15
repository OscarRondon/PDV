using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesPDV.CargaArchivos;

namespace NegocioPDV.CargaArchivos
{
    public class CargaArchivosBusiness
    {
        public static List<CargaArchivosDesInst> LeeLineas(string path, string path2)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException(Environment.GetEnvironmentVariable("Argument_EmptyPath"));
            else
                return LeeDocumento(path, path2);
        }

        private static List<CargaArchivosDesInst> LeeDocumento(string path, string path2)
        {
            List<string> listaLineas = new List<string>();
            List<CargaArchivosDesInst> ListaLineasDV = new List<CargaArchivosDesInst>();

            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                string str;
                while ((str = streamReader.ReadLine()) != null)
                {
                    str = str.Trim().ToString();
                    ListaLineasDV.Add(
                    new CargaArchivosDesInst
                    {
                        DP1117 = str.Substring(0, 6),
                        NCuenta = Convert.ToString(Convert.ToInt64(str.Substring(6, 11))),
                        Rut = Convert.ToString(Convert.ToInt64(str.Substring(22, 9))),
                        C = str.Substring(31, 1),
                        Monto = Convert.ToInt64(str.Substring(36, 9)),
                        Cien = str.Substring(56, 3),
                        FechaEnvio = str.Substring(74, 6),
                        No = str.Substring(80, 4)
                    });
                }
            }
            return ListaLineasDV;
        }

        private static bool DevuelveFuncion(int Modulo, string ruta)
        {
            bool resp = false;
            if (Modulo == 123) //ver como se manejan los datos del modulo o formulario de SAP
            {
                List<CargaArchivosDesInst> ListaLineasDV = new List<CargaArchivosDesInst>();
                // linq que traiga los datos directos para pasarlos a un objeto
                if (EscribeArchivo(ListaLineasDV, 0, 1000000, ruta))
                    resp = true;
            }
            else
            {
                List<CargaArchivosFAntiguo> ListaLineasFA = new List<CargaArchivosFAntiguo>();
                List<CargaArchivosFNCabecera> ListaLineasFNC = new List<CargaArchivosFNCabecera>();
                List<CargaArchivosFNDetalle> ListaLineasFND = new List<CargaArchivosFNDetalle>();
                List<CargaArchivosCSV> ListaLineasFCSV = new List<CargaArchivosCSV>();

                // linq que traiga los datos directos para pasarlos a los objetos

                if (EscribeArchivoFormatoAntiguo(ListaLineasFA, 0, 1000000, @"c:\\SAP\Cobranzas\"))
                    if (EscribeArchivoFormatoNuevo(ListaLineasFND, ListaLineasFNC, 0, 1000000, @"c:\\SAP\Cobranzas\"))
                        if (EscribeArchivoCSV(ListaLineasFCSV, 0, 1000000, @"c:\\SAP\Cobranzas\"))
                            resp = true;
            }
            return resp;
        }

        /// <summary>
        /// METODO PARA DESCUENTOS INSTITUCIONALES
        /// </summary>
        /// <param name="ListaDoc"></param>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="MapPath"></param>
        /// <returns></returns>
        public static bool EscribeArchivo(List<CargaArchivosDesInst> ListaDoc, int desde, int hasta, string MapPath)
        {
            using (StreamWriter archivo = new StreamWriter(MapPath + "HM" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt"))
            {
                try
                {
                    for (int a = desde; a <= hasta; a++)
                    {
                        archivo.WriteLine(
                            Comunes.CargaArchivos.Zero(ListaDoc[a].DP1117, "0", "DP1117", "I").ToString() +
                            "  " +
                            Comunes.CargaArchivos.Zero(ListaDoc[a].NCuenta, "0", "NCuenta", "I").ToString() +
                            Comunes.CargaArchivos.Zero(ListaDoc[a].Rut, "0", "Rut", "I").ToString() +
                            Comunes.CargaArchivos.Zero(ListaDoc[a].C, "0", "C", "I").ToString() +
                            Comunes.CargaArchivos.Zero(ListaDoc[a].Monto.ToString(), "0", "Monto", "I").ToString() +
                            Comunes.CargaArchivos.Zero(ListaDoc[a].Cien, "0", "Cien", "I").ToString() +
                            Comunes.CargaArchivos.Zero(ListaDoc[a].FechaEnvio.ToString(), "0", "FechaEnvio", "I").ToString() +
                            Comunes.CargaArchivos.Zero(ListaDoc[a].No, "0", "No", "D").ToString()
                            );
                    }
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// METODO PARA COBRANZAS INSTITUCIONALES FORMATO NUEVO 1
        /// </summary>
        /// <param name="ListaDocFNDetalle"></param>
        /// <param name="ListaDocFNCabecera"></param>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="MapPath"></param>
        /// <returns></returns>
        public static bool EscribeArchivoFormatoNuevo(List<CargaArchivosFNDetalle> ListaDocFNDetalle, List<CargaArchivosFNCabecera> ListaDocFNCabecera, int desde, int hasta, string MapPath)
        {
            using (StreamWriter archivo = new StreamWriter(MapPath + "HM" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt"))
            {
                try
                {
                    for (int a = desde; a <= hasta; a++)
                    {
                        if (a == 1)
                        {
                            archivo.WriteLine(
                            Comunes.CargaArchivos.ZeroFNCabecera(ListaDocFNCabecera[a].Numero, "0", "numero", "I").ToString() +
                            Comunes.CargaArchivos.ZeroFNCabecera(ListaDocFNCabecera[a].Fecha, "0", "fecha", "I").ToString() +
                            Comunes.CargaArchivos.ZeroFNCabecera(ListaDocFNCabecera[a].DocumentosCantidad, "0", "documentosCantidad", "I").ToString() +
                            Comunes.CargaArchivos.ZeroFNCabecera(ListaDocFNCabecera[a].DocumentosValor, "0", "documentosValor", "I").ToString() +
                            Comunes.CargaArchivos.ZeroFNCabecera(ListaDocFNCabecera[a].PrestadorRut, "0", "prestadorRut", "I").ToString()
                            );
                        }
                        else
                        {
                            archivo.WriteLine(
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].Codigo, "0", "Codigo", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].Numero, "0", "Numero", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].Fecha, "0", "Fecha", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].TipoDocumento, "0", "TipoDocumento", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].RunPaciente, "0", "RunPaciente", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].RunTitular, "0", "RunTitular", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].TransaccionFecha, "0", "TransaccionFecha", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].TransaccionId, "0", "TransaccionId", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].AtencionID, "0", "AtencionID", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].Prestacion, "0", "Prestacion", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].TipoPrestacion, "0", "TipoPrestacion", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].Bonificacion, "0", "Bonificacion", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].Seguros, "0", "Seguros", "I").ToString() +
                                Comunes.CargaArchivos.Zero(ListaDocFNDetalle[a].Copago, "0", "Copago", "I").ToString()
                                );
                        }
                    }
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// METODO PARA COBRANZAS INSTITUCIONALES FORMATO ANTIGUO
        /// </summary>
        /// <param name="ListaDocFNDetalle"></param>
        /// <param name="ListaDocFNCabecera"></param>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="MapPath"></param>
        /// <returns></returns>
        public static bool EscribeArchivoFormatoAntiguo(List<CargaArchivosFAntiguo> ListaDocFNDetalle, int desde, int hasta, string MapPath)
        {
            using (StreamWriter archivo = new StreamWriter(MapPath + "HM" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt"))
            {
                try
                {
                    for (int a = desde; a <= hasta; a++)
                    {
                        archivo.WriteLine(
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].RunPaciente, "0", "RunPaciente", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].PAO, "0", "PAO", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].RunTitular, "0", "RunTitular", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].NumeroFicha, "0", "NumeroFicha", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].RutPacienteDV, "0", "RutPacienteDV", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].NombrePaciente, "0", "NombrePaciente", "I").Substring(0, 29) +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].RutNN, "0", "RutNN", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].ItemPrestacion, "0", "ItemPrestacion", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].Ceros, "0", "Ceros", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].TipoPrestacion, "0", "TipoPrestacion", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].ValorUnitario, "0", "ValorUnitario", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].Cantidad, "0", "Cantidad", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].BAsegurador, "0", "BAsegurador", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].Badicional, "0", "Badicional", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].Copago, "0", "Copago", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].CopagoFormulado, "0", "CopagoFormulado", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].NumeroDocumento, "0", "NumeroDocumento", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].FechaAtencion, "0", "FechaAtencion", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].HorarioInHabil, "0", "HorarioInHabil", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].AmbHos, "0", "AmbHos", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].FechaDocumento, "0", "FechaDocumento", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].NumeroGuia, "0", "NumeroGuia", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].NumeroFactura, "0", "NumeroFactura", "I").ToString() +
                        Comunes.CargaArchivos.ZeroFAntiguo(ListaDocFNDetalle[a].FechaFactura, "0", "FechaFactura", "I").ToString()
                        );
                    }
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// METODO PARA COBRANZAS INSTITUCIONALES FORMATO CSV
        /// </summary>
        /// <param name="ListaDocFNDetalle"></param>
        /// <param name="ListaDocFNCabecera"></param>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="MapPath"></param>
        /// <returns></returns>
        public static bool EscribeArchivoCSV(List<CargaArchivosCSV> ListaDocFNCSV, int desde, int hasta, string MapPath)
        {
            using (StreamWriter archivo = new StreamWriter(MapPath + "HM" + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".DAT.csv"))
            {
                try
                {
                    for (int a = desde; a <= hasta; a++)
                    {
                        if (a == 1)
                        {
                            archivo.WriteLine("cuenta; tipotitular; runresponsable; numeroficha; runpaciente; nombrepaciente; rutprestador; itemprestacion; codigoprestacion; tipoprestacion; valorunitario; cantidad; asegurador; fosafe; pagoefectivo; descuentohms; numerodocumento; fechaatencion; horario; tipoatencion; fechafactura; numeroguia; numerofactura; fechafactura");
                        }
                        else
                        {
                            archivo.WriteLine(
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Cuenta, "0", "cuenta", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Tipotitular, "0", "tipotitular", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Runresponsable, "0", "runresponsable", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Numeroficha, "0", "numeroficha", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Runpaciente, "0", "runpaciente", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Nombrepaciente, "0", "nombrepaciente", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Rutprestador, "0", "rutprestador", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Itemprestacion, "0", "itemprestacion", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Codigoprestacion, "0", "codigoprestacion", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Tipoprestacion, "0", "tipoprestacion", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Valorunitario, "0", "valorunitario", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Cantidad, "0", "cantidad", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Asegurador, "0", "asegurador", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Fosafe, "0", "fosafe", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Pagoefectivo, "0", "pagoefectivo", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Descuentohms, "0", "descuentohms", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Numerodocumento, "0", "numerodocumento", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Fechaatencion, "0", "fechaatencion", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Horario, "0", "horario", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Tipoatencion, "0", "tipoatencion", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Fechafactura, "0", "fechafactura", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Numeroguia, "0", "numeroguia", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Numerofactura, "0", "numerofactura", "I").ToString() + ";" +
                            Comunes.CargaArchivos.Zero(ListaDocFNCSV[a].Fechafactura2, "0", "fechafactura", "I").ToString()
                            );
                        }
                    }
                }
                catch { }
            }
            return true;
        }
    }
}
