using XTACore.XTAUtils;
using XTAPlaywright.XPlwCircle;

namespace XTAPlaywright.XTestCircle;

public class XTestAdapter : IXTestAdapter
{
    public string XTestMetaKey { get; set; }
    public string XTestCorrelationID { get; set; }
    
    public IXTestAdapter ProduceXTestAdapter(string in_testMetaKey, string in_browserType)
    {
        string browserPrefix = in_browserType.ToUpper() switch
        {
            nameof(EBrowserType.CHROME) => "chr",
            nameof(EBrowserType.FIREFOX) => "ff",
    
            _ => throw new Exception($"$Unsupport the browser type {in_browserType}")
        };
    
        XTestMetaKey = in_testMetaKey;
        XTestCorrelationID = $"{browserPrefix}_{new XRandomUtils().GenGuid()}";
    
        return this;
    }
}