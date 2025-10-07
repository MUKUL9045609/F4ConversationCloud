using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace F4ConversationCloud.Domain.Extension
{
    public static class GenericExtensions
    {
        public static string SerializeToXml<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.Unicode; // Specify UTF-16 encoding
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    xmlSerializer.Serialize(xmlWriter, obj);
                    return textWriter.ToString();
                }
            }
        }
    }
}
