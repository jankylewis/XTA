using XTADomain.XTAPageObjects.POUtils;

namespace XTADomain.XTAPageObjects.OnboardingExperience.Modals;

internal class SignInToXMOs
{
    internal SignInToXMOs() {}
    
    private readonly POSharedUtils m_ro_PO_SHARED_UTILS = new();

    internal String TXT_USERNAME => m_ro_PO_SHARED_UTILS.BuildSelector("input[autocomplete='username'][autocapitalize='sentences']",
            ELocatingMechanism.CSS);

    internal String BTN_NEXT => m_ro_PO_SHARED_UTILS.BuildSelector("//button[div//span[text()='Next']]", 
        ELocatingMechanism.XPATH);
    
    internal String LBL_HEADING => m_ro_PO_SHARED_UTILS.BuildSelector("modal-header", ELocatingMechanism.ID);

    internal String BTN_SIGN_IN_WITH_APPLE =>
        m_ro_PO_SHARED_UTILS.BuildSelector("div[aria-modal] button[data-testid='apple_sign_in_button']", 
            ELocatingMechanism.CSS);
}