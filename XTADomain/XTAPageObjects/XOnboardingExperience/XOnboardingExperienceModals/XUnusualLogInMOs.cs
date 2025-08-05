using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

public class XUnusualLogInMOs : AXMOs
{
    public XUnusualLogInMOs() {}

    internal String TXT_PHONE_OR_EMAIL 
        => pr_xPOSharedUtils.BuildSelector("input[data-testid='ocfEnterTextTextInput']", ELocatingMechanism.CSS);

    internal String BTN_NEXT
        => pr_xPOSharedUtils.BuildSelector("button[data-testid='ocfEnterTextNextButton']", ELocatingMechanism.CSS);
}