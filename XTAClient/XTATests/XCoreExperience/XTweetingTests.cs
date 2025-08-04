using XTAClient.XTATests.XTATestFoundation;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XCoreExperience;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;
using XTADomain.XTASharedActions;
using XTAPlaywright.XTestCircle;

namespace XTAClient.XTATests.XCoreExperience;

#region XTweetingTests > Class level

[Parallelizable(ParallelScope.Children)]
[TestFixture]
internal class XTweetingTests : AXTATestFoundation
{
    [Test]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    [XSigma(nameof(_xSigma_RemoveTweet))]
    public async Task XUITest_NavigateToXHomePage_MakeATextBasedTweet_VerifyTweetSuccessfullyCreated()
    {
        XHomePage xHomePage = new(p_xPage);

        await xHomePage.ClickOnPostBtnAsync();
        await xHomePage.FillPostContentAsync(XSingletonFactory.s_DaVinci<XRandomUtils>().GenRandomString(17, 61));
        await xHomePage.ClickOnPostTweetBtnAsync();

        await new XUserProfilePage(p_xPage).VerifyAnXPostSuccessfullyCreated();
    }

    #region Introduce private methods

    private async Task _xSigma_RemoveTweet()
    {
           
    }

    #endregion Introduce private methods
    
    #region Introduce NUnit SetUp phase

    [OneTimeSetUp]
    public static void s_XMetaSetUp() 
        => XSingletonFactory.s_DaVinci<XTANavigationKit>();

    [SetUp]
    public async Task XMegaSetUp()
        => await XSingletonFactory
            .s_Retrieve<XTANavigationKit>()
            .NavigateToURLAsync(p_xPage, ps_xAppConfModel.BaseXURL);
    
    #endregion Introduce NUnit SetUp phase
}

#endregion XTweetingTests > Class level

