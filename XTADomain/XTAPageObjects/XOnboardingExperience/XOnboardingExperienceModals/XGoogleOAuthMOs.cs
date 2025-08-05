using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

public class XGoogleOAuthMOs : AXMOs
{
    public XGoogleOAuthMOs() {}
    
    internal string TXT_EMAIL 
        => pr_xPOSharedUtils.BuildSelector("identifierId", ELocatingMechanism.ID);
    
    internal String BTN_FORGOT_EMAIL 
        => pr_xPOSharedUtils.BuildSelector("//button[text()='Forgot email?']", ELocatingMechanism.XPATH);
    
    internal String BTN_NEXT 
        => pr_xPOSharedUtils.BuildSelector("//button[span[text()='Next']]", ELocatingMechanism.XPATH);
    
    internal String BTN_CREATE_ACCOUNT
        => pr_xPOSharedUtils.BuildSelector("//button[span[text()='Create account']]", ELocatingMechanism.XPATH);
}