using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XCoreExperience.XHomExperience;

namespace XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;

public class XHomePage(IPage in_xPage) : AXPage<XHomePOs>(in_xPage, XSingletonFactory.s_DaVinci<XHomePOs>())
{
    #region Introduce actions

    public async Task ClickOnProfileNavAsync() 
        => await ClickOnSharedNavAsync(pr_xPOs.r_profileNav);

    public async Task LikeFirstPresentedTweetAsync()
    {
        List<ILocator> likeTweetButtons = await m_GetListOfLikeTweetButtonsAsync();
        await pr_xtaWebUISharedActions.ClickAsync(likeTweetButtons.First());
    }

    public async Task LikeTweetsAsync(int in_numberOfTweets)
    {
        List<ILocator> likeTweetButtons = await m_GetListOfLikeTweetButtonsAsync();
        
        for (int k = 0; k < in_numberOfTweets; k++)
            await pr_xtaWebUISharedActions.ClickAsync(likeTweetButtons.ElementAt(k));
    }

    public async Task UnlikeFirstPresentedTweetAsync()
    {
        List<ILocator> unlikeTweetButtons = await m_GetListOfLikeTweetButtonsAsync();
        await pr_xtaWebUISharedActions.ClickAsync(unlikeTweetButtons.First());

        await pr_xtaWebUISharedVerifiers.VerifyIfElementIsNotVisibleWithWaitsAsync(pr_xPage, pr_xPOs.BTN_UNLIKE_TWEET);
    }
    
    public async Task UnlikeAllTweetsAsync()
    {
        List<ILocator> unlikeTweetButtons = await pr_xtaWebUISharedActions.GetListOfLocatorsAsync(pr_xPage, pr_xPOs.BTN_UNLIKE_TWEET);
     
        for (int k = 0; k < unlikeTweetButtons.Count; k++)
            await pr_xtaWebUISharedActions.ClickAsync(unlikeTweetButtons.ElementAt(k));
        
        await pr_xtaWebUISharedVerifiers.VerifyIfElementIsNotVisibleWithWaitsAsync(pr_xPage, pr_xPOs.BTN_UNLIKE_TWEET);
    }
    
    #endregion Introduce actions

    #region Introduce verifications

    public async Task VerifyIfFirstTweetSuccessfullyLikedAsync()
    {
        List<ILocator> likeTweetButtons = await m_GetListOfLikeTweetButtonsAsync();

        await pr_xtaWebUISharedVerifiers.VerifyIfElementAttributeMatchedAsync(
            likeTweetButtons.First(), "data-testid", "unlike");
    }

    public async Task VerifyIfTweetsSuccessfullyLikedAsync(int in_numberOfTweets)
    {
        List<ILocator> likeTweetButtons = await m_GetListOfLikeTweetButtonsAsync();

        for (int k = 0; k < in_numberOfTweets; k++)
        {
            await pr_xtaWebUISharedVerifiers.VerifyIfElementAttributeMatchedAsync(
                likeTweetButtons.ElementAt(k), "data-testid", "unlike");
        }
    }

    #endregion Introduce verifications

    #region Private services

    private async Task<List<ILocator>> m_GetListOfLikeTweetButtonsAsync()
        => await pr_xtaWebUISharedActions.GetListOfLocatorsAsync(pr_xPage, pr_xPOs.BTN_LIKE_OR_UNLIKE_TWEET);

    #endregion Private services
} 