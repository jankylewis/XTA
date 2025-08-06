using Microsoft.Playwright;
using XTAInfras.XConstHouse;

namespace XTADomain.XTASharedActions;

public class XTAWebUIJSWaitStrategies
{
    public XTAWebUIJSWaitStrategies() {}
    
    internal async Task WaitForJSFuncAsync(
        IPage in_xPage, 
        string in_jsFunc, 
        PageWaitForFunctionOptions? in_pageWaitForFunctionOptions = default
        ) 
            => await in_xPage.WaitForFunctionAsync(
                in_jsFunc, 
                in_pageWaitForFunctionOptions ?? new PageWaitForFunctionOptions 
                { 
                    Timeout = XTimedoutConsts.MAX_ELEMENT_TIMEOUT_MS 
                });
}