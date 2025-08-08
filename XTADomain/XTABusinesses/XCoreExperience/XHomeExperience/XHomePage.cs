using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XCoreExperience.XHomExperience;

namespace XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;

public class XHomePage(IPage in_xPage) : AXPage<XHomePOs>(in_xPage, XSingletonFactory.s_DaVinci<XHomePOs>())
{
    #region Introduce actions

    public async Task ClickOnProfileNavAsync() 
        => await ClickOnSharedNavAsync(pr_xPOs.r_profileNav);
    
    #endregion Introduce actions
}