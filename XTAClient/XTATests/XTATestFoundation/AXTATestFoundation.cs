using Microsoft.Playwright;
using XTAInfras.XConfFactories.XConfFactories;
using XTAInfras.XConfFactories.XConfModels;
using XTAInfras.XInfrasExceptions;
using XTAInfras.XPlwCircle;
using XTAInfras.XPlwCircle.XPlwAdapter;
using XTAInfras.XPlwCircle.XPlwCable.XPlwCableModels;
using XTAInfras.XTestCircle;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using XTACore.XCoreUtils;
using XTAInfras.XRabbitMQCircle;

namespace XTAClient.XTATests.XTATestFoundation;

internal abstract partial class AXTATestFoundation
{
    #region Introduce foundational vars
    
    private static XWindowsServiceManager ms_xWindowsServiceManager;
    
    private static readonly ConcurrentDictionary<string, (
        XAccountCredModel out_xAccountCredModel, ulong out_xDeliveryTag, IChannel out_xRabbitMQChann
        )> msr_checkedOutXAccountCredModels = new();
    
    private static XRabbitMQManager ms_xRabbitMQManager;
    
    private static XPlwSingleCoreCableModel ms_xPlwSingleCoreCableModel;
    
    private static readonly ConcurrentDictionary<string, XPlwMultiCoreCableModel> msr_xPlwMultiCoreCableModels = new();
    private static readonly ConcurrentDictionary<string, IXTestAdapter> msr_xTestAdapters = new();
    
    private static XPlwAdapterModel ms_xPlwAdapterModel;
    
    private static XPlwEngineer ms_xPlwEngineer;

    private const string m_RABBIT_MQ_SERVICE_NAME = "RabbitMQ";
    
    protected string p_xTestMetaKey => TestContext.CurrentContext.Test.MethodName!;
    
    protected static XAppConfModel ps_xAppConfModel;
    protected static XPlwConfModel ps_xPlaywrightConfModel;
    protected static XAppAccountCredConfModel ps_xAppAccountCredConfModel;
    
    protected async Task<(XAccountCredModel?, ulong, IChannel)> p_TakeIdleXAccountCredAsync() 
        => await ms_xRabbitMQManager.GetIdleXAccountCreds();
    
    protected IPage p_xPage => m_TakeCurrentXPage();
    protected IBrowserContext p_xBrContext => m_TakeCurrentXBrowserContext();
    
    #endregion Introduce foundational vars

    #region Introduce NUnit hooks

    #region Introduce NUnit SetUp phase
    
    public static async Task s_XAlphaSetUpAsync() 
    {
        if (ps_xAppConfModel.XExeMode is EXExeMode.LOCAL)
        {
            ms_xWindowsServiceManager = XSingletonFactory.s_DaVinci(() 
                => new XWindowsServiceManager(m_RABBIT_MQ_SERVICE_NAME));
            
            await ms_xWindowsServiceManager.EnsureServiceIsRunningAsync();
        }
        
        ms_xRabbitMQManager = XSingletonFactory.s_DaVinci<XRabbitMQManager>();
        
        ps_xAppAccountCredConfModel = XAppAccountCredConfFactory.s_LoadXAppAccountCredConfModel();
        
        await ms_xRabbitMQManager
            .PushAllXAccountCredsAsync(ps_xAppAccountCredConfModel.XTestAccounts, new ConnectionFactory
            {
                HostName = ps_xAppConfModel.XExeMode is EXExeMode.LOCAL ? "localhost" : "",
                RequestedHeartbeat = TimeSpan.FromSeconds(30),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                AutomaticRecoveryEnabled = true,
            });
        
        ps_xPlaywrightConfModel = XPlwConfFactory.s_LoadPlwConfModel();
        ps_xAppConfModel = XAppConfFactory.s_LoadXAppConfModel();
        
        ms_xPlwEngineer = XSingletonFactory.s_DaVinci(() => new XPlwEngineer(ps_xPlaywrightConfModel));
        
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
        var (account, deliveryTag, channel) = await ms_xRabbitMQManager.GetIdleXAccountCreds();
        
        if (account == null)
        {
            // This can happen if you run more parallel tests than you have accounts.
            // The test will fail here with a clear message.
            throw new InvalidOperationException("No available test accounts in the RabbitMQ queue.");
        }
        
        msr_checkedOutXAccountCredModels[p_xTestMetaKey] = (account, deliveryTag, channel);
        
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
        if (msr_checkedOutXAccountCredModels.TryRemove(p_xTestMetaKey, out var out_xCheckedOutXAccountCredModoel))
        {
            // 1. Acknowledge the message was successfully processed.
            // This permanently removes it from the queue and closes the channel.
            await ms_xRabbitMQManager.AckMessagesAsync(
                out_xCheckedOutXAccountCredModoel.out_xDeliveryTag, out_xCheckedOutXAccountCredModoel.out_xRabbitMQChann);

            // 2. Publish the account back to the queue for another test to use.
            await ms_xRabbitMQManager.RepublishXAccountToQueueAsync(
                out_xCheckedOutXAccountCredModoel.out_xAccountCredModel, out_xCheckedOutXAccountCredModoel.out_xRabbitMQChann);;
        }
        
        if (msr_xPlwMultiCoreCableModels.TryRemove(p_xTestMetaKey, out var a_xPlwMultiCableModel)
            && msr_xTestAdapters.TryRemove(p_xTestMetaKey, out var a_xTestAdapter))
            
            ms_xPlwAdapterModel = await ms_xPlwEngineer
                .UnplugMultiCoreCableFromXAdapterAsync(a_xPlwMultiCableModel, ms_xPlwAdapterModel, a_xTestAdapter);
    }

    public static async Task s_XAlphaTearDownAsync()
    {
        try
        {
            await ms_DischargeXRabbitMQServerAsync();
        }
        finally
        {
            await ms_xPlwEngineer.PowerDownPlwPowerSourceAsync(ms_xPlwSingleCoreCableModel, ms_xPlwAdapterModel);
            XSingletonFactory.s_DisposeAll();
        }
    }

    #endregion Introduce NUnit TearDown phase
    
    #endregion Introduce NUnit hooks
    
    #region Introduce private services

    private static async Task ms_DischargeXRabbitMQServerAsync()
    {
        await ms_xRabbitMQManager.DeleteAllExistingXRabbitMQMsgsAsync();

        if (ps_xAppConfModel.XExeMode is EXExeMode.LOCAL)
            await ms_xWindowsServiceManager.StopServiceAsync();
            
        await ms_xRabbitMQManager.DisposeXRabbitMQConnectionAsync();
    }
    
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