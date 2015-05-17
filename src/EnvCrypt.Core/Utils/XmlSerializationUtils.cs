using System.IO;
using System.Text;
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
        /// <summary>
        /// No BOM in a .NET string.
        /// </summary>
        private readonly Encoding _usedEncoding = new UnicodeEncoding(false, false);


        public string Serialize(T poco)
        {
            var serializer = new XmlSerializer(typeof (T));

            var settings = new XmlWriterSettings
            {
                Encoding = _usedEncoding,
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


        public Encoding GetUsedEncoding()
        {
            return _usedEncoding;
        }
    }
}
