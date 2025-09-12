using XTAClient.XTATests.XTATestFoundation;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;
using XTAInfras.XTestCircle;

namespace XTAClient.XTATests.XCoreExperience.XTweetingExperience;

[Parallelizable(ParallelScope.Children)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
internal class XLikeTweets : AXTATestFoundation
{
    #region Introduce class vars

    private XHomePage m_xHomePage;

    #endregion Introduce class vars

    #region Introduce test methods

    [Test]
    [XSigma(nameof(m_xSigma_UnlikeATweetAsync))]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_NavigateToXHomePage_LikeFirstTweet_VerifyFirstTweetSuccessfullyLiked()
    {
        await m_xHomePage.LikeFirstPresentedTweetAsync();
        await m_xHomePage.VerifyIfFirstTweetSuccessfullyLikedAsync();
    }
    
    [Test]
    [XSigma(nameof(m_xSigma_UnlikeAllTweetsAsync))]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_NavigateToXHomePage_LikeANumberOfTweets_VerifyTweetsSuccessfullyLiked()
    {
        int tweetsLiked = XSingletonFactory.s_DaVinci<XRandomUtils>().GenRandomInt(1, 10);

        await m_xHomePage.LikeTweetsAsync(in_numberOfTweets: tweetsLiked);
        await m_xHomePage.VerifyIfTweetsSuccessfullyLikedAsync(in_numberOfTweets: tweetsLiked);
    }

    #endregion Introduce test methods

    #region Introduce private methods

    private async Task m_xSigma_UnlikeATweetAsync() => await m_xHomePage.UnlikeFirstPresentedTweetAsync();

    private async Task m_xSigma_UnlikeAllTweetsAsync() => await m_xHomePage.UnlikeAllTweetsAsync();
    
    #endregion Introduce private methods
    
    #region Introduce SetUp/ TearDown phase

    [SetUp]
    public async Task XMegaSetUpAsync()
    {
        m_xHomePage = new XHomePage(p_xPage);
        
        await p_LogInToXAsync();
    }

    #endregion Introduce SetUp/ TearDown phase
}

