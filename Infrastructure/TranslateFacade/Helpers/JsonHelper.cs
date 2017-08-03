using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Repeater.Infrastructure.TranslateFacade.Helpers
{
    public static class JsonHelper
    {
        /// <summary>
        ///     Convert object to JSON
        /// </summary>
        public static string To<T>(T obj)
        {
            string retVal = null;
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                retVal = Encoding.UTF8.GetString(ms.ToArray());
            }

            return retVal;
        }

        /// <summary>
        ///     Convert JSON to object
        /// </summary>
        public static T From<T>(string json)
        {
            var obj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T) serializer.ReadObject(ms);
            }

            return obj;
        }
    }
}