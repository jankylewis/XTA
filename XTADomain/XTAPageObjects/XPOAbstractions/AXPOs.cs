using XTACore.XTAUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XPOAbstractions;

public abstract class AXPOs
{
    #region Introduce vars

    protected readonly XPOSharedUtils pr_xPOSharedUtils = XSingletonFactory.s_DaVinci<XPOSharedUtils>();
    
    #endregion Introduce vars

    #region Introduce shared UI elements

    #region Introduce Banner elements

    internal String LNK_HOME 
        => pr_xPOSharedUtils.BuildSelector("a[data-testid='AppTabBar_Home_Link']", ELocatingMechanism.CSS);

    internal String BTN_POST 
        => pr_xPOSharedUtils.BuildSelector("a[data-testid='SideNav_NewTweet_Button']", ELocatingMechanism.CSS);

    internal String TXTA_TWEET_CONTENT
        => pr_xPOSharedUtils.BuildSelector("div[aria-modal] div[data-testid='tweetTextarea_0']", ELocatingMechanism.CSS);

    internal String BTN_TWEET
        => pr_xPOSharedUtils.BuildSelector("button[data-testid='tweetButton']", ELocatingMechanism.CSS);

    #endregion Introduce Banner elements
    
    internal String SHARED_NAV(String in_xProfile) 
        => pr_xPOSharedUtils.BuildSelector($"nav[role='navigation'] a[data-testid='{in_xProfile}']", ELocatingMechanism.CSS);

    #region Introduce Account Switcher elements
    
    internal String BTN_ACCOUNT_SWITCHER
        => pr_xPOSharedUtils.BuildSelector("button[data-testid='SideNav_AccountSwitcher_Button']", ELocatingMechanism.CSS);

    internal String BTN_LOGOUT
        => pr_xPOSharedUtils.BuildSelector("a[data-testid='AccountSwitcher_Logout_Button']", ELocatingMechanism.CSS);

    #endregion Introduce Account Switcher elements
    
    #endregion Introduce shared UI elements

    #region Introduce element utilities

    internal readonly String r_profileNav = "AppTabBar_Profile_Link";
    internal readonly String r_notiNav = "AppTabBar_Notifications_Link";

    #endregion Introduce element utilities
}