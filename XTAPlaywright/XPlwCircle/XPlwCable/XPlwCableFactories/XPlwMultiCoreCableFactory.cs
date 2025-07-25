using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;
using ViewportSize = Microsoft.Playwright.ViewportSize;

namespace XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableFactories;

internal class XPlwMultiCoreCableFactory
{
    internal XPlwMultiCoreCableFactory(XPlwConfModel in_xPlwConfModel)
    {
        mr_xPlwConfModel = in_xPlwConfModel;
        m_xPlwMultiCoreCableModel = new();
    }
    
    private readonly XPlwConfModel mr_xPlwConfModel;
    private XPlwMultiCoreCableModel m_xPlwMultiCoreCableModel;

    public async Task GenBrowserContextAsync(IBrowser in_xBrowser)
        => m_xPlwMultiCoreCableModel.XBrowserContext = await in_xBrowser.NewContextAsync(
            new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = mr_xPlwConfModel.ViewportSize.Width,
                    Height = mr_xPlwConfModel.ViewportSize.Height
                }
            });

    public async Task GenPageAsync() 
        => m_xPlwMultiCoreCableModel.XPage = await m_xPlwMultiCoreCableModel.XBrowserContext.NewPageAsync();
    
    internal XPlwMultiCoreCableModel TakeXPlwMultiCoreCableModel() => m_xPlwMultiCoreCableModel;
}