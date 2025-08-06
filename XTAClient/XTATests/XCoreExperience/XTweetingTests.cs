using XTAClient.XTATests.XTATestFoundation;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XCoreExperience;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience.XHomeExperienceModals;
using XTADomain.XTAModels.XCoreExperience;
using XTADomain.XTASharedActions;
using XTAInfras.XExceptions;
using XTAInfras.XTestCircle;

namespace XTAClient.XTATests.XCoreExperience;

#region XTweetingTests > Class level

[Parallelizable(ParallelScope.Children)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[TestFixture]
internal class XTweetingTests : AXTATestFoundation
{
    #region Introduce class vars

    private XUserProfilePage m_xUserProfilePage;
    private XTweetModel m_xTweetModel;
    private XHomePage m_xHomePage;
    
    #endregion Introduce class vars
    
    [Test]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    [XZeta(nameof(m_xZeta_DeleteAllTweetsAsync), nameof(m_xZeta_NavigateToBaseXURLAsync))]
    [XSigma(nameof(m_xSigma_DeleteATweetAsync))]
    public async Task XUITest_NavigateToXHomePage_MakeATextBasedTweet_VerifyTweetSuccessfullyCreatedAsync()
    {
        m_xTweetModel = new()
        {
            TweetContent = XSingletonFactory.s_DaVinci<XRandomUtils>().GenRandomString(12, 170)
        };
        
        await m_xHomePage.ClickOnPostBtnAsync();
        await m_xHomePage.FillPostContentAsync(m_xTweetModel.TweetContent);
        await m_xHomePage.ClickOnPostTweetBtnAsync();

        await m_xHomePage.ClickOnProfileNavAsync();
        
        await m_xUserProfilePage.VerifyAnXPostSuccessfullyCreated(m_xTweetModel);
    }
    
    #region Introduce private methods

    private async Task m_xSigma_DeleteATweetAsync()
    {
        await m_xUserProfilePage.ClickOnDeleteBtnAsync();
        await new XDeletePostModal(p_xPage).ClickOnDeletePostBtnAsync();
    }

    private async Task m_xZeta_DeleteAllTweetsAsync()
    {
        m_xUserProfilePage = new XUserProfilePage(p_xPage);
        m_xHomePage = new XHomePage(p_xPage);
        
        await m_xHomePage.ClickOnProfileNavAsync();
        
        if (await m_xUserProfilePage.CheckWhetherAnyTweet())
            if (!await m_xUserProfilePage.DeleteAllTweetsAsync())
                throw new XTestBusinessFlowException("The process of deleting all existing tweets got unexpected problems. Please have checks!      ");
    }

    private async Task m_xZeta_NavigateToBaseXURLAsync() 
        => await XSingletonFactory
            .s_Retrieve<XTANavigationKit>()
            .NavigateToURLAsync(p_xPage, ps_xAppConfModel.BaseXURL);

    #endregion Introduce private methods
    
    #region Introduce NUnit SetUp phase

    [OneTimeSetUp]
    public static void s_XMetaSetUp() {}
    
    [SetUp]
    public async Task XMegaSetUpAsync() => await p_LogInToXAsync();
    
    #endregion Introduce NUnit SetUp phase
}

#endregion XTweetingTests > Class level

