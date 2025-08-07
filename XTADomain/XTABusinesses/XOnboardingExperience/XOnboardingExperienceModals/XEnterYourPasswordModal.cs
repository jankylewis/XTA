using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XEnterYourPasswordModal(IPage in_xPage) 
    : AXModal<XEnterYourPasswordMOs>(in_xPage, XSingletonFactory.s_DaVinci<XEnterYourPasswordMOs>())
{
    #region Introduce actions

    public async Task InputPasswordAsync(String in_password) 
        => await pr_xtaWebUISharedActions.FillTextAsync(pr_xPage, pr_xPOs.TXT_PASSWORD, in_password);

    public async Task ClickOnLogInBtnAsync() 
        => await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_LOG_IN);

    #endregion Introduce actions
}