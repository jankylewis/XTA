using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

public class XAppleOAuthMOs : AXMOs
{
    public XAppleOAuthMOs() {}
    
    internal String IC_X_APP_LOGO 
        => pr_xPOSharedUtils.BuildSelector("#signin img", ELocatingMechanism.CSS);

    internal String TXT_APPLE_ID_EMAIL 
        => pr_xPOSharedUtils.BuildSelector("account_name_text_field", ELocatingMechanism.ID);
}