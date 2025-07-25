using Microsoft.Playwright;
using XTADomain.XTAPageObjects.OnboardingExperience;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.OnboardingExperience;

public class LogInPage
{
    #region Introduce constructors

    public LogInPage(IPage in_page) => m_page = in_page;

    #endregion Introduce constructors

    #region Introduce class vars

    private IPage m_page;
    private readonly LogInPOs m_ro_LOG_IN_POS = new();
    private readonly XTAWebUISharedActions m_ro_XTA_WEBUI_SHARED_ACTIONS = new();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task ClickOnSignInButton() => 
        await m_ro_XTA_WEBUI_SHARED_ACTIONS.ClickAsync(m_page, m_ro_LOG_IN_POS.BTN_SIGN_IN);
    
    #endregion Introduce actions
}