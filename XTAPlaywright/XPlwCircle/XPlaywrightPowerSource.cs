using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XTestCircle;
using ViewportSize = Microsoft.Playwright.ViewportSize;

namespace XTAPlaywright.XPlaywrightCircle;

public class XPlaywrightPowerSource : IAsyncDisposable
{
    #region Introduce constructors
    
    public XPlaywrightPowerSource(XPlwConfModel in_xPlaywrightConfModel) 
        => m_X_PLW_CONF_MODEL = in_xPlaywrightConfModel;

    #endregion Introduce constructors
    
    #region Introduce vars
    
    private readonly XPlwConfModel m_X_PLW_CONF_MODEL;

    private XPlaywrightAdapter m_xPlaywrightAdapter;
    
    #endregion Introduce vars

    #region Inject XPlaywright adapter

    public void InjectXPlaywrightAdapter(XPlaywrightAdapter in_xPlaywrightAdapter) 
        => m_xPlaywrightAdapter = in_xPlaywrightAdapter;

    #endregion Inject XPlaywright adapter

    public async Task<IBrowser> GenBrowserAsync()
    {
        var plw = await Playwright.CreateAsync();
        
        BrowserTypeLaunchOptions browserTypeLaunchOptions = new()
        {
            Headless = !m_X_PLW_CONF_MODEL.Headed,
            SlowMo = m_X_PLW_CONF_MODEL.SlowMo,
            Timeout = m_X_PLW_CONF_MODEL.Timeout,
        };

        IBrowser xBrowser = m_X_PLW_CONF_MODEL.BrowserType.ToUpper() switch
        {
            nameof(EBrowserType.CHROME) 
                => await plw.Chromium.LaunchAsync(browserTypeLaunchOptions),

            _ => throw new Exception($"Unsupport the browser type: {m_X_PLW_CONF_MODEL.BrowserType}")
        };
        
        return xBrowser;
    }
    
    #region Produce Playwright
    
    private async Task _ProducePlaywrightAsync()
        => _SetPlaywrightToXPlaywrightAdapter(await Playwright.CreateAsync());
    
    private void _SetPlaywrightToXPlaywrightAdapter(IPlaywright in_playwright) 
        => m_xPlaywrightAdapter.XPlaywright = in_playwright;

    #endregion Produce Playwright

    #region Produce Browser
    
    private async Task _ProduceBrowserAsync()
    {
        BrowserTypeLaunchOptions browserTypeLaunchOptions = new()
        {
            Headless = !m_X_PLW_CONF_MODEL.Headed,
            SlowMo = m_X_PLW_CONF_MODEL.SlowMo,
            Timeout = m_X_PLW_CONF_MODEL.Timeout,
            
            // Args = new[] { 
            //     "--no-sandbox", 
            //     "--disable-dev-shm-usage",
            //     "--disable-blink-features=AutomationControlled",
            //     "--disable-features=VizDisplayCompositor",
            //     "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            // }
        };

        IBrowser xBrowser = m_X_PLW_CONF_MODEL.BrowserType.ToUpper() switch
        {
            nameof(EBrowserType.CHROME) 
                => await m_xPlaywrightAdapter.XPlaywright.Chromium.LaunchAsync(browserTypeLaunchOptions),

            _ => throw new Exception($"Unsupport the browser type: {m_X_PLW_CONF_MODEL.BrowserType}")
        };
        
        _SetBrowserToXPlaywrightAdapter(xBrowser);
    }

    private void _SetBrowserToXPlaywrightAdapter(IBrowser in_browser)
        => m_xPlaywrightAdapter.XBrowser = in_browser;

    #endregion Produce Browser

    #region Produce Browser Context

    private async Task _GenerateBrowserContextAsync()
    {
        
    }
    
    private async Task _ProduceBrowserContextAsync()
    {
        IBrowserContext xBrowserContext = await m_xPlaywrightAdapter.XBrowser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize
            {
                Width = m_X_PLW_CONF_MODEL.ViewportSize.Width,
                Height = m_X_PLW_CONF_MODEL.ViewportSize.Height,
            },
            // UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
            // ExtraHTTPHeaders = new Dictionary<string, string>
            // {
            //     ["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7",
            //     ["Accept-Language"] = "en-US,en;q=0.9",
            //     ["Accept-Encoding"] = "gzip, deflate, br",
            //     ["Cache-Control"] = "no-cache",
            //     ["Pragma"] = "no-cache",
            //     ["Sec-Ch-Ua"] = "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"",
            //     ["Sec-Ch-Ua-Mobile"] = "?0",
            //     ["Sec-Ch-Ua-Platform"] = "\"Windows\"",
            //     ["Sec-Fetch-Dest"] = "document",
            //     ["Sec-Fetch-Mode"] = "navigate",
            //     ["Sec-Fetch-Site"] = "none",
            //     ["Sec-Fetch-User"] = "?1",
            //     ["Upgrade-Insecure-Requests"] = "1"
            // },
            // JavaScriptEnabled = true,
            // AcceptDownloads = true,
            // IgnoreHTTPSErrors = true
        });
        
        _SetBrowserContextToXPlaywrightAdapter(xBrowserContext);
    }
    
    private void _SetBrowserContextToXPlaywrightAdapter(IBrowserContext in_browserContext)
        => m_xPlaywrightAdapter.XBrowserContext = in_browserContext;

    #endregion Produce Browser Context

    #region Produce Page

    private async Task _ProducePageAsync()
        => _SetPageToXPlaywrightAdapter(await m_xPlaywrightAdapter.XBrowserContext.NewPageAsync());
    
    private void _SetPageToXPlaywrightAdapter(IPage in_page) => m_xPlaywrightAdapter.XPage = in_page;

    #endregion Produce Page
    
    #region Produce XPlaywright circle

    #region Produce XPlaywright power source

    public async Task ProducePlaywrightPowerSource()
    {
        m_xPlaywrightAdapter = new()
        {
            XBrowserContexts = new(),
            XPages = new()
        };
        
        await _ProducePlaywrightAsync();
        await _ProduceBrowserAsync();
    }

    #endregion Produce XPlaywright power source

    #region Produce XPlaywright adapter

    public async Task<XPlaywrightAdapter> ProduceXPlaywrightAdapter(IXTestAdapter in_xTestAdapter)
    {
        await _ProduceBrowserContextAsync();
        await _ProducePageAsync();
        
        return m_xPlaywrightAdapter.InitPlaywrightAdapter(m_xPlaywrightAdapter, in_xTestAdapter);
    }
    
    
    #endregion Produce XPlaywright adapter
    
    #endregion Produce XPlaywright circle
    
    #region Dispose Playwright power source
    
    public async ValueTask DisposeAsync()
    {
        // if (xPlaywrightAdapter.XPage is not null)
        //     await xPlaywrightAdapter.XPage.CloseAsync();
        //
        // if (xPlaywrightAdapter.XBrowserContext is not null)
        //     await xPlaywrightAdapter.XBrowserContext.CloseAsync();
        
        // xPlaywrightAdapter.XPages.Values.ToList().ForEach(async a_xPage => await a_xPage.CloseAsync());
        //
        // xPlaywrightAdapter.XBrowserContexts.Values.ToList().ForEach(async a_xBrowserContext => await a_xBrowserContext.CloseAsync());

        foreach (IPage a_xPage in m_xPlaywrightAdapter.XPages.Values)
        {
            await a_xPage.CloseAsync();
        }
        
        foreach (IBrowserContext a_xBrowserContext in m_xPlaywrightAdapter.XBrowserContexts.Values)
        {
            await a_xBrowserContext.CloseAsync();
        }
        
        if (m_xPlaywrightAdapter.XBrowser is not null)
            await m_xPlaywrightAdapter.XBrowser.CloseAsync();
        
        m_xPlaywrightAdapter.XPlaywright?.Dispose();
    }
    
    #endregion Dispose Playwright power source
}