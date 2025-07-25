using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfFactories;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XExceptions;
using XTAPlaywright.XPlwCircle;
using XTAPlaywright.XPlwCircle.XPlwAdapter;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;
using XTAPlaywright.XTestCircle;

namespace XTAClient.XTATests.XTATestFoundation;

internal abstract partial class AXTATestFoundation
{
    #region Introduce foundational vars
    
    private static XPlwEngineer ms_xPlwEngineer;
    
    private static XPlwSingleCoreCableModel ms_xPlwSingleCoreCableModel;
    private static readonly AsyncLocal<XPlwMultiCoreCableModel> msr_xPlwMultiCoreCableModel = new();
    
    protected static XAppConfModel ps_xAppConfModel;
    private static XPlwConfModel ms_xPlaywrightConfModel;
    
    private string m_testMethodKey => TestContext.CurrentContext.Test.MethodName;
    protected IPage p_xPage => _TakeCurrentXPage();
    protected IBrowserContext p_xBrContext => _TakeCurrentXBrowserContext();
    
    private static readonly AsyncLocal<IXTestAdapter> msr_xTestAdapter = new();
    private static XPlwAdapterModel ms_xPlwAdapterModel;
    
    #endregion Introduce foundational vars

    #region Introduce NUnit hooks

    #region Introduce NUnit SetUp phase
    
    [OneTimeSetUp]
    public static async Task XAlphaSetUp()
    {
        ms_xPlaywrightConfModel = XPlwConfFactory.s_LoadPlaywrightConfModel();
        ps_xAppConfModel = XAppConfFactory.s_LoadXAppConfModel();
        
        ms_xPlwEngineer = new(ms_xPlaywrightConfModel);
        
        ms_xPlwSingleCoreCableModel = await ms_xPlwEngineer.GenXPlwSingleCoreCableModelAsync();
     
        ms_xPlwAdapterModel = new()
        {
            XBrowserContexts = new(),
            XPages = new()
        };
    }

    [SetUp]
    public async Task XHyperSetUp()
    {
        msr_xTestAdapter.Value = new XTestAdapter()
            .ProduceXTestAdapter(
                m_testMethodKey ?? throw new XTestMethodKeyGotEmptyException("Test Method Key might got empty     "), 
                ms_xPlaywrightConfModel.BrowserType
                );

        msr_xPlwMultiCoreCableModel.Value 
            = await ms_xPlwEngineer.GenXPlwMultiCoreCableModelAsync(ms_xPlwSingleCoreCableModel.XBrowser);

        ms_xPlwAdapterModel 
            = ms_xPlwEngineer.PlugXMultiCoreCableIntoXAdapter(msr_xTestAdapter.Value, msr_xPlwMultiCoreCableModel.Value, ms_xPlwAdapterModel);
    }

    #endregion Introduce NUnit SetUp phase
    
    #region Introduce NUnit TearDown phase
    
    [TearDown]
    public async Task XHyperTearDown() {}

    [OneTimeTearDown]
    public static async Task XAlphaTearDown()
        => await ms_xPlwEngineer.PowerDownPlwPowerSourceAsync(ms_xPlwSingleCoreCableModel, ms_xPlwAdapterModel);

    #endregion Introduce NUnit TearDown phase
    
    #endregion Introduce NUnit hooks
    
    #region Introduce private services

    private IPage _TakeCurrentXPage() 
        => ms_xPlwAdapterModel.XPages.TryGetValue(m_testMethodKey, out IPage a_xPage) 
            ? a_xPage 
            : throw new XPageNotInitializedException(
                $"Page not initialized for test method '{m_testMethodKey}'");
    
    private IBrowserContext _TakeCurrentXBrowserContext()
        => ms_xPlwAdapterModel.XBrowserContexts.TryGetValue(m_testMethodKey, out IBrowserContext a_xBrContext)
            ? a_xBrContext
            : throw new XBrowserContextNotInitializedException(
                $"Browser Context not initialized for test method '{m_testMethodKey}'");
    
    #endregion Introduce private services
}