using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XSignInToXModal(IPage in_xPage) 
    : AXModal<XSignInToXMOs>(in_xPage, XSingletonFactory.s_DaVinci<XSignInToXMOs>())
{
    #region Introduce actions

    public async Task InputUsernameAsync(String in_username) 
        => await pr_xtaWebUISharedActions.FillTextAsync(pr_xPage, pr_xPOs.TXT_USERNAME, in_username);

    public async Task ClickOnNextBtnAsync()
        => await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_NEXT);

    public async Task VerifyIfSignInToXModalDisplayedAsync()
    {
        await pr_xtaWebUISharedVerifiers.VerifyIfElementIsVisibleWithWaitsAsync(pr_xPage, pr_xPOs.LBL_HEADING);
        await pr_xtaWebUISharedVerifiers.VerifyIfElementIsVisibleWithWaitsAsync(pr_xPage, pr_xPOs.BTN_SIGN_IN_WITH_APPLE);
    }
    
    #endregion Introduce actions
}