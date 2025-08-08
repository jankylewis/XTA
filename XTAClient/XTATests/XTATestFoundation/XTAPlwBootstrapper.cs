using XTAClient.XTATests.XTATestFoundation;

[SetUpFixture]
public sealed class XTAPlwBootstrapper
{
    [OneTimeSetUp]
    public static async Task s_XGlobalBootAsync() 
        => await AXTATestFoundation.s_XGlobalBootAsync();
    
    [OneTimeTearDown]
    public static async Task s_XGlobalShutdownAsync() 
        => await AXTATestFoundation.s_XGlobalShutdownAsync();
}