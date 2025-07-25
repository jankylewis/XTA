using System.Collections.Concurrent;
using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfFactories;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XPlaywrightCircle;
using XTAPlaywright.XPlwCircle;
using XTAPlaywright.XPlwCircle.XPlwAdapter;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;
using XTAPlaywright.XTestCircle;

namespace XTAClient._XTATests;

internal class AXTATestFoundation
{
    private static XPlwEngineer m_s_xPlwEngineer;
    private static XPlwSingleCoreCableModel m_s_xPlwSingleCoreCableModel;
    private static readonly AsyncLocal<XPlwMultiCoreCableModel> m_xPlwMultiCoreCableModel = new();
    
    protected static XAppConfModel prot_xAppConfModel;
    private static XPlwConfModel m_xPlaywrightConfModel;   
    
    private static string m_testMethodKey => TestContext.CurrentContext.Test.MethodName;

    private static IPlaywright s_plw;
    private static IBrowser s_br;

    private static bool m_headless = true;
    
    private static readonly ConcurrentDictionary<string, IBrowserContext> s_brContexts = new();
    private static readonly ConcurrentDictionary<string, IPage> s_pages = new();
    
    // protected IPage prot_xPage => m_s_xPlaywrightAdapter.XPages.TryGetValue(m_testMethodKey, out IPage a_xPage) 
    //     ? a_xPage 
    //     : throw new InvalidOperationException($"Page not initialized for test method '{m_testMethodKey}'");
    //
    // private IBrowserContext prot_xBrContext => m_s_xPlaywrightAdapter.XBrowserContexts.TryGetValue(m_testMethodKey, out IBrowserContext a_xBrContext) 
    //     ? a_xBrContext
    //     : throw new InvalidOperationException($"Browser Context not initialized for test method '{m_testMethodKey}'");
    
    protected IPage prot_xPage => m_s_xPlwAdapterModel.XPages.TryGetValue(m_testMethodKey, out IPage a_xPage) 
        ? a_xPage 
        : throw new InvalidOperationException($"Page not initialized for test method '{m_testMethodKey}'");
    
    private IBrowserContext prot_xBrContext => m_s_xPlwAdapterModel.XBrowserContexts.TryGetValue(m_testMethodKey, out IBrowserContext a_xBrContext) 
        ? a_xBrContext
        : throw new InvalidOperationException($"Browser Context not initialized for test method '{m_testMethodKey}'");
    
    private static XPlaywrightPowerSource m_s_xPlaywrightPowerSource;
    private static XPlaywrightAdapter m_s_xPlaywrightAdapter;
    private static readonly AsyncLocal<IXTestAdapter> m_xTestAdapter;

    private static XPlwAdapterModel m_s_xPlwAdapterModel;
    
    [OneTimeSetUp]
    public static async Task XAlphaSetUp()
    {
        // m_xPlaywrightConfModel = XPlwConfFactory.s_LoadPlaywrightConfModel();   
        // m_s_xPlaywrightPowerSource = new XPlaywrightPowerSource(m_xPlaywrightConfModel);
        //
        //
        // prot_xAppConfModel = XAppConfFactory.s_LoadXAppConfModel();
        //
        // s_br = await m_s_xPlaywrightPowerSource.GenBrowserAsync();
        //
        // m_s_xPlaywrightAdapter = new ()
        // {
        //     XBrowserContexts = new ConcurrentDictionary<string, IBrowserContext>(),
        //     XPages = new ConcurrentDictionary<string, IPage>()
        // };
        
        m_xPlaywrightConfModel = XPlwConfFactory.s_LoadPlaywrightConfModel();   
        m_s_xPlwEngineer = new(m_xPlaywrightConfModel);
        prot_xAppConfModel = XAppConfFactory.s_LoadXAppConfModel();

        m_s_xPlwSingleCoreCableModel = await m_s_xPlwEngineer.GenXPlwSingleCoreCableModelAsync();
        
        // m_s_xPlaywrightAdapter = new ()
        // {
        //     XBrowserContexts = new ConcurrentDictionary<string, IBrowserContext>(),
        //     XPages = new ConcurrentDictionary<string, IPage>()
        // };

        m_s_xPlwAdapterModel = new()
        {
            XBrowserContexts = new(),
            XPages = new()
        };
    }

    [SetUp]
    public async Task XHyperSetUp()
    {
        // m_xTestAdapter = new XTestAdapter();
        //
        // m_xTestAdapter = ((XTestAdapter) m_xTestAdapter)
        //     .ProduceXTestAdapter(m_testMethodKey, m_xPlaywrightConfModel.BrowserType);
        //
        // var brContext = await s_br.NewContextAsync(
        //     new BrowserNewContextOptions
        //     {
        //         ViewportSize = new Microsoft.Playwright.ViewportSize { Width = 1280, Height = 720 }
        //     });
        //
        // var page = await brContext.NewPageAsync();
        //
        // m_s_xPlaywrightAdapter.XBrowserContexts[m_testMethodKey] = brContext;
        // m_s_xPlaywrightAdapter.XPages[m_testMethodKey] = page;
        
        m_xTestAdapter = new XTestAdapter()
            .ProduceXTestAdapter(m_testMethodKey, m_xPlaywrightConfModel.BrowserType);

        m_xPlwMultiCoreCableModel.Value = await m_s_xPlwEngineer.GenXPlwMultiCoreCableModelAsync(m_s_xPlwSingleCoreCableModel.XBrowser);

        m_s_xPlwAdapterModel = m_s_xPlwEngineer.PlugXMultiCoreCableIntoXAdapter(m_xTestAdapter, m_xPlwMultiCoreCableModel.Value, m_s_xPlwAdapterModel);

        // m_s_xPlwAdapterModel.XBrowserContexts[m_testMethodKey] = m_xPlwMultiCoreCableModel.Value.XBrowserContext;
        // m_s_xPlwAdapterModel.XPages[m_testMethodKey] = m_xPlwMultiCoreCableModel.Value.XPage;
        
        // m_s_xPlaywrightAdapter.XBrowserContexts[m_testMethodKey] = m_xPlwMultiCoreCableModel.Value.XBrowserContext;
        // m_s_xPlaywrightAdapter.XPages[m_testMethodKey] = m_xPlwMultiCoreCableModel.Value.XPage;
    }

    [TearDown]
    public async Task XHyperTearDown()
    {
        if (s_pages.TryRemove(m_testMethodKey, out var a_xPage))
            await a_xPage.CloseAsync();
        
        if (s_brContexts.TryRemove(m_testMethodKey, out var a_xBrContext))
            await a_xBrContext.CloseAsync();
    }

    [OneTimeTearDown]
    public static async Task XAlphaTearDown()
    {
        foreach (var v in s_pages.Values)
        {
            await v.CloseAsync();
        }
        
        s_pages.Clear();
        
        foreach (var v in s_brContexts.Values)
        {
            await v.CloseAsync();
        }
        
        s_brContexts.Clear();
        
        if (s_br is not null)
            await s_br.CloseAsync();
        
        // s_plw.Dispose();

        await m_s_xPlaywrightPowerSource.DisposeAsync();
    }
}