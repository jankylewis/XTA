using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XBusinessAbstractions;

public abstract class AXPage<XPOs> where XPOs : AXPOs
{
    #region Introduce constructors

    protected AXPage(IPage in_xPage, XPOs in_xPOs)
    {
        pr_xPage = in_xPage;
        pr_xPOs = in_xPOs;
    }

    #endregion Introduce constructors
    
    #region Introduce shared vars
    
    protected readonly IPage pr_xPage;
    protected readonly XPOs pr_xPOs;
    
    protected readonly XTAWebUISharedActions pr_xtaWebUISharedActions = XSingletonFactory.s_DaVinci<XTAWebUISharedActions>();
    protected readonly XTAWebUISharedVerifiers pr_xtaWebUISharedVerifiers = XSingletonFactory.s_DaVinci<XTAWebUISharedVerifiers>();
    protected readonly XTAWebUIWaitStrategies pr_xtaWebUIWaitStrategies = XSingletonFactory.s_DaVinci<XTAWebUIWaitStrategies>();
    
    #endregion Introduce shared vars
    
    #region Introduce X-shared actions

    public async Task ClickOnSharedNavAsync(String in_expectedNav) 
        => await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.SHARED_NAV(in_expectedNav));

    public async Task ClickOnPostBtnAsync() 
        => await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_POST);

    public async Task FillPostContentAsync(string in_expPostContent)
        => await pr_xtaWebUISharedActions.FillTextAsync(pr_xPage, pr_xPOs.TXTA_TWEET_CONTENT, in_expPostContent);

    public async Task ClickOnPostTweetBtnAsync()
        => await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_TWEET);
    
    #endregion Introduce X-shared actions
}