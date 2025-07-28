using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XModals;

internal class XGoogleOAuthMOs
{
    internal XGoogleOAuthMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = new();

    internal string TXT_EMAIL => mr_xPOSharedUtils.BuildSelector("identifierId", ELocatingMechanism.ID);
    internal String BTN_FORGOT_EMAIL 
        => mr_xPOSharedUtils.BuildSelector("//button[text()='Forgot email?']", ELocatingMechanism.XPATH);
    internal String BTN_NEXT 
        => mr_xPOSharedUtils.BuildSelector("//button[span[text()='Next']]", ELocatingMechanism.XPATH);
    internal String BTN_CREATE_ACCOUNT
        => mr_xPOSharedUtils.BuildSelector("//button[span[text()='Create account']]", ELocatingMechanism.XPATH);
}