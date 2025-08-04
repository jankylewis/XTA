using XTACore.XTAUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

internal class XEnterYourPasswordMOs
{
    internal XEnterYourPasswordMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = XSingletonFactory.s_DaVinci<XPOSharedUtils>();

    internal String TXT_PASSWORD 
        => mr_xPOSharedUtils.BuildSelector("input[name='password']", ELocatingMechanism.CSS);

    internal String BTN_REVEAL_PASSWORD 
        => mr_xPOSharedUtils.BuildSelector("button[aria-label='Reveal password']", ELocatingMechanism.CSS);

    internal String BTN_LOG_IN 
        => mr_xPOSharedUtils.BuildSelector("button[data-testid='LoginForm_Login_Button']", ELocatingMechanism.CSS);
}