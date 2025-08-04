using XTACore.XTAUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

internal class XAppleOAuthMOs
{
    public XAppleOAuthMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = XSingletonFactory.s_DaVinci<XPOSharedUtils>();
    
    internal String IC_X_APP_LOGO 
        => mr_xPOSharedUtils.BuildSelector("#signin img", ELocatingMechanism.CSS);

    internal String TXT_APPLE_ID_EMAIL =>
        mr_xPOSharedUtils.BuildSelector("account_name_text_field", ELocatingMechanism.ID);
}