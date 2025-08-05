using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XUnusualLogInModal(IPage in_xUnusualLogInModal)
    : AXModal<XUnusualLogInMOs>(in_xUnusualLogInModal, XSingletonFactory.s_DaVinci<XUnusualLogInMOs>())
{
    #region Introduce verifications

    public async Task<bool> VerifyWhetherXUnusualLogInModalPresented()
    {
        await pr_xtaWebUIWaitStrategies.WaitForElementToBeVisibleAsync(pr_xPage, pr_xPOs.LBL_HEADING);
        
        string modalHeaderContent 
            = (await pr_xtaWebUISharedActions.GetTextContentAsync(pr_xPage, pr_xPOs.LBL_HEADING)).Trim();

        return modalHeaderContent == "Enter your phone number or email address";
    }

    #endregion Introduce verifications
    
    #region Introduce actions
    
    public async Task InputEmailAsync(string in_email)
        => await pr_xtaWebUISharedActions.FillTextAsync(pr_xPage, pr_xPOs.TXT_PHONE_OR_EMAIL, in_email);

    public async Task ClickOnNextBtnAsync()
        => await pr_xtaWebUISharedActions.ClickAsync(pr_xPage, pr_xPOs.BTN_NEXT);

    #endregion Introduce actions
}