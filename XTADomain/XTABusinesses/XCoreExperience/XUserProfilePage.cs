using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience.XHomeExperienceModals;
using XTADomain.XTAModels.XCoreExperience;
using XTADomain.XTAPageObjects.XCoreExperience;

namespace XTADomain.XTABusinesses.XCoreExperience;

public class XUserProfilePage(IPage in_xPage) 
    : AXPage<XUserProfilePOs>(in_xPage, XSingletonFactory.s_DaVinci<XUserProfilePOs>())
{
    #region Introduce verifications

    public async Task VerifyDisplayNameShownCorrectlyAsync(String in_expectedXDisplayName)
    {
        await pr_xtaWebUIWaitStrategies
            .WaitForElementToBeVisibleAsync(pr_xPage, pr_xPOs.LBL_USER_DISPLAY_NAME(in_expectedXDisplayName));
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(pr_xPage, pr_xPOs.LBL_USER_DISPLAY_NAME(in_expectedXDisplayName));
    }

    public async Task VerifyUsernameShownCorrectlyAsync(String in_expectedXUsername) 
        => await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(pr_xPage, pr_xPOs.LBL_USERNAME(in_expectedXUsername));

    public async Task VerifyAnXPostSuccessfullyCreated(XTweetModel in_xTweetModel)
    {
        const string NUMBER_OF_POSTS = "1 post";
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfTextContentMatchedAsync(pr_xPage, pr_xPOs.LBL_NUMBER_OF_TWEETS, NUMBER_OF_POSTS);

        await pr_xtaWebUISharedVerifiers
            .VerifyIfTextContentMatchedAsync(pr_xPage, pr_xPOs.LBl_TWEET_CONTENT, in_xTweetModel.TweetContent);
    }
    
    #endregion Introduce verifications

    #region Introduce actions

    public async Task ClickOnDeleteBtnAsync()
    {
        await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_TWEET_MENU);
        await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_DELETE_TWEET);
    }

    public async Task<bool> DeleteAllTweetsAsync()
    {
        await pr_xtaWebUIWaitStrategies.WaitForElementToBeVisibleAsync(pr_xPage, pr_xPOs.IMG_USER_AVATAR);
        
        async Task<IList<ILocator>> i_GetListOfTweetMenuBtns() =>
            await pr_xtaWebUISharedActions.GetListOfLocatorsAsync(pr_xPage, pr_xPOs.BTN_TWEET_MENU);
        
        IList<ILocator> tweetMenuBtns = await i_GetListOfTweetMenuBtns();

        await XSingletonFactory.s_DaVinci<XRetryUtils>()
            .RetryAsync(
                async () => tweetMenuBtns = await i_GetListOfTweetMenuBtns(),
                () => tweetMenuBtns.Count is 0,
                5,
                140
        );
        
        XDeletePostModal xDeletePostModal = new (pr_xPage);
        
        int tweetIdx = 0;
        while (tweetIdx < tweetMenuBtns.Count)
        {
            await pr_xtaWebUISharedActions.ClickAsync(tweetMenuBtns.First());
            await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_DELETE_TWEET);

            await xDeletePostModal.ClickOnDeletePostBtnAsync();
            
            tweetIdx++;
        }

        await pr_xtaWebUISharedActions.ReloadPageAsync(pr_xPage);

        return (await i_GetListOfTweetMenuBtns()).Count is 0;
    }

    public async Task<bool> CheckWhetherAnyTweet()
    {
        await pr_xtaWebUIWaitStrategies.WaitForElementToBeVisibleAsync(pr_xPage, pr_xPOs.LBL_NUMBER_OF_TWEETS);
        
        int numberOfTweets 
            = int.Parse((await pr_xtaWebUISharedActions.GetTextContentAsync(pr_xPage, pr_xPOs.LBL_NUMBER_OF_TWEETS))
                .Split(" ")[0]);

        return numberOfTweets != 0;
    }
    
    #endregion Introduce actions
}