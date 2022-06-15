using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PDV.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDV.Reports
{
    public partial class AspReportViewer : System.Web.UI.Page
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private ReportDocument rd = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string rptName = Request.QueryString["reportName"] ?? string.Empty;
                string rptParamName = Request.QueryString["paramName"] ?? string.Empty;
                string rptParamValue = Request.QueryString["paramValue"] ?? string.Empty;


                Dictionary<string, string> parametros = new Dictionary<string, string>();
                if (rptParamName.Contains(",") && rptParamValue.Contains(","))
                {
                    string[] strParametroNombre = rptParamName.Split(',');
                    string[] strParametroValue = rptParamValue.Split(',');
                    for (int i = 0; i < strParametroNombre.Length; i++)
                    {
                        parametros.Add(strParametroNombre[i], string.Format("'{0}'", strParametroValue[i]));
                    }
                }
                else
                {
                    parametros.Add(rptParamName, string.Format("'{0}'", rptParamValue));
                }
                rd = Comunes.ImpresionDocumentos.GetReportDocument(Server.MapPath(@"\Reports"), rptName, parametros, UtilesWeb.ConeccionReporte);

                if (rd != null) // If Report Name provided then do other operation
                {
                    CrystalReportViewer1.ReportSource = rd;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rd != null && rd.IsLoaded)
            {
                rd.Close();
                rd.Dispose();
            }
            CrystalReportViewer1.Dispose();
            CrystalReportViewer1 = null;
        }

        //private ConnectionInfo GetConnectionInfo()
        //{
        //    ConnectionInfo connectionInfo = new ConnectionInfo();
        //    connectionInfo.ServerName = ConfigurationManager.AppSettings["dbServ"].ToString();
        //    connectionInfo.DatabaseName = ConfigurationManager.AppSettings["BDCompany"].ToString();
        //    connectionInfo.UserID = ConfigurationManager.AppSettings["dbUser"].ToString();
        //    connectionInfo.Password = ConfigurationManager.AppSettings["dbPsw"].ToString();

        //    return connectionInfo;
        //}

        private void PruebaImpresion()
        {

            try
            {
                log.Log(NLog.LogLevel.Info, "Entrando");
                // Establish the local endpoint for the socket.
                log.Log(NLog.LogLevel.Info, "hostname: " + Dns.GetHostName());

                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

                log.Log(NLog.LogLevel.Info, "IpHost: " + ipHost.ToString());

                IPAddress ipAddr = ipHost.AddressList[0];
                log.Log(NLog.LogLevel.Info, "ipAddr: " + ipAddr.ToString());

                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

                log.Log(NLog.LogLevel.Info, "ipEndPoint: " + ipEndPoint.ToString());

                // Create a TCP socket.
                Socket client = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint.
                client.Connect(ipEndPoint);

                // Send file fileName to the remote host with preBuffer and postBuffer data.
                // There is a text file test.txt located in the root directory.
                string fileName = "C:\\testPDV.txt";

                // Create the preBuffer data.
                string string1 = String.Format("This is text data that precedes the file.{0}", Environment.NewLine);
                byte[] preBuf = Encoding.ASCII.GetBytes(string1);

                // Create the postBuffer data.
                string string2 = String.Format("This is text data that will follow the file.{0}", Environment.NewLine);
                byte[] postBuf = Encoding.ASCII.GetBytes(string2);

                //Send file fileName with buffers and default flags to the remote device.
                Console.WriteLine("Sending {0} with buffers to the host.{1}", fileName, Environment.NewLine);
                client.SendFile(fileName, preBuf, postBuf, TransmitFileOptions.UseDefaultWorkerThread);

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception ex)
            {

                log.Error(ex);

            }

        }
    }
}
