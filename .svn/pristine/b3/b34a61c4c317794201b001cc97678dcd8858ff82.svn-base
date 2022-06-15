using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer.ClientDoc;
using CrystalDecisions.ReportAppServer.Controllers;
using CrystalDecisions.Shared;
using EntidadesPDV;
using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;

namespace Comunes
{
    public class ImpresionDocumentos
    {
        public static ReportDocument GetReportDocument(string MapPath, string Reporte, Dictionary<string, string> listaParametros, ConnectionInfo connectionInfo)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                //Carga el Reporte seleccionado segun su MapPath
                rd.Load(Path.Combine(MapPath, Reporte));

                //Agrega los Parametros definidos
                ParameterFieldDefinitions parameterFieldDefinitions = rd.DataDefinition.ParameterFields;

                foreach (var item in listaParametros)
                {
                    //Agrega los Parametros definidos
                    ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue() { Value = item.Value };

                    ParameterValues currentParameterValues = new ParameterValues() { parameterDiscreteValue };

                    ParameterFieldDefinition parameterFieldDefinition = parameterFieldDefinitions[item.Key]; //nombre del parametro en el reporte
                    parameterFieldDefinition.ApplyCurrentValues(currentParameterValues);
                }

                SetConeccionReport(connectionInfo, ref rd);

                return rd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void SetConeccionReport(ConnectionInfo connectionInfo, ref ReportDocument rd)
        {
            try
            {
                rd.DataSourceConnections[0].IntegratedSecurity = false;
                rd.DataSourceConnections[0].SetConnection(connectionInfo.ServerName, connectionInfo.DatabaseName, connectionInfo.UserID, connectionInfo.Password);
                if (rd.Subreports != null && rd.Subreports.Count > 0)
                {
                    for (int i = 0; i < rd.Subreports.Count; i++)
                    {
                        rd.Subreports[i].DataSourceConnections[0].IntegratedSecurity = false;
                        rd.Subreports[i].DataSourceConnections[0].SetConnection(connectionInfo.ServerName, connectionInfo.DatabaseName, connectionInfo.UserID, connectionInfo.Password);
                    }
                }

                Tables tables = rd.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
                {
                    TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                    tableLogOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(tableLogOnInfo);
                }
                if (rd.Subreports != null && rd.Subreports.Count > 0)
                {
                    for (int i = 0; i < rd.Subreports.Count; i++)
                    {
                        Tables tablesSub = rd.Subreports[i].Database.Tables;
                        foreach (CrystalDecisions.CrystalReports.Engine.Table tableS in tablesSub)
                        {
                            TableLogOnInfo tableLogOnInfo = tableS.LogOnInfo;
                            tableLogOnInfo.ConnectionInfo = connectionInfo;
                            tableS.ApplyLogOnInfo(tableLogOnInfo);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public static RespuestaTransaccion ExportReport(string MapPath, string Reporte, string Impresora, Dictionary<string, string> listaParametros, ConnectionInfo connectionInfo, Boolean isOki = false)
        {
            RespuestaTransaccion mensaje = new RespuestaTransaccion();
            try
            {
                //Carga el Reporte seleccionado segun su MapPath
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(MapPath, Reporte));

                //Agrega los Parametros definidos
                ParameterFieldDefinitions parameterFieldDefinitions = rd.DataDefinition.ParameterFields;

                foreach (var item in listaParametros)
                {
                    //Agrega los Parametros definidos
                    ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue() { Value = item.Value };

                    ParameterValues currentParameterValues = new ParameterValues() { parameterDiscreteValue };

                    ParameterFieldDefinition parameterFieldDefinition = parameterFieldDefinitions[item.Key]; //nombre del parametro en el reporte
                    parameterFieldDefinition.ApplyCurrentValues(currentParameterValues);
                }

                SetConeccionReport(connectionInfo, ref rd);

                //Combina el reporte ya cargado y lo transforma en PDF
                string inputFile = Path.Combine(MapPath, Guid.NewGuid().ToString() + ".pdf");
                mensaje.Data = inputFile;

                ExportOptions CrExportOptions = rd.ExportOptions;
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = new DiskFileDestinationOptions() { DiskFileName = inputFile };
                CrExportOptions.FormatOptions = new PdfRtfWordFormatOptions();

                rd.Export();

                if (isOki)
                {
                    PrintLayoutSettings ply = new PrintLayoutSettings();
                    ply.Scaling = PrintLayoutSettings.PrintScaling.DoNotScale;

                    PrinterSettings pdr = new PrinterSettings();
                    pdr.PrinterName = Impresora;

                    PageSettings pgs = new PageSettings();

                    rd.PrintToPrinter(pdr, pgs, false, ply);
                }
                else
                {
                    string fileName = inputFile;
                    byte[] pdfByteArray = System.IO.File.ReadAllBytes(fileName);
                    mensaje.DataByte = pdfByteArray;
                    string base64EncodedPDF = System.Convert.ToBase64String(pdfByteArray);
                    mensaje.Data = base64EncodedPDF;
                }

                File.Delete(inputFile);
            }
            catch (Exception ex)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = ex.Message;
            }

            return mensaje;
        }
        public static byte[] ExportReportByte(string MapPath, string Reporte, Dictionary<string, string> listaParametros, ConnectionInfo connectionInfo)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                //Carga el Reporte seleccionado segun su MapPath
                rd.Load(Path.Combine(MapPath, Reporte));

                //Agrega los Parametros definidos
                ParameterFieldDefinitions parameterFieldDefinitions = rd.DataDefinition.ParameterFields;

                foreach (var item in listaParametros)
                {
                    //Agrega los Parametros definidos
                    ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue() { Value = item.Value };

                    ParameterValues currentParameterValues = new ParameterValues() { parameterDiscreteValue };

                    ParameterFieldDefinition parameterFieldDefinition = parameterFieldDefinitions[item.Key]; //nombre del parametro en el reporte
                    parameterFieldDefinition.ApplyCurrentValues(currentParameterValues);
                }

                SetConeccionReport(connectionInfo, ref rd);

                //Combina el reporte ya cargado y lo transforma en PDF
                string inputFile = Path.Combine(MapPath, Guid.NewGuid().ToString() + ".rpt");

                ExportOptions CrExportOptions = rd.ExportOptions;
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.CrystalReport;
                CrExportOptions.DestinationOptions = new DiskFileDestinationOptions() { DiskFileName = inputFile };
                CrExportOptions.FormatOptions = new PdfRtfWordFormatOptions();

                rd.Export();

                byte[] byteArray = System.IO.File.ReadAllBytes(inputFile);

                File.Delete(inputFile);
                return byteArray;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
