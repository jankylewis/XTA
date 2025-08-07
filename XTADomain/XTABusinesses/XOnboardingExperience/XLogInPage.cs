using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTAPageObjects.XOnboardingExperience;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XOnboardingExperience;

public class XLogInPage(IPage in_xPage)
{
    #region Introduce class vars
    
    private readonly XLogInPOs mr_xLogInPOs = XSingletonFactory.s_DaVinci<XLogInPOs>();
    
    private readonly XTAWebUISharedActions mr_xtaWebUISharedActions = XSingletonFactory.s_DaVinci<XTAWebUISharedActions>();
    private readonly XTAWebUIWaitStrategies mr_xtaWebUIWaitStrategies = XSingletonFactory.s_DaVinci<XTAWebUIWaitStrategies>();
    private readonly XTAWebUIJSWaitStrategies mr_xtaWebUIJSWaitStrategies = XSingletonFactory.s_DaVinci<XTAWebUIJSWaitStrategies>();
    
    private readonly string mr_appleSDKExistenceCheckJSFunc 
        = "() => window.AppleID !== undefined && typeof window.AppleID.auth === 'object'";

    private readonly string mr_appleSDKReadyCheckJSFunc
        = @"() => {
                const el = document.querySelector('[data-appleid-state=""ready""]');
                return !!el && window.AppleID?.auth !== undefined;
            }";
    #endregion Introduce class vars

    #region Introduce actions

    public async Task ClickOnSignInBtnAsync() 
        => await mr_xtaWebUISharedActions.ClickAsync(in_xPage, mr_xLogInPOs.BTN_SIGN_IN);

    public async Task ClickOnSignInWithAppleBtnAsync()
    {
        await m_WaitForXOAuthBtnToBeReadyAsync(mr_xLogInPOs.BTN_SIGN_IN_WITH_APPLE);
        await mr_xtaWebUISharedActions.ClickAsync(in_xPage, mr_xLogInPOs.BTN_SIGN_IN_WITH_APPLE);
    }
    
    public Task<IPage> GenXOAuthPopupListener() 
        => in_xPage.WaitForPopupAsync(new PageWaitForPopupOptions 
        {
            Timeout = 2700
        });

    private async Task m_WaitForXOAuthBtnToBeReadyAsync(string in_xOAuthBtnSelector)
    {
        await mr_xtaWebUIJSWaitStrategies.WaitForJSFuncAsync(in_xPage, mr_appleSDKExistenceCheckJSFunc);
        await mr_xtaWebUIWaitStrategies.WaitForElementToBeClickableAsync(in_xPage, in_xOAuthBtnSelector);
    }

    public async Task ClickOnSignInWithGoogleBtnAsync()
    {
        await mr_xtaWebUIWaitStrategies.WaitForElementToBeClickableAsync(in_xPage, mr_xLogInPOs.BTN_SIGN_IN_WITH_GOOGLE);
        await mr_xtaWebUISharedActions.ClickAsync(in_xPage, mr_xLogInPOs.BTN_SIGN_IN_WITH_GOOGLE);
    }
    
    #endregion Introduce actions
}