using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfFactories;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XExceptions;
using XTAPlaywright.XPlwCircle;
using XTAPlaywright.XPlwCircle.XPlwAdapter;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;
using XTAPlaywright.XTestCircle;
using System.Collections.Concurrent;
using XTACore.XTAUtils;

namespace XTAClient.XTATests.XTATestFoundation;

internal abstract partial class AXTATestFoundation
{
    #region Introduce foundational vars
    
    protected string p_xTestMetaKey => TestContext.CurrentContext.Test.MethodName!;

    private static XPlwSingleCoreCableModel ms_xPlwSingleCoreCableModel;
    
    private static readonly ConcurrentDictionary<string, XPlwMultiCoreCableModel> msr_xPlwMultiCoreCableModels = new();
    private static readonly ConcurrentDictionary<string, IXTestAdapter> msr_xTestAdapters = new();
    
    private static XPlwAdapterModel ms_xPlwAdapterModel;
    
    private static XPlwEngineer ms_xPlwEngineer;
    
    protected static XAppConfModel ps_xAppConfModel;
    protected static XPlwConfModel ps_xPlaywrightConfModel;
    
    protected IPage p_xPage => m_TakeCurrentXPage();
    protected IBrowserContext p_xBrContext => m_TakeCurrentXBrowserContext();
    
    #endregion Introduce foundational vars

    #region Introduce NUnit hooks

    #region Introduce NUnit SetUp phase
    
    public static async Task s_XAlphaSetUpAsync() 
    {
        ps_xPlaywrightConfModel = XPlwConfFactory.s_LoadPlaywrightConfModel();
        ps_xAppConfModel = XAppConfFactory.s_LoadXAppConfModel();
        
        ms_xPlwEngineer = new(ps_xPlaywrightConfModel);
        
        ms_xPlwSingleCoreCableModel = await ms_xPlwEngineer.GenXPlwSingleCoreCableModelAsync();
     
        ms_xPlwAdapterModel = new()
        {
            XBrowserContexts = new(),
            XPages = new()
        };
    }

    [SetUp]
    public async Task XHyperSetUpAsync()
    {
        IXTestAdapter xTestAdapter = new XTestAdapter()
            .ProduceXTestAdapter(
                p_xTestMetaKey ?? throw new XTestMethodKeyGotEmptyException("Test Method Key might got empty     "),
                ps_xPlaywrightConfModel.BrowserType
            );

        XPlwMultiCoreCableModel xPlwMultiCoreCableModel 
            = await ms_xPlwEngineer.GenXPlwMultiCoreCableModelAsync(ms_xPlwSingleCoreCableModel.XBrowser);
        
        msr_xTestAdapters[p_xTestMetaKey] = xTestAdapter;

        msr_xPlwMultiCoreCableModels[p_xTestMetaKey] = xPlwMultiCoreCableModel;

        ms_xPlwAdapterModel 
            = ms_xPlwEngineer.PlugXMultiCoreCableIntoXAdapter(xTestAdapter, xPlwMultiCoreCableModel, ms_xPlwAdapterModel);
    }

    #endregion Introduce NUnit SetUp phase
    
    #region Introduce NUnit TearDown phase

    [TearDown]
    public async Task XHyperTearDownAsync()
    {
        if (msr_xPlwMultiCoreCableModels.TryRemove(p_xTestMetaKey, out var a_xPlwMultiCableModel)
            && msr_xTestAdapters.TryRemove(p_xTestMetaKey, out var a_xTestAdapter))
        {
            ms_xPlwAdapterModel = await ms_xPlwEngineer
                .UnplugMultiCoreCableFromXAdapter(a_xPlwMultiCableModel, ms_xPlwAdapterModel, a_xTestAdapter);
        }
    }

    public static async Task s_XAlphaTearDownAsync()
    {
        await ms_xPlwEngineer.PowerDownPlwPowerSourceAsync(ms_xPlwSingleCoreCableModel, ms_xPlwAdapterModel);
        XSingletonFactory.s_DisposeAll();
    }

    #endregion Introduce NUnit TearDown phase
    
    #endregion Introduce NUnit hooks
    
    #region Introduce private services

    private IPage m_TakeCurrentXPage() 
        => ms_xPlwAdapterModel.XPages.TryGetValue(p_xTestMetaKey, out IPage a_xPage) 
            ? a_xPage 
            : throw new XPageNotInitializedException(
                $"Page not initialized for test method '{p_xTestMetaKey}'");
    
    private IBrowserContext m_TakeCurrentXBrowserContext()
        => ms_xPlwAdapterModel.XBrowserContexts.TryGetValue(p_xTestMetaKey, out IBrowserContext a_xBrContext)
            ? a_xBrContext
            : throw new XBrowserContextNotInitializedException(
                $"Browser Context not initialized for test method '{p_xTestMetaKey}'");
    
    #endregion Introduce private services
}