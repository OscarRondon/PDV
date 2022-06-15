using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Comunes
{
    public enum CacheItemAbsoluteExpirationEnum
    {
        Segundos,
        Minutos,
        Horas,
        Dias,
        Meses,
        Agnos
    }

    public class CacheItemHelper
    {
        private static ObjectCache _cache;
        /// <summary>
        /// Elimina el item del cache
        /// </summary>
        /// <param name="key">Nombre del cache</param>
        public static void Limpiar(string key)
        {
            if (_cache == null)
            {
                _cache = MemoryCache.Default;
                return;
            }
            _cache.Remove(key);
        }
        /// <summary>
        /// Inserta un item en la cache
        /// </summary>
        /// <typeparam name="T">tipo del item</typeparam>
        /// <param name="entity">item cached</param>
        /// <param name="key">Nombre item</param>
        /// <param name="cacheItemAbsoluteExpirationEnum">Enum de CacheItemAbsoluteExpirationEnum</param>
        /// <param name="cacheExpiration">Numero segundos, minutos, horas, dias, meses, años</param>
        public static void Agregar<T>(T entity, string key, CacheItemAbsoluteExpirationEnum cacheItemAbsoluteExpirationEnum, int cacheExpiration) where T : class
        {
            if (_cache == null)
            {
                _cache = MemoryCache.Default;
            }
            if (_cache.Contains(key))
                _cache.Remove(key);
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (cacheItemAbsoluteExpirationEnum == CacheItemAbsoluteExpirationEnum.Dias)
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddDays(cacheExpiration);
            else if (cacheItemAbsoluteExpirationEnum == CacheItemAbsoluteExpirationEnum.Horas)
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(cacheExpiration);
            else if (cacheItemAbsoluteExpirationEnum == CacheItemAbsoluteExpirationEnum.Minutos)
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(cacheExpiration);
            else if (cacheItemAbsoluteExpirationEnum == CacheItemAbsoluteExpirationEnum.Meses)
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMonths(cacheExpiration);
            else if (cacheItemAbsoluteExpirationEnum == CacheItemAbsoluteExpirationEnum.Segundos)
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddSeconds(cacheExpiration);
            else if (cacheItemAbsoluteExpirationEnum == CacheItemAbsoluteExpirationEnum.Agnos)
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddYears(cacheExpiration);
            _cache.Set(key, entity, cacheItemPolicy);
        }
        /// <summary>
        /// Obtiene el item del cache
        /// </summary>
        /// <typeparam name="T">Tipo del item</typeparam>
        /// <param name="key">Nombre del item</param>
        /// <returns>Retorna el tipo</returns>
        public static T Obtener<T>(string key) where T : class
        {
            if (_cache == null)
            {
                _cache = MemoryCache.Default;
            }
            try
            {
                return (T)_cache.Get(key);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
