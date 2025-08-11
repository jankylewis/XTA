using Microsoft.Playwright;
using XTAInfras.XConfFactories.XConfFactories;
using XTAInfras.XConfFactories.XConfModels;
using XTAInfras.XInfrasExceptions;
using XTAInfras.XPlwCircle;
using XTAInfras.XPlwCircle.XPlwAdapter;
using XTAInfras.XPlwCircle.XPlwCable.XPlwCableModels;
using XTAInfras.XTestCircle;
using System.Collections.Concurrent;
using NUnit.Framework.Interfaces;
using RabbitMQ.Client;
using XTACore.XCoreUtils;
using XTACore.XCoreUtils.XOSUtils;
using XTAInfras.XInfrasUtils;
using XTAInfras.XRabbitMQCircle;
using XTAInfras.XReporting;

namespace XTAClient.XTATests.XTATestFoundation;

#region AXTATestFoundation > Class level

internal abstract partial class AXTATestFoundation
{
    #region Introduce foundational vars

    #region Introduce private-scoped vars

    private static XWindowsServiceManager ms_xRabbitMQServiceManager;
    
    private static XRabbitMQManager ms_xRabbitMQManager;
    
    private static XPlwSingleCoreCableModel ms_xPlwSingleCoreCableModel;
    
    private static readonly ConcurrentDictionary<string, XPlwMultiCoreCableModel> 
        msr_xPlwMultiCoreCableModels = new();
    
    private static readonly ConcurrentDictionary<string, IXTestAdapter> 
        msr_xTestAdapters = new();
    
    private static XPlwAdapterModel ms_xPlwAdapterModel;
    
    private static XPlwEngineer ms_xPlwEngineer;

    private const string m_RABBIT_MQ_SERVICE_NAME = "RabbitMQ";

    private static readonly ConcurrentDictionary<string, (string correlationID, DateTime startedUTC)> 
        msr_caseMeta = new();

    #endregion Introduce private-scoped vars

    #region Introduce protected-scoped vars
    protected string p_xTestMetaKey => TestContext.CurrentContext.Test.MethodName!;

    protected static readonly ConcurrentDictionary<string, (
        XAccountCredModel out_xAccountCredModel, ulong out_xDeliveryTag, IChannel out_xRabbitMQChann
        )> psr_checkedOutXAccountCredCluster = new();
   
    protected static XAppConfModel ps_xAppConfModel;
    protected static XPlwConfModel ps_xPlwConfModel;
    protected static XAppAccountCredConfModel ps_xAppAccountCredConfModel;
    
    protected IPage p_xPage => m_TakeCurrentXPage();
    protected IBrowserContext p_xBrContext => m_TakeCurrentXBrowserContext();

    #endregion Introduce protected-scoped vars
    
    #endregion Introduce foundational vars

    #region Introduce NUnit hooks

    #region Introduce NUnit SetUp phase
    
    public static async Task s_XGlobalBootAsync()
    {
        ms_ResolveXConfModels();
        ms_DaVinciXCriticalServices();
        await ms_PowerUpXRabbitMQServerAsync();
        await ms_PowerUpXPlwPowerSourceAsync();
    }

    [OneTimeSetUp]
    public static async Task s_XAlphaSetUpAsync()
        => await ms_xRabbitMQManager
            .PubAllXAccountCredModelsAsync(ps_xAppAccountCredConfModel.XTestAccounts, new ConnectionFactory
            {
                HostName = ps_xAppConfModel.XExeMode is EXExeMode.LOCAL
                    ? XNetworkingServices.LOOPBACK_ADDRESS
                    : XSingletonFactory.s_Retrieve<XNetworkingServices>()
                        .ResolveXRabbitMQServerIP(),

                RequestedHeartbeat = TimeSpan.FromSeconds(200),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                AutomaticRecoveryEnabled = true
            });

    [SetUp]
    public async Task XHyperSetUpAsync()
    {
        await m_GetAnIdleXAccountCredModelAsync();
        await m_ResolveXPlwCircleAsync();
        await m_PublishTestStartedAsync();
    }

    #endregion Introduce NUnit SetUp phase
    
    #region Introduce NUnit TearDown phase
    
    [TearDown]
    public async Task XHyperTearDownAsync()
    {
        await m_PublishTestCompletedAsync();
        await m_TransitXAccountCredModelAsync();
        await m_UnplugMultiCoreCableFromXAdapterAsync();
    }

    [OneTimeTearDown]
    public static async Task s_XAlphaTearDownAsync()
        => await ms_xRabbitMQManager.PurgeXAccountCredModelQueueAsync();
    
    public static async Task s_XGlobalShutdownAsync()
    {
        try
        {
            await ms_PowerDownXRabbitMQServerAsync();
        }
        finally
        {
            await ms_xPlwEngineer.PowerDownXPlwPowerSourceAsync(ms_xPlwSingleCoreCableModel, ms_xPlwAdapterModel);
            XSingletonFactory.s_DisposeAll();
        }
    }

    #endregion Introduce NUnit TearDown phase
    
    #endregion Introduce NUnit hooks
    
    #region Introduce private services

    #region Introduce private NUnit SetUp services 

    private static void ms_ResolveXConfModels()
    {
        ps_xAppAccountCredConfModel = XAppAccountCredConfFactory.s_LoadXAppAccountCredConfModel();
        ps_xPlwConfModel = XPlwConfFactory.s_LoadPlwConfModel();
        ps_xAppConfModel = XAppConfFactory.s_LoadXAppConfModel();
    }

    private static void ms_DaVinciXCriticalServices()
    {
        XSingletonFactory.s_Register<XNetworkingServices>();
        ms_xRabbitMQManager = XSingletonFactory.s_DaVinci<XRabbitMQManager>();
        ms_xPlwEngineer = XSingletonFactory.s_DaVinci(() => new XPlwEngineer(ps_xPlwConfModel));
    }

    private static async Task ms_PowerUpXPlwPowerSourceAsync()
    {
        ms_xPlwSingleCoreCableModel = await ms_xPlwEngineer.GenXPlwSingleCoreCableModelAsync();
     
        ms_xPlwAdapterModel = new()
        {
            XBrowserContexts = new(),
            XPages = new()
        };
    }
    
    private static async Task ms_PowerUpXRabbitMQServerAsync()
    {
        if (ps_xAppConfModel.XExeMode is EXExeMode.LOCAL)
        {
            ms_xRabbitMQServiceManager = XSingletonFactory.s_DaVinci(() 
                => new XWindowsServiceManager(m_RABBIT_MQ_SERVICE_NAME));
            
            await ms_xRabbitMQServiceManager.EnsureServiceIsRunningAsync();
        }
    }
    
    private async Task m_ResolveXPlwCircleAsync()
    {
        IXTestAdapter xTestAdapter = new XTestAdapter()
            .ProduceXTestAdapter(
                p_xTestMetaKey ?? throw new XTestMethodKeyGotEmptyException("Test Method Key might got empty     "),
                ps_xPlwConfModel.BrowserType
            );

        XPlwMultiCoreCableModel xPlwMultiCoreCableModel 
            = await ms_xPlwEngineer.GenXPlwMultiCoreCableModelAsync(ms_xPlwSingleCoreCableModel.XBrowser);
        
        msr_xTestAdapters[p_xTestMetaKey] = xTestAdapter;

        msr_xPlwMultiCoreCableModels[p_xTestMetaKey] = xPlwMultiCoreCableModel;

        ms_xPlwAdapterModel 
            = ms_xPlwEngineer.PlugXMultiCoreCableIntoXAdapter(xTestAdapter, xPlwMultiCoreCableModel, ms_xPlwAdapterModel);
        // Store correlationID for reporting
        msr_caseMeta[p_xTestMetaKey] = (xTestAdapter.XTestCorrelationID, DateTime.UtcNow);
    }
    
    private async Task m_GetAnIdleXAccountCredModelAsync()
    {
        (XAccountCredModel?, ulong, IChannel) idleXAccountCredModelCluster 
            = await ms_xRabbitMQManager.GetAnIdleXAccountCredModel();

        if (idleXAccountCredModelCluster.Item1 is null)
            throw new XAccountCredModelNotFoundException("There is no avail X Account Cred Model in the queue.      ");
        
        psr_checkedOutXAccountCredCluster[p_xTestMetaKey] = idleXAccountCredModelCluster;
    }

    #endregion Introduce private NUnit SetUp services

    #region Introduce private NUnit TearDown services

    private async Task m_TransitXAccountCredModelAsync()
    {
        if (psr_checkedOutXAccountCredCluster.TryRemove(
                p_xTestMetaKey, 
                out (XAccountCredModel out_xAccountCredModel, ulong out_xDeliveryTag, IChannel out_xRabbitMQChann) out_xCheckedOutXAccountCredCluster))
        {
            await ms_xRabbitMQManager.AckMsgAsync(
                out_xCheckedOutXAccountCredCluster.out_xDeliveryTag, out_xCheckedOutXAccountCredCluster.out_xRabbitMQChann);

            await ms_xRabbitMQManager.RepublishXAccountToQueueAsync(
                out_xCheckedOutXAccountCredCluster.out_xAccountCredModel, out_xCheckedOutXAccountCredCluster.out_xRabbitMQChann);
        }
    }

    private async Task m_PublishTestStartedAsync()
    {
        if (!msr_caseMeta.TryGetValue(p_xTestMetaKey, out var meta)) 
            return;
        
        if (XTAPlwBootstrapper.s_Publisher is null) 
            return;
        
        var ch = XTAPlwBootstrapper.s_Publisher
            .GetType()
            .GetField("m_channel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(XTAPlwBootstrapper.s_Publisher) as IChannel;

        if (ch is null) 
            return;
        
        await XTAReportTestEvents.PublishTestStartedAsync(
            channel: ch,
            runSessionID: XTAPlwBootstrapper.RunSessionID,
            testMethodName: p_xTestMetaKey,
            testClassName: TestContext.CurrentContext.Test.ClassName!,
            testCategories: TestContext.CurrentContext.Test.Properties["Category"]?.Cast<string>().ToArray() ?? Array.Empty<string>(),
            correlationID: meta.correlationID,
            startedUTC: meta.startedUTC,
            ct: default
        );
    }

    private async Task m_PublishTestCompletedAsync()
    {
        if (!msr_caseMeta.TryRemove(p_xTestMetaKey, out var meta)) return;
        if (XTAPlwBootstrapper.s_Publisher is null) return;
        var status = TestContext.CurrentContext.Result.Outcome.Status switch
        {
            TestStatus.Passed => "Passed",
            TestStatus.Skipped => "Skipped",
            _ => "Failed"
        };
        var durationMs = (long)(DateTime.UtcNow - meta.startedUTC).TotalMilliseconds;
        var ch = XTAPlwBootstrapper.s_Publisher
            .GetType()
            .GetField("m_channel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(XTAPlwBootstrapper.s_Publisher) as IChannel;
        if (ch is null) return;

        if (status == "Failed")
        {
            try
            {
                // Emit an error log entry automatically
                var errorMsg = TestContext.CurrentContext.Result.Message ?? "Test failed";
                var stack = TestContext.CurrentContext.Result.StackTrace;
                await XTAReportTestEvents.PublishLogWrittenAsync(
                    ch,
                    XTAPlwBootstrapper.RunSessionID,
                    meta.correlationID,
                    level: "Error",
                    message: stack is null ? errorMsg : $"{errorMsg}\n{stack}",
                    exception: null
                );

                string reportRoot = Environment.GetEnvironmentVariable("XTA_REPORT_ROOT")
                    ?? Path.Combine(AppContext.BaseDirectory, "report");
                string attachDir = Path.Combine(reportRoot, "attachments", meta.correlationID);
                Directory.CreateDirectory(attachDir);
                string fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_failure.png";
                string fullPath = Path.Combine(attachDir, fileName);
                await p_xPage.ScreenshotAsync(new PageScreenshotOptions { Path = fullPath, FullPage = true });
                string relativePath = Path.Combine("attachments", meta.correlationID, fileName);
                await XTAReportTestEvents.PublishStepLoggedAsync(
                    ch,
                    XTAPlwBootstrapper.RunSessionID,
                    meta.correlationID,
                    order: 0,
                    name: "Failure Screenshot",
                    message: TestContext.CurrentContext.Result.Message,
                    status: "Failed",
                    attachmentRelativePath: relativePath,
                    attachmentCaption: "Failure screenshot"
                );
            }
            catch { }
        }
        await XTAReportTestEvents.PublishTestCompletedAsync(
            ch,
            XTAPlwBootstrapper.RunSessionID,
            meta.correlationID,
            status,
            DateTime.UtcNow,
            durationMs
        );
    }

    protected async Task p_LogStepAsync(
        string stepSummary, 
        string? stepMessage = null, 
        bool withScreenshot = false, 
        string status = "Info")
    {
        if (!msr_caseMeta.TryGetValue(p_xTestMetaKey, out var meta)) return;
        if (XTAPlwBootstrapper.s_Publisher is null) return;
        var ch = XTAPlwBootstrapper.s_Publisher
            .GetType()
            .GetField("m_channel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(XTAPlwBootstrapper.s_Publisher) as IChannel;
        if (ch is null) return;

        string? relativePath = null;
        string? caption = null;

        if (withScreenshot)
        {
            string reportRoot = Environment.GetEnvironmentVariable("XTA_REPORT_ROOT")
                ?? Path.Combine(AppContext.BaseDirectory, "report");
            string attachDir = Path.Combine(reportRoot, "attachments", meta.correlationID);
            Directory.CreateDirectory(attachDir);
            string fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{stepSummary}.png";
            string fullPath = Path.Combine(attachDir, fileName);
            await p_xPage.ScreenshotAsync(new PageScreenshotOptions { Path = fullPath, FullPage = true });
            relativePath = Path.Combine("attachments", meta.correlationID, fileName);
            caption = stepSummary;
        }

        await XTAReportTestEvents.PublishStepLoggedAsync(
            ch,
            XTAPlwBootstrapper.RunSessionID,
            meta.correlationID,
            order: 0,
            stepSummary,
            stepMessage,
            status,
            attachmentRelativePath: relativePath,
            attachmentCaption: caption
        );
    }

    protected async Task p_LogInfoAsync(string message) => await m_LogAsync("Info", message, null);
    protected async Task p_LogErrorAsync(string message, Exception? ex = null) => await m_LogAsync("Error", message, ex);

    private async Task m_LogAsync(string logLevel, string message, Exception? ex)
    {
        if (!msr_caseMeta.TryGetValue(p_xTestMetaKey, out var meta)) return;
        if (XTAPlwBootstrapper.s_Publisher is null) return;
        var ch = XTAPlwBootstrapper.s_Publisher
            .GetType()
            .GetField("m_channel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(XTAPlwBootstrapper.s_Publisher) as IChannel;
        if (ch is null) return;

        await XTAReportTestEvents.PublishLogWrittenAsync(
            ch,
            XTAPlwBootstrapper.RunSessionID,
            meta.correlationID,
            logLevel,
            message,
            ex?.ToString()
        );
    }

    private async Task m_UnplugMultiCoreCableFromXAdapterAsync()
    {
        if (msr_xPlwMultiCoreCableModels.TryRemove(p_xTestMetaKey, out XPlwMultiCoreCableModel out_xPlwMultiCableModel)
            && msr_xTestAdapters.TryRemove(p_xTestMetaKey, out IXTestAdapter out_xTestAdapter))
            
            ms_xPlwAdapterModel = await ms_xPlwEngineer
                .UnplugMultiCoreCableFromXAdapterAsync(out_xPlwMultiCableModel, ms_xPlwAdapterModel, out_xTestAdapter);
    }

    #endregion Introduce private NUnit TearDown services
    
    private static async Task ms_PowerDownXRabbitMQServerAsync()
    {
        await ms_xRabbitMQManager.DeleteXAccountCredModelQueueAsync();
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

#endregion AXTATestFoundation > Class level
