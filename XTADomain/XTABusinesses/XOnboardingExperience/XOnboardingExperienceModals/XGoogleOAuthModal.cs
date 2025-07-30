using Microsoft.Playwright;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;
using XTAPlaywright.XConstHouse;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XGoogleOAuthModal : AXModal
{
    #region Introduce constructors

    public XGoogleOAuthModal(IPage in_xGoogleOAuthModal) => mr_xGoogleOAuthModal = in_xGoogleOAuthModal;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly IPage mr_xGoogleOAuthModal = default!;
    
    private readonly XGoogleOAuthMOs mr_xGoogleOAuthMOs = new();

    #endregion Introduce class vars

    #region Introduce verifications

    public async Task VerifyXOAuthGoogleModalPresentedAsync()
    {
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithWaitsAsync(
                
                mr_xGoogleOAuthModal, 
                mr_xGoogleOAuthMOs.TXT_EMAIL,
                new LocatorAssertionsToBeVisibleOptions
                {
                    Timeout = XTimedoutConsts.MED_ELEMENT_TIMEOUT_MS
                }
            );
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(mr_xGoogleOAuthModal, mr_xGoogleOAuthMOs.BTN_CREATE_ACCOUNT);
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(mr_xGoogleOAuthModal, mr_xGoogleOAuthMOs.BTN_FORGOT_EMAIL);

        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsClickableAsync(mr_xGoogleOAuthModal, mr_xGoogleOAuthMOs.BTN_NEXT);
    } 

    #endregion Introduce verifications
}