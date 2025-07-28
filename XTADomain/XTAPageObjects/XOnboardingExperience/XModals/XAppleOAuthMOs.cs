using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XModals;

internal class XAppleOAuthMOs
{
    internal XAppleOAuthMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = new();
    
    internal String IC_X_APP_LOGO 
        => mr_xPOSharedUtils.BuildSelector("#signin img", ELocatingMechanism.CSS);

    internal String TXT_APPLE_ID_EMAIL =>
        mr_xPOSharedUtils.BuildSelector("account_name_text_field", ELocatingMechanism.ID);
}