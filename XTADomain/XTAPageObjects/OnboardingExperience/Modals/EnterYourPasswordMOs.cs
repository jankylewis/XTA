using XTADomain.XTAPageObjects.POUtils;

namespace XTADomain.XTAPageObjects.OnboardingExperience.Modals;

internal class EnterYourPasswordMOs
{
    internal EnterYourPasswordMOs() {}
    
    private readonly POSharedUtils m_ro_PO_SHARED_UTILS = new();

    internal String TXT_PASSWORD => m_ro_PO_SHARED_UTILS.BuildSelector("input[name='password']", ELocatingMechanism.CSS);

    internal String BTN_REVEAL_PASSWORD =>
        m_ro_PO_SHARED_UTILS.BuildSelector("button[aria-label='Reveal password']", ELocatingMechanism.CSS);

    internal String BTN_LOG_IN =>
        m_ro_PO_SHARED_UTILS.BuildSelector("button[data-testid='LoginForm_Login_Button']", ELocatingMechanism.CSS);
}