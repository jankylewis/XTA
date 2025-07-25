using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfFactories;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XPlaywrightCircle;
using XTAPlaywright.XTestCircle;

namespace XTAClient.XTATests.Abstractions;

internal abstract class AXTATestFoundation
{
    #region Introduce foundational vars

    private static XPlaywrightPowerSource m_xPlaywrightPowerSource;
    protected static XPlaywrightAdapter? m_xPlaywrightAdapter;
    
    protected IPage prot_xPage => m_xPlaywrightAdapter.XPages.TryGetValue(m_xTestAdapter.XTestMetaKey, out IPage a_xPage) ? a_xPage :
        throw new InvalidOperationException($"Page not initialized for test method '{m_xTestAdapter.XTestMetaKey}'");

    protected static XAppConfModel prot_xAppConfModel;
    private static XPlwConfModel m_xPlaywrightConfModel;

    private IXTestAdapter m_xTestAdapter;

    private static string m_testMethodKey => TestContext.CurrentContext.Test.MethodName;

    private static IBrowser s_browser;
    
    #endregion Introduce foundational vars

    #region Introduce NUnit hooks

    [OneTimeSetUp]
    public static async Task XAlphaSetUp()
    {
        m_xPlaywrightConfModel = XPlwConfFactory.s_LoadPlaywrightConfModel();
        prot_xAppConfModel = XAppConfFactory.s_LoadXAppConfModel();
        
        m_xPlaywrightPowerSource = new XPlaywrightPowerSource(m_xPlaywrightConfModel);
        //
        // await m_xPlaywrightPowerSource.ProducePlaywrightPowerSource();

        s_browser = await m_xPlaywrightPowerSource.GenBrowserAsync();
    }

    [SetUp]
    public async Task XHyperSetUp()
    {
        m_xTestAdapter = new XTestAdapter()
            .ProduceXTestAdapter(m_testMethodKey, m_xPlaywrightConfModel.BrowserType);

        m_xPlaywrightAdapter ??= new();
        m_xPlaywrightAdapter.XBrowserContexts ??= new();
        m_xPlaywrightAdapter.XPages ??= new();        
        
        var browserContext = await s_browser.NewContextAsync(
            new BrowserNewContextOptions
            {
                ViewportSize = new Microsoft.Playwright.ViewportSize { Width = 1280, Height = 720 }
            });
        
        var page = await browserContext.NewPageAsync();
        
        m_xPlaywrightAdapter.XBrowserContexts[m_xTestAdapter.XTestMetaKey] = browserContext;
        m_xPlaywrightAdapter.XPages[m_xTestAdapter.XTestMetaKey] = page;
        
        // m_xPlaywrightAdapter = await m_xPlaywrightPowerSource.ProduceXPlaywrightAdapter(m_xTestAdapter);
    }

    [TearDown]
    public async Task XHyperTearDown()
    {

    }

    [OneTimeTearDown]
    public static async Task XAlphaTearDown()
    {
        await m_xPlaywrightPowerSource.DisposeAsync();
    }

#endregion Introduce NUnit hooks
}