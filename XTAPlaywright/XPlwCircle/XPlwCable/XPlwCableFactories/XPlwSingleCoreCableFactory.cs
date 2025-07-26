using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XExceptions;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;

namespace XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableFactories;

internal class XPlwSingleCoreCableFactory
{
    internal XPlwSingleCoreCableFactory(XPlwConfModel in_xPlwConfModel)
    {
        mr_xPlwConfModel = in_xPlwConfModel;
        m_xPlwSingleCoreCableModel = new();
    }

    private readonly XPlwConfModel mr_xPlwConfModel;
    private XPlwSingleCoreCableModel m_xPlwSingleCoreCableModel;

    public async Task GenPlaywrightAsync()
        => m_xPlwSingleCoreCableModel.XPlaywright = await Playwright.CreateAsync();

    public async Task GenBrowserAsync()
    {
        BrowserTypeLaunchOptions browserTypeLaunchOptions = new()
        {
            Headless = !mr_xPlwConfModel.Headed,
            SlowMo = mr_xPlwConfModel.SlowMo,
            Timeout = mr_xPlwConfModel.Timeout
        };

        IBrowser xBrowser = mr_xPlwConfModel.BrowserType.ToUpper() switch
        {
            nameof(EBrowserType.CHROME)
                => await m_xPlwSingleCoreCableModel.XPlaywright.Chromium.LaunchAsync(browserTypeLaunchOptions),

            nameof(EBrowserType.FIREFOX)
                => await m_xPlwSingleCoreCableModel.XPlaywright.Firefox.LaunchAsync(browserTypeLaunchOptions),
            
            nameof(EBrowserType.WEBKIT)
                => await m_xPlwSingleCoreCableModel.XPlaywright.Webkit.LaunchAsync(browserTypeLaunchOptions),
            
            _ => throw new XBrowserExecutionNotSupported(
                $"We currently do not support the browser type: {mr_xPlwConfModel.BrowserType}")
        };
        
        m_xPlwSingleCoreCableModel.XBrowser = xBrowser;
    }

    internal XPlwSingleCoreCableModel TakeXPlwSingleCoreCableModel() => m_xPlwSingleCoreCableModel;
}