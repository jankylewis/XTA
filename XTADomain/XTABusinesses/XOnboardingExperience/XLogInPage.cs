using Microsoft.Playwright;
using XTADomain.XTAPageObjects.XOnboardingExperience;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XOnboardingExperience;

public class XLogInPage
{
    #region Introduce constructors
    
    public XLogInPage(IPage in_xPage) => m_xPage = in_xPage;

    #endregion Introduce constructors

    #region Introduce class vars

    private IPage m_xPage;
    private readonly XLogInPOs mr_xLogInPOs = new();
    
    private readonly XTAWebUISharedActions mr_xtaWebUISharedActions = new();
    private readonly XTAWebUIWaitStrategies mr_xtaWebUIWaitStrategies = new();
    private readonly XTAWebUIJSWaitStrategies mr_xtaWebUIJSWaitStrategies = new();
    
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
        => await mr_xtaWebUISharedActions.ClickAsync(m_xPage, mr_xLogInPOs.BTN_SIGN_IN);

    public async Task ClickOnSignInWithAppleBtnAsync()
    {
        await _WaitForXOAuthBtnToBeReadyAsync(mr_xLogInPOs.BTN_SIGN_IN_WITH_APPLE);
        await mr_xtaWebUISharedActions.ClickAsync(m_xPage, mr_xLogInPOs.BTN_SIGN_IN_WITH_APPLE);
    }
    
    public Task<IPage> GenXOAuthPopupListener() 
        => m_xPage.WaitForPopupAsync(new PageWaitForPopupOptions 
        {
            Timeout = 2700
        });

    private async Task _WaitForXOAuthBtnToBeReadyAsync(string in_xOAuthBtnSelector)
    {
        await mr_xtaWebUIJSWaitStrategies.WaitForJavaScriptFunctionAsync(m_xPage, mr_appleSDKExistenceCheckJSFunc);
        await mr_xtaWebUIWaitStrategies.WaitForElementToBeClickableAsync(m_xPage, in_xOAuthBtnSelector);
    }

    public async Task ClickOnSignInWithGoogleBtnAsync()
    {
        await mr_xtaWebUIWaitStrategies.WaitForElementToBeClickableAsync(m_xPage, mr_xLogInPOs.BTN_SIGN_IN_WITH_GOOGLE);
        await mr_xtaWebUISharedActions.ClickAsync(m_xPage, mr_xLogInPOs.BTN_SIGN_IN_WITH_GOOGLE);
    }
    
    #endregion Introduce actions
}