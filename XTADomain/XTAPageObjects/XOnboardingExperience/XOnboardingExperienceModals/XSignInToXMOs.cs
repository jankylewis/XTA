using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

public class XSignInToXMOs : AXMOs
{
    public XSignInToXMOs() {}
    
    internal String TXT_USERNAME 
        => pr_xPOSharedUtils.BuildSelector("input[autocomplete='username'][autocapitalize='sentences']"
            , ELocatingMechanism.CSS);

    internal String BTN_NEXT 
        => pr_xPOSharedUtils.BuildSelector("//button[div//span[text()='Next']]"
            , ELocatingMechanism.XPATH);

    internal String BTN_SIGN_IN_WITH_APPLE 
        => pr_xPOSharedUtils.BuildSelector("div[aria-modal] button[data-testid='apple_sign_in_button']"
            , ELocatingMechanism.CSS);
}