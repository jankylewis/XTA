using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XCoreExperience.XHomExperience;

namespace XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;

public class XHomePage : AXPage
{
    #region Introduce constructors

    public XHomePage(IPage in_xPage) => p_xPage = in_xPage;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly XHomePOs mr_xHomePOs = XSingletonFactory.s_DaVinciResolve<XHomePOs>();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task ClickOnProfileNavAsync() 
        => await p_ClickOnSharedNavAsync(mr_xHomePOs.SHARED_NAV(mr_xHomePOs.r_profileNav));
    
    #endregion Introduce actions
}