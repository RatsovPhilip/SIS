using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SIS.WebServer.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToXml(this object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
        public static string ToJson(this object obj)
        {
          return  JsonConvert.SerializeObject(obj);
        }

    }
}
