using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace EnvCrypt.InteractiveDecrypt
{
    public abstract class SectionHandlerBase<TConfig> : IConfigurationSectionHandler
        where TConfig : class
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var xmlSerializer = new XmlSerializer(typeof (TConfig));
            var config = xmlSerializer.Deserialize(new XmlNodeReader(section));
            if (config != null)
            {
                var typedConfig = config as TConfig;
                if (typedConfig == null)
                {
                    throw new ConfigurationErrorsException("deserialized type does not match the expected type of " + typeof(TConfig).Name);
                }
                return config;
            }
        }
    }
}
