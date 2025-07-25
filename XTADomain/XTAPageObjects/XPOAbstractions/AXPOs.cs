using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XPOAbstractions;

internal abstract class AXPOs
{
    #region Introduce vars

    protected XPOSharedUtils p_xPOSharedUtils = new();
    
    #endregion Introduce vars

    #region Introduce shared UI elements

    internal String SHARED_NAV(String in_xProfile) 
        => p_xPOSharedUtils.BuildSelector($"nav[role='navigation'] a[data-testid='{in_xProfile}']"
            , ELocatingMechanism.CSS);

    #endregion Introduce shared UI elements

    #region Introduce element utilities

    internal readonly String r_profileNav = "AppTabBar_Profile_Link";
    internal readonly String r_notiNav = "AppTabBar_Notifications_Link";

    #endregion Introduce element utilities
}