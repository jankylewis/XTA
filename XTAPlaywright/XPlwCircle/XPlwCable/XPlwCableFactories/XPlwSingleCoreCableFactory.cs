using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XPlaywrightCircle;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;

namespace XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableFactories;

internal class XPlwSingleCoreCableFactory
{
    internal XPlwSingleCoreCableFactory(XPlwConfModel in_xPlwConfModel)
    {
        m_X_PLW_CONF_MODEL = in_xPlwConfModel;
        m_xPlwSingleCoreCableModel = new();
    }

    private readonly XPlwConfModel m_X_PLW_CONF_MODEL;

    private XPlwSingleCoreCableModel m_xPlwSingleCoreCableModel;

    public async Task GenPlaywrightAsync()
        => m_xPlwSingleCoreCableModel.XPlaywright = await Playwright.CreateAsync();

    public async Task GenBrowserAsync()
    {
        BrowserTypeLaunchOptions browserTypeLaunchOptions = new()
        {
            Headless = !m_X_PLW_CONF_MODEL.Headed,
            SlowMo = m_X_PLW_CONF_MODEL.SlowMo,
            Timeout = m_X_PLW_CONF_MODEL.Timeout
        };

        IBrowser xBrowser = m_X_PLW_CONF_MODEL.BrowserType.ToUpper() switch
        {
            nameof(EBrowserType.CHROME)
                => await m_xPlwSingleCoreCableModel.XPlaywright.Chromium.LaunchAsync(browserTypeLaunchOptions),

            _ => throw new Exception($"We currently do not support the browser type: {m_X_PLW_CONF_MODEL.BrowserType}")
        };
        
        m_xPlwSingleCoreCableModel.XBrowser = xBrowser;
    }

    internal XPlwSingleCoreCableModel TakeXPlwSingleCoreCableModel() => m_xPlwSingleCoreCableModel;
}