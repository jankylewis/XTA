using XTAClient.XTATests.XTATestFoundation;

[SetUpFixture]
public sealed class XTAPlwBootstrapper
{
    [OneTimeSetUp]
    public static async Task s_XGlobalBoot() 
        => await AXTATestFoundation.s_XAlphaSetUp();
    
    [OneTimeTearDown]
    public static async Task s_XGlobalShutdown() 
        => await AXTATestFoundation.s_XAlphaTearDown();
}