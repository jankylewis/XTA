using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;
using XTAInfras.XConstHouse;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XAppleOAuthModal(IPage in_xAppleOAuthModal) 
    : AXModal<XAppleOAuthMOs>(in_xAppleOAuthModal, XSingletonFactory.s_DaVinci<XAppleOAuthMOs>())
{
    #region Introduce verifications

    public async Task VerifyXAppleOAuthModalPresentedAsync()
    {
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithWaitsAsync(
                
                pr_xPage, 
                pr_xPOs.IC_X_APP_LOGO,
                new LocatorAssertionsToBeVisibleOptions
                {
                    Timeout = XTimedoutConsts.MED_ELEMENT_TIMEOUT_MS
                }
                );
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithWaitsAsync(pr_xPage, pr_xPOs.TXT_APPLE_ID_EMAIL);
    }

    #endregion Introduce verifications
}