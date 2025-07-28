using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience;

internal class XLogInPOs
{
    internal XLogInPOs() {}

    private readonly XPOSharedUtils mr_xPOSharedUtils = new();

    internal String BTN_SIGN_IN 
        => mr_xPOSharedUtils.BuildSelector("a[href='/login'] div", ELocatingMechanism.CSS);
    
    internal String BTN_SIGN_IN_WITH_APPLE 
        => mr_xPOSharedUtils.BuildSelector("button[data-testid='apple_sign_in_button']", ELocatingMechanism.CSS);
    
    internal String BTN_SIGN_IN_WITH_GOOGLE
        => mr_xPOSharedUtils.BuildSelector("div[data-testid='google_sign_in_container'] iframe", ELocatingMechanism.CSS);
}