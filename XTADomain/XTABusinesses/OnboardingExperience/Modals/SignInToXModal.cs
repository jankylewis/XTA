using Microsoft.Playwright;
using XTADomain.XTABusinesses.Abstractions;
using XTADomain.XTAPageObjects.OnboardingExperience.Modals;

namespace XTADomain.XTABusinesses.OnboardingExperience.Modals;

public class SignInToXModal : AXModal
{
    #region Introduce constructors

    public SignInToXModal(IPage in_page) => prot_page = in_page;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly SignInToXMOs m_ro_SIGN_IN_TO_X_MOS = new();

    #endregion Introduce class vars
    
    #region Introduce actions

    public async Task InputUsername(String in_username) =>
        await prot_ro_XTA_WEBUI_SHARED_ACTIONS.FillTextAsync(prot_page, m_ro_SIGN_IN_TO_X_MOS.TXT_USERNAME, in_username);

    public async Task ClickOnNextButton() =>
        await prot_ro_XTA_WEBUI_SHARED_ACTIONS.ClickAsync(prot_page, m_ro_SIGN_IN_TO_X_MOS.BTN_NEXT);

    public async Task VerifyIfSignInToXModalDisplayed()
    {
        await prot_ro_XTA_WEBUI_SHARED_VERIFIERS.VerifyIfElementIsVisibleWithWaits(prot_page, m_ro_SIGN_IN_TO_X_MOS.LBL_HEADING);
        await prot_ro_XTA_WEBUI_SHARED_VERIFIERS.VerifyIfElementIsVisibleWithWaits(prot_page, m_ro_SIGN_IN_TO_X_MOS.BTN_SIGN_IN_WITH_APPLE);
    }
    
    #endregion Introduce actions
}