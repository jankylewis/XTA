using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XCoreExperience.XHomExperience;

namespace XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;

public class XHomePage(IPage in_xPage) : AXPage<XHomePOs>(in_xPage, XSingletonFactory.s_DaVinci<XHomePOs>())
{
    #region Introduce class vars

    private readonly XHomePOs mr_xHomePOs = XSingletonFactory.s_Retrieve<XHomePOs>();

    #endregion Introduce class vars
    
    #region Introduce actions

    public async Task ClickOnProfileNavAsync() 
        => await ClickOnSharedNavAsync(mr_xHomePOs.r_profileNav);
    
    #endregion Introduce actions
}