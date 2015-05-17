using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EnvCrypt.Core.Utils
{
    /// <summary>
    /// (De)Serialization utility for XML.
    /// Code taken from http://stackoverflow.com/questions/1564718/using-stringwriter-for-xml-serialization
    /// </summary>
    internal class XmlSerializationUtils<T> : IXmlSerializationUtils<T> where T : class
    {
        public string Serialize(T poco)
        {
            var serializer = new XmlSerializer(typeof (T));

            var settings = new XmlWriterSettings
            {
                Encoding = new UnicodeEncoding(false, false), // no BOM in a .NET string
                Indent = true,
                OmitXmlDeclaration = false
            };

            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, poco);
                }
                return textWriter.ToString();
            }
        }


        public T Deserialize(string xml)
        {
            var serializer = new XmlSerializer(typeof (T));

            var settings = new XmlReaderSettings();

            using (var textReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T) serializer.Deserialize(xmlReader);
                }
            }
        }
    }
}
