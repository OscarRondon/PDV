using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CrystalDecisions.Shared;

namespace PDV.Clases
{
    public sealed class UtilesWeb
    {
        #region Propiedades
        public static ConnectionInfo ConeccionReporte
        {
            get
            {
                ConnectionInfo connectionInfo = new ConnectionInfo();
                connectionInfo.ServerName = ConfigurationManager.AppSettings["dbServ"].ToString();
                connectionInfo.DatabaseName = ConfigurationManager.AppSettings["BDCompany"].ToString();
                connectionInfo.UserID = ConfigurationManager.AppSettings["dbUser"].ToString();
                connectionInfo.Password = ConfigurationManager.AppSettings["dbPsw"].ToString();

                return connectionInfo;
            }
        }

        #endregion

        #region Metodos
        public static bool CajaValidaIp(string ipEntrada)
        {
            bool cajaValida = false;
            try
            {
                if (ConfigurationManager.AppSettings["CajaValidaIP"].ToString() == "1")
                {
                    string ipHost = HttpContext.Current.Request.UserHostAddress;

                    if (ipEntrada == ipHost)
                    {
                        cajaValida = true;
                    }
                }
                else
                {
                    cajaValida = true;
                }
            }
            catch (Exception)
            {
                cajaValida = false;
            }

            return cajaValida;
        }
        #endregion
    }
}