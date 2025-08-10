using XTAClient.XTATests.XTATestFoundation;
using XTAInfras.XReporting;

[SetUpFixture]
public sealed class XTAPlwBootstrapper
{
    internal static string RunSessionID = string.Empty;
    internal static XTAReportEventPublisher? s_Publisher;

    [OneTimeSetUp]
    public static async Task s_XGlobalBootAsync() 
    {
        RunSessionID = $"{DateTime.UtcNow:yyyy-MM-ddTHH-mm-ssZ}_{Guid.NewGuid():N}";

        s_Publisher = new XTAReportEventPublisher();

        await s_Publisher.PublishRunSessionStartedAsync(RunSessionID);

        await AXTATestFoundation.s_XGlobalBootAsync();
    }
    
    [OneTimeTearDown]
    public static async Task s_XGlobalShutdownAsync() 
    {
        try
        {
            await s_Publisher!.PublishRunSessionCompletedAsync(RunSessionID);
        }
        finally
        {
            if (s_Publisher is not null)
                await s_Publisher.DisposeAsync();
        }
        await AXTATestFoundation.s_XGlobalShutdownAsync();
    }
}