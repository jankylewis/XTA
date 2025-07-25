using XTAPlaywright.XConfFactories.XConfModels;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace XTAPlaywright.XConfFactories.XConfFactories;

public static class XPlwConfFactory
{
    private const String m_PLAYWRIGHT_CONF_PATH = "XTAClientConfigs/plw_confs.yaml";

    public static XPlwConfModel s_LoadPlaywrightConfModel(string in_playwrightConfPath = m_PLAYWRIGHT_CONF_PATH)
    {
        String plwConfYAMLContent = File.ReadAllText(in_playwrightConfPath);

        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<XPlwConfModel>(plwConfYAMLContent);
    }
}