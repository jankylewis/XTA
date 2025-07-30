using Microsoft.Playwright;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XSignInToXModal : AXModal
{
    #region Introduce constructors

    public XSignInToXModal(IPage in_xPage) => p_xPage = in_xPage;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly XSignInToXMOs mr_xSignInToXMOs = new();

    #endregion Introduce class vars
    
    #region Introduce actions

    public async Task InputUsernameAsync(String in_username) 
        => await pr_xtaWebUISharedActions.FillTextAsync(p_xPage, mr_xSignInToXMOs.TXT_USERNAME, in_username);

    public async Task ClickOnNextBtnAsync()
        => await pr_xtaWebUISharedActions.ClickAsync(p_xPage, mr_xSignInToXMOs.BTN_NEXT);

    public async Task VerifyIfSignInToXModalDisplayedAsync()
    {
        await pr_xtaWebUISharedVerifiers.VerifyIfElementIsVisibleWithWaitsAsync(p_xPage, mr_xSignInToXMOs.LBL_HEADING);
        await pr_xtaWebUISharedVerifiers.VerifyIfElementIsVisibleWithWaitsAsync(p_xPage, mr_xSignInToXMOs.BTN_SIGN_IN_WITH_APPLE);
    }
    
    #endregion Introduce actions
}