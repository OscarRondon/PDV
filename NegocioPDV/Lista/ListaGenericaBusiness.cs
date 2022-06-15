using Comunes;
using DatosPDV.AccesoDatos;
using EntidadesPDV.PuntoVenta;
using EntidadesPDV;
using System;
using System.Collections.Generic;
using System.Linq;
using EntidadesPDV.Usuario;

namespace NegocioPDV.Lista
{
    public enum ListaDatosEnum
    {
        Funcionalidad,
        Funcion,
        Seccion,
        Piso,
        Banco,
        Tarjeta,
        MedioDePago,
        Comuna,
        MotivoAnulacion,
        TipoNominaAdmisionista,
        TipoNominaSupervisor,
        TipoDocumentoDetalle,
        EstadoNominaDetalle,
        EstadoAsignacionCaja,
        EstadoGarantia,
        TipoGarantia
    }

    public class ListaGenericaBusiness
    {
        NoObjectDatos datos = new NoObjectDatos();

        public ListaGenericaBusiness()
        {

        }
        public ListaGenericaBusiness(UsuarioDV Info)
        {
            datos.UsuarioActual = Info;
        }
        private object ObtenerListaGenerica(ListaDatosEnum enumListaDatos)
        {
            if (!this.ExisteCache())
            {
                DatosPDV.SRNoObject.NoObjectListType result = datos.GetListas();

                if (result != null)
                {
                    this.ResetearCache();
                    this.GuardaEnCache(result);
                    if (!this.ExisteCache())
                    {
                        throw new Exception("No se pudieron cargar los listados");
                    }
                }
                else
                {
                    throw new Exception("No se pudieron cargar los listados.");
                }
            }
            return Obtener(enumListaDatos);
        }

        private bool ExisteCache()
        {
            var result = CacheItemHelper.Obtener<ItemList[]>(ListaDatosEnum.Tarjeta.ToString());
            if (result == null) return false;
            return true;
        }
        private void ResetearCache()
        {
            Enum.GetNames(typeof(ListaDatosEnum)).ToList().ForEach(enumerador =>
            {
                CacheItemHelper.Limpiar(enumerador);

            });
        }
        private void GuardaEnCache(DatosPDV.SRNoObject.NoObjectListType result)
        {
            int diasCache = 1;

            ItemList[] bancos = result.Bancos.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] funcionalidades = result.Funcionalidades.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] funciones = result.Funciones.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            MediosPagoEnt[] metodosDePago = result.MetodosDePago.Select(i => new MediosPagoEnt() { IdMedioPago = i.Code, MedioPago = i.Descripcion, Cheque = i.Cheque, Efectivo = i.Efectivo, Tarjeta = i.Tarjeta, Transferencia = i.Transferencia }).OrderBy(e => e.MedioPago).ToArray();
            ItemList[] pisos = result.Pisos.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] secciones = result.Secciones.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] tarjetas = result.Tarjetas.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] comunas = result.Comunas.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] motivoAnulacion = result.MotivoAnulacion.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] tipoNominaAdmisionista = result.TipoNominaAdmisionista.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] tipoNominaSupervisor = result.TipoNominaSupervisor.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] tipoDocumentoDetalle = result.TipoDocumentoDetalle.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] estadoNominaDetalle = result.EstadoDetalleNomina.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] estadoAsignacionCaja = result.EstadoAsignacionCaja.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] estadoGarantia = result.EstadoGarantia.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();
            ItemList[] tipoGarantia = result.TipoGarantia.Select(i => new ItemList() { Codigo = i.Code, Descripcion = i.Name }).OrderBy(e => e.Descripcion).ToArray();


            #region Listas WebServices SAP
            CacheItemHelper.Agregar(bancos, ListaDatosEnum.Banco.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(funcionalidades, ListaDatosEnum.Funcionalidad.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(funciones, ListaDatosEnum.Funcion.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(metodosDePago, ListaDatosEnum.MedioDePago.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(pisos, ListaDatosEnum.Piso.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(secciones, ListaDatosEnum.Seccion.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(tarjetas, ListaDatosEnum.Tarjeta.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(comunas, ListaDatosEnum.Comuna.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(motivoAnulacion, ListaDatosEnum.MotivoAnulacion.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(tipoNominaAdmisionista, ListaDatosEnum.TipoNominaAdmisionista.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(tipoNominaSupervisor, ListaDatosEnum.TipoNominaSupervisor.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(tipoDocumentoDetalle, ListaDatosEnum.TipoDocumentoDetalle.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(estadoNominaDetalle, ListaDatosEnum.EstadoNominaDetalle.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(estadoAsignacionCaja, ListaDatosEnum.EstadoAsignacionCaja.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(estadoGarantia, ListaDatosEnum.EstadoGarantia.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            CacheItemHelper.Agregar(tipoGarantia, ListaDatosEnum.TipoGarantia.ToString(), CacheItemAbsoluteExpirationEnum.Dias, diasCache);
            #endregion
        }
        private object Obtener(ListaDatosEnum enumListaDatos)
        {
            object result = new object();
            #region Obtencion de datos directos de la Cache
            switch (enumListaDatos)
            {
                case ListaDatosEnum.MedioDePago:
                    result = CacheItemHelper.Obtener<MediosPagoEnt[]>(enumListaDatos.ToString()).ToList();
                    break;
                case ListaDatosEnum.Funcionalidad:
                case ListaDatosEnum.Funcion:
                case ListaDatosEnum.Seccion:
                case ListaDatosEnum.Piso:
                case ListaDatosEnum.Banco:
                case ListaDatosEnum.Tarjeta:
                case ListaDatosEnum.Comuna:
                case ListaDatosEnum.MotivoAnulacion:
                case ListaDatosEnum.TipoNominaAdmisionista:
                case ListaDatosEnum.TipoNominaSupervisor:
                case ListaDatosEnum.TipoDocumentoDetalle:
                case ListaDatosEnum.EstadoAsignacionCaja:
                case ListaDatosEnum.EstadoNominaDetalle:
                case ListaDatosEnum.EstadoGarantia:
                case ListaDatosEnum.TipoGarantia:
                    result = CacheItemHelper.Obtener<ItemList[]>(enumListaDatos.ToString()).ToList();
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
            #endregion
        }

        public List<T> ObtenerListado<T>(ListaDatosEnum enumListaDatos)
        where T : class, new()
        {
            List<T> lstResultado = (List<T>)this.ObtenerListaGenerica(enumListaDatos);

            return lstResultado;
        }
    }
}
