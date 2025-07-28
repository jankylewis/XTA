using Microsoft.Playwright;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XModals;
using XTAPlaywright.XConstHouse;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XModals;

public class XAppleOAuthModal : AXModal
{
    #region Introduce constructors

    public XAppleOAuthModal(IPage in_xAppleOAuthModal) => mr_xAppleOAuthModal = in_xAppleOAuthModal;

    #endregion Introduce constructors
    
    #region Introduce class vars
    
    private readonly IPage mr_xAppleOAuthModal = default!;
    
    private readonly XAppleOAuthMOs mr_xAppleOAuthMOs = new();
    
    #endregion Introduce class vars

    #region Introduce verifications

    public async Task VerifyXAppleOAuthModalPresentedAsync()
    {
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithWaitsAsync(
                
                mr_xAppleOAuthModal, 
                mr_xAppleOAuthMOs.IC_X_APP_LOGO,
                new LocatorAssertionsToBeVisibleOptions
                {
                    Timeout = XTimedoutConsts.MED_ELEMENT_TIMEOUT_MS
                }
                );
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithWaitsAsync(mr_xAppleOAuthModal, mr_xAppleOAuthMOs.TXT_APPLE_ID_EMAIL);
    }

    #endregion Introduce verifications
}