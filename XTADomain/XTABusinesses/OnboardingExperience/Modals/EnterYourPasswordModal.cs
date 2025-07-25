using Microsoft.Playwright;
using XTADomain.XTABusinesses.Abstractions;
using XTADomain.XTAPageObjects.OnboardingExperience.Modals;

namespace XTADomain.XTABusinesses.OnboardingExperience.Modals;

public class EnterYourPasswordModal : AXModal
{
    #region Introduce constructors

    public EnterYourPasswordModal(IPage in_page) => prot_page = in_page;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly EnterYourPasswordMOs m_ro_ENTER_YOUR_PASSWORD_MOS = new();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task InputPassword(String in_password) => 
        await prot_ro_XTA_WEBUI_SHARED_ACTIONS.FillTextAsync(prot_page, m_ro_ENTER_YOUR_PASSWORD_MOS.TXT_PASSWORD, in_password);

    public async Task ClickOnLogInButton() =>
        await prot_ro_XTA_WEBUI_SHARED_ACTIONS.ClickAsync(prot_page, m_ro_ENTER_YOUR_PASSWORD_MOS.BTN_LOG_IN);

    #endregion Introduce actions
}