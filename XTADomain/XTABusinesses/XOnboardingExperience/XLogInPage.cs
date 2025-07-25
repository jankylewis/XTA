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

    #endregion Introduce class vars

    #region Introduce actions

    public async Task ClickOnSignInBtnAsync() 
        => await mr_xtaWebUISharedActions.ClickAsync(m_xPage, mr_xLogInPOs.BTN_SIGN_IN);
    
    #endregion Introduce actions
}