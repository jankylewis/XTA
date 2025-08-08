using Microsoft.Playwright;
using XTAInfras.XPlwCircle.XPlwAdapter;
using XTAInfras.XPlwCircle.XPlwCable.XPlwCableModels;

namespace XTAInfras.XPlwCircle;

public class XPlwPowerSource(
    XPlwSingleCoreCableModel in_xPlwSingleCoreCableModel, XPlwAdapterModel in_xPlwAdapterModel) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        foreach (IPage l_xPage in in_xPlwAdapterModel.XPages.Values)
            await l_xPage.CloseAsync();
        
        foreach (IBrowserContext l_xBrowserContext in in_xPlwAdapterModel.XBrowserContexts.Values)
            await l_xBrowserContext.CloseAsync();
        
        if (in_xPlwSingleCoreCableModel.XBrowser is not null)
            await in_xPlwSingleCoreCableModel.XBrowser.CloseAsync();
        
        in_xPlwSingleCoreCableModel.XPlaywright.Dispose();
    }
}