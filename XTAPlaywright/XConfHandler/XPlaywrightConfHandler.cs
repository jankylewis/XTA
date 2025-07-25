using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace XTAPlaywright.XConfHandler;

public class XPlaywrightConfHandler
{
    private const String m_PLAYWRIGHT_CONF_PATH = "XTAClientConfigs/plw_confs.yaml";

    public static XPlaywrightConfModel s_LoadPlaywrightConfModel(string in_playwrightConfPath = m_PLAYWRIGHT_CONF_PATH)
    {
        String plwConfYAMLContent = File.ReadAllText(in_playwrightConfPath);

        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<XPlaywrightConfModel>(plwConfYAMLContent);
    }
}