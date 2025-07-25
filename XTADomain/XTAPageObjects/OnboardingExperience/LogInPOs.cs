using XTADomain.XTAPageObjects.POUtils;

namespace XTADomain.XTAPageObjects.OnboardingExperience;

internal class LogInPOs
{
    internal LogInPOs() {}

    private readonly POSharedUtils m_ro_PO_SHARED_UTILS = new();

    internal String BTN_SIGN_IN => m_ro_PO_SHARED_UTILS.BuildSelector("a[href='/login'] div", 
        ELocatingMechanism.CSS);
}