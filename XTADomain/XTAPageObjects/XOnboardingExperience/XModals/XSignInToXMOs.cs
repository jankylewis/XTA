using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOnboardingExperience.XModals;

internal class XSignInToXMOs
{
    internal XSignInToXMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = new();

    internal String TXT_USERNAME 
        => mr_xPOSharedUtils.BuildSelector("input[autocomplete='username'][autocapitalize='sentences']"
            , ELocatingMechanism.CSS);

    internal String BTN_NEXT 
        => mr_xPOSharedUtils.BuildSelector("//button[div//span[text()='Next']]"
            , ELocatingMechanism.XPATH);
    
    internal String LBL_HEADING 
        => mr_xPOSharedUtils.BuildSelector("modal-header", ELocatingMechanism.ID);

    internal String BTN_SIGN_IN_WITH_APPLE 
        => mr_xPOSharedUtils.BuildSelector("div[aria-modal] button[data-testid='apple_sign_in_button']"
            , ELocatingMechanism.CSS);
}