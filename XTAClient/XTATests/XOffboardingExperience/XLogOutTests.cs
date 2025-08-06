using XTAClient.XTATests.XTATestFoundation;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;
using XTADomain.XTABusinesses.XOffboardingExperience.XOffboardingExperienceModals;
using XTADomain.XTABusinesses.XOnboardingExperience;
using XTAPlaywright.XTestCircle;

namespace XTAClient.XTATests.XOffboardingExperience;

[Parallelizable(ParallelScope.Children)]
[FixtureLifeCycle(LifeCycle.SingleInstance)]
[TestFixture]
internal class XLogOutTests : AXTATestFoundation
{
    [Test]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_LoggedInToX_ClickOnLogOutButton_VerifySuccessfullyLogOutAsync()
    {
        await new XHomePage(p_xPage).DoLogOutOfXAsync();
        await new XLogOutOfXModal(p_xPage).ClickOnLogOutBtnAsync();
        await new XLogInPage(p_xPage).VerifyWhetherXLogInPagePresentedAsync();
    }
    
    #region Introduce NUnit SetUp phase

    [SetUp]
    public async Task XMegaSetUpAsync() => await p_LogInToXAsync();

    #endregion Introduce NUnit SetUp phase
}