using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience;

internal class XLogInPOs
{
    internal XLogInPOs() {}

    private readonly XPOSharedUtils mr_xPOSharedUtils = new();

    internal String BTN_SIGN_IN 
        => mr_xPOSharedUtils.BuildSelector("a[href='/login'] div", ELocatingMechanism.CSS);
}