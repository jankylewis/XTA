using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace XTAPlaywright.XConfHandler;

public class XAppConfHandler
{
    private const String m_X_CONFS_PATH = "XTAClientConfigs/xta_app_confs.yaml";

    public static XAppConfModel s_LoadXAppConfModel(string in_xAppConfPath = m_X_CONFS_PATH)
    {
        String xConfYAMLContent = File.ReadAllText(in_xAppConfPath);
        
        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<XAppConfModel>(xConfYAMLContent);
    }
}

