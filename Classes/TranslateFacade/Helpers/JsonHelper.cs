using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Repeater.Classes.TranslateFacade
{
    public static class JsonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string To<T>(T obj)
        {
            string retVal = null;
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                retVal = Encoding.UTF8.GetString(ms.ToArray());
            }

            return retVal;
        }

        /// <summary>
        /// Конвертирует JSON в объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T From<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
            }

            return obj;
        }
    }
}
