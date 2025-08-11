using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;
using XTAInfras.XInfrasConstHouse;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XGoogleOAuthModal(IPage in_xPage) 
    : AXModal<XGoogleOAuthMOs>(in_xPage, XSingletonFactory.s_DaVinci<XGoogleOAuthMOs>())
{
    #region Introduce verifications

    public async Task VerifyXOAuthGoogleModalPresentedAsync()
    {
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithWaitsAsync(
                
                pr_xPage, 
                pr_xPOs.TXT_EMAIL,
                new LocatorAssertionsToBeVisibleOptions
                {
                    Timeout = XTimedoutConsts.MED_ELEMENT_TIMEOUT_MS
                }
            );
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(pr_xPage, pr_xPOs.BTN_CREATE_ACCOUNT);
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(pr_xPage, pr_xPOs.BTN_FORGOT_EMAIL);

        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsClickableAsync(pr_xPage, pr_xPOs.BTN_NEXT);
    } 

    #endregion Introduce verifications
}