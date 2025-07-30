using XTACore.XTAUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XPOAbstractions;

internal abstract class AXPOs
{
    #region Introduce vars

    protected XPOSharedUtils p_xPOSharedUtils = XSingletonFactory.s_DaVinciResolve<XPOSharedUtils>();
    
    #endregion Introduce vars

    #region Introduce shared UI elements

    #region Introduce banner elements

    internal String LNK_HOME 
        => p_xPOSharedUtils.BuildSelector("a[data-testid='AppTabBar_Home_Link']", ELocatingMechanism.CSS);

    internal String BTN_POST 
        => p_xPOSharedUtils.BuildSelector("a[data-testid='SideNav_NewTweet_Button']", ELocatingMechanism.CSS);

    internal String TXTA_POST_CONTENT
        => p_xPOSharedUtils.BuildSelector("div[aria-modal] div[data-testid='tweetTextarea_0']", ELocatingMechanism.CSS);

    internal String BTN_POST_TWEET 
        => p_xPOSharedUtils.BuildSelector("button[data-testid='tweetButton']", ELocatingMechanism.CSS);

    #endregion Introduce banner elements
    
    internal String SHARED_NAV(String in_xProfile) 
        => p_xPOSharedUtils.BuildSelector($"nav[role='navigation'] a[data-testid='{in_xProfile}']"
            , ELocatingMechanism.CSS);

    #endregion Introduce shared UI elements

    #region Introduce element utilities

    internal readonly String r_profileNav = "AppTabBar_Profile_Link";
    internal readonly String r_notiNav = "AppTabBar_Notifications_Link";

    #endregion Introduce element utilities
}