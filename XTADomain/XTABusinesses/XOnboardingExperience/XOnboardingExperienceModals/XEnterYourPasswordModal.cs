using Microsoft.Playwright;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XEnterYourPasswordModal : AXModal
{
    #region Introduce constructors

    public XEnterYourPasswordModal(IPage in_xPage) => p_xPage = in_xPage;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly XEnterYourPasswordMOs mr_xEnterYourPasswordMOs = new();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task InputPasswordAsync(String in_password) 
        => await pr_xtaWebUISharedActions.FillTextAsync(p_xPage, mr_xEnterYourPasswordMOs.TXT_PASSWORD, in_password);

    public async Task ClickOnLogInBtnAsync() 
        => await pr_xtaWebUISharedActions.ClickAsync(p_xPage, mr_xEnterYourPasswordMOs.BTN_LOG_IN);

    #endregion Introduce actions
}