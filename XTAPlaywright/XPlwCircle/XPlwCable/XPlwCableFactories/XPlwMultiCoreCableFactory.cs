using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;
using ViewportSize = Microsoft.Playwright.ViewportSize;

namespace XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableFactories;

internal class XPlwMultiCoreCableFactory
{
    internal XPlwMultiCoreCableFactory(XPlwConfModel in_xPlwConfModel)
    {
        m_X_PLW_CONF_MODEL = in_xPlwConfModel;
        m_xPlwMultiCoreCableModel = new();
    }
    
    private readonly XPlwConfModel m_X_PLW_CONF_MODEL;

    private XPlwMultiCoreCableModel m_xPlwMultiCoreCableModel;

    public async Task GenBrowserContextAsync(IBrowser in_xBrowser)
        => m_xPlwMultiCoreCableModel.XBrowserContext = await in_xBrowser.NewContextAsync(
            new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = m_X_PLW_CONF_MODEL.ViewportSize.Width,
                    Height = m_X_PLW_CONF_MODEL.ViewportSize.Height
                }
            });

    public async Task GenPageAsync() 
        => m_xPlwMultiCoreCableModel.XPage = await m_xPlwMultiCoreCableModel.XBrowserContext.NewPageAsync();
    
    internal XPlwMultiCoreCableModel TakeXPlwMultiCoreCableModel() => m_xPlwMultiCoreCableModel;
}