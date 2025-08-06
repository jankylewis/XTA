using XTACore.XTAUtils;
using XTAInfras.XExceptions;
using XTAInfras.XPlwCircle;

namespace XTAInfras.XTestCircle;

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
            nameof(EBrowserType.WEBKIT) => "wk",
    
            _ => throw new XBrowserExecutionNotSupported(
                $"We now are not supporting test execution on the browser type: {in_browserType}, saying sorry and please use a different one!      ")
        };
    
        XTestMetaKey = in_testMetaKey;
        XTestCorrelationID = $"{browserPrefix}_{XSingletonFactory.s_DaVinci<XRandomUtils>().GenGuid()}";
    
        return this;
    }
}