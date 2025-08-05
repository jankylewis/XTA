using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

public class XEnterYourPasswordMOs : AXMOs
{
    public XEnterYourPasswordMOs() {}
    internal String TXT_PASSWORD 
        => pr_xPOSharedUtils.BuildSelector("input[name='password']", ELocatingMechanism.CSS);

    internal String BTN_REVEAL_PASSWORD 
        => pr_xPOSharedUtils.BuildSelector("button[aria-label='Reveal password']", ELocatingMechanism.CSS);

    internal String BTN_LOG_IN 
        => pr_xPOSharedUtils.BuildSelector("button[data-testid='LoginForm_Login_Button']", ELocatingMechanism.CSS);
}