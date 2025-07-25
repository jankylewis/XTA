using XTADomain.XTAPageObjects.POUtils;

namespace XTADomain.XTAPageObjects.Abstractions;

internal abstract class APOs
{
    #region Introduce vars

    protected POSharedUtils m_ro_PO_SHARED_UTILS = new();
    
    #endregion Introduce vars

    #region Introduce shared UI elements

    internal String SHARED_NAV(String in_profile) => 
        m_ro_PO_SHARED_UTILS.BuildSelector($"nav[role='navigation'] a[data-testid='{in_profile}']", ELocatingMechanism.CSS);

    #endregion Introduce shared UI elements

    #region Introduce element utilities

    internal readonly String ro_PROFILE_NAV = "AppTabBar_Profile_Link";
    internal readonly String ro_NOTIFICATION_NAV = "AppTabBar_Notifications_Link";

    #endregion Introduce element utilities
}