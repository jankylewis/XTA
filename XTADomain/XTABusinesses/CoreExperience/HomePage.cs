using Microsoft.Playwright;
using XTADomain.XTABusinesses.Abstractions;
using XTADomain.XTAPageObjects.CoreExperience;

namespace XTADomain.XTABusinesses.CoreExperience;

public class HomePage : AXPage
{
    #region Introduce constructors

    public HomePage(IPage in_page) => prot_page = in_page;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly HomePOs m_ro_HOME_POS = new();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task ClickOnProfileNav() => 
        await ClickOnSharedNav(m_ro_HOME_POS.SHARED_NAV(m_ro_HOME_POS.ro_PROFILE_NAV));

    #endregion Introduce actions
}